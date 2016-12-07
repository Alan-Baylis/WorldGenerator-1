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

            var block = new Block(Block.BlockType.Water1);
            worldInstance.SetBlock(pos.X, pos.Y, pos.Z, block);
            worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);

            AddIfEmpty(pos.X, pos.Y - 1, pos.Z);
            AddIfEmpty(pos.X+1, pos.Y, pos.Z);
            AddIfEmpty(pos.X-1, pos.Y, pos.Z);
            AddIfEmpty(pos.X, pos.Y, pos.Z+1);
            AddIfEmpty(pos.X, pos.Y, pos.Z-1);
            AddIfEmpty(pos.X, pos.Y + 1, pos.Z);
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
            Rivers = new List<River>();
            Run();
            //GenerateRivers();

            //ProcessWater(); // TODO - run from own thread
        }

        private IWorld worldInstance;
        private List<River> Rivers;
        private Thread thread;
        private const int WATER = 2; // TODO - stick these somewhere
        private const int GRASS = 4;
        private const int RIVER = 5;

        public void Run()
        {
            thread = new Thread(new ThreadStart(StartThread));
            thread.Start();
        }

        private void CreateRiver()
        {
            for (var i = 0; i < Settings.RiverCount; i++)
            {
                var pos = worldInstance.GetRandomLocationOnLoadedChunk();
                bool isWater = true;
                while (isWater)
                {
                    pos = worldInstance.GetRandomLocationOnLoadedChunk();
                    isWater = (worldInstance.GlobalMapTerrain[pos.X, pos.Z] == WATER);
                }

                pos.Y = 255;
                var b = worldInstance.GetBlock(pos);
                while (b.IsTransparent)
                {
                    pos.Y--;
                    b = worldInstance.GetBlock(pos);
                }
                var river = new River(worldInstance, pos);
                Rivers.Add(river);
            }
        }

        private void StartThread()
        {
            while (worldInstance.LoadedChunkCount == 0)
            {
                System.Threading.Thread.Sleep(5000);
            }
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

