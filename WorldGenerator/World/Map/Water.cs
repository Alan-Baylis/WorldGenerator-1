using Sean.Shared;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Sean.WorldGenerator
{
    public class River
    {
        private IWorld worldInstance;
        public List<Shared.Position> Coords { get; set; }
        public List<Shared.Position> EmptyCoords { get; set; }
        public Position Source { get; set; }

        private const int WATER = 2; // TODO - stick these somewhere
        private const int GRASS = 4;
        private const int RIVER = 5;

        public River(IWorld world, Position source)
        {
            worldInstance = world;
            Coords = new List<Position>();
            EmptyCoords = new List<Position>();
            Source = source;
            Add(source);
        }
        public void Grow()
        {
            var minPos = new Position(0, Global.CHUNK_HEIGHT, 0);
            foreach(var pos in EmptyCoords)
            {
                if (pos.Y < minPos.Y)
                    minPos = pos;
            }
            Add(minPos);
        }
        public void Add(Position pos)
        {
            if (pos.X == 0 && pos.Z == 0) return;

            if (EmptyCoords.Contains(pos))
                EmptyCoords.Remove(pos);
            Coords.Add(pos);

            var block = new Block(Block.BlockType.WaterSource);
            worldInstance.SetBlock(pos.X, pos.Y, pos.Z, block);
            worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);

            AddIfEmpty(pos.X, pos.Y - 1, pos.Z);
            AddIfEmpty(pos.X+1, pos.Y, pos.Z);
            AddIfEmpty(pos.X-1, pos.Y, pos.Z);
            AddIfEmpty(pos.X, pos.Y, pos.Z+1);
            AddIfEmpty(pos.X, pos.Y, pos.Z-1);
        }
        private void AddIfEmpty(int x,int y,int z)
        {
            if (!worldInstance.GetBlock(x, y, z).IsSolid)
                EmptyCoords.Add(new Position(x, y, z));
        }
    }

    public class Water
    {
        public Water (IWorld world)
        {
            worldInstance = world;
            GenerateRivers();

            //ProcessWater(); // TODO - run from own thread
        }

        private IWorld worldInstance;
        private List<River> Rivers;
        private Thread thread;
        private const int WATER = 2; // TODO - stick these somewhere
        private const int GRASS = 4;
        private const int RIVER = 5;
        private void GenerateRivers()
        {
            Rivers = new List<River>();
            for (var i = 0; i < Settings.RiverCount; i++)
            {
                int x = 0, y = 255, z = 0; // TODO set y
                bool isWater = true;
                while (isWater)
                {
                    x = Settings.Random.Next(worldInstance.GlobalMap.Size.minX, worldInstance.GlobalMap.Size.maxX);
                    z = Settings.Random.Next(worldInstance.GlobalMap.Size.minZ, worldInstance.GlobalMap.Size.maxZ);
                    isWater = (worldInstance.GlobalMapTerrain[x, z] == WATER);
                }
                var pos = new Position(x, y, z);
                var river = new River(worldInstance, pos);
                Rivers.Add(river);
            }

            thread = new Thread(new ThreadStart(StartThread));
            thread.Start();
        }

        private void StartThread()
        {
            while (true)
            {
                foreach (var river in Rivers)
                {
                    river.Grow();
                }
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}

