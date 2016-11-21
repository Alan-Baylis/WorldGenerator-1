using Sean.Shared;
using System;
using System.Collections.Generic;

namespace Sean.WorldGenerator
{
    public class Water
    {
        public Water (IWorld world)
        {
            worldInstance = world;
            GenerateRivers();

            //ProcessWater(); // TODO - run from own thread
        }

        private IWorld worldInstance;
        private List<Position> WaterSources;
        private const int WATER = 2; // TODO - stick these somewhere
        private const int GRASS = 4;
        private const int RIVER = 5;
        private void GenerateRivers()
        {
            WaterSources = new List<Position>();
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
                var chunk = worldInstance.GetChunk(new ChunkCoords(pos));

                var block = new Block(Block.BlockType.WaterSource);
                worldInstance.SetBlock(pos.X, pos.Y, pos.Z, block);
                worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);

                WaterSources.Add(pos);
            }
        }

        private void ProcessWater()
        {
            var expandWater = new UniqueQueue<Position>();
            foreach (var pos in WaterSources)
            {

                expandWater.Enqueue(pos);
            }

            while (expandWater.Count > 0)
            {
                var pos = expandWater.Dequeue();
                var x = pos.X;
                var y = pos.Y;
                var z = pos.Z;
                if (worldInstance.GetBlock(x, y - 1, z).Type == Block.BlockType.Air)
                {
                    worldInstance.SetBlock(x, y - 1, z, new Block(Block.BlockType.Ocean));
                    worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);
                    expandWater.Enqueue(new Position(x, y - 1, z));
                    continue;
                }

                if (worldInstance.GetBlock(x - 1, y, z).Type == Block.BlockType.Air)
                {
                    worldInstance.SetBlock(x - 1, y, z, new Block(Block.BlockType.Ocean));
                    worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);
                    expandWater.Enqueue(new Position(x - 1, y, z));
                }
                if (worldInstance.GetBlock(x + 1, y, z).Type == Block.BlockType.Air)
                {
                    worldInstance.SetBlock(x + 1, y, z, new Block(Block.BlockType.Ocean));
                    worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);
                    expandWater.Enqueue(new Position(x + 1, y, z));
                }
                if (worldInstance.GetBlock(x, y, z - 1).Type == Block.BlockType.Air)
                {
                    worldInstance.SetBlock(x, y, z - 1, new Block(Block.BlockType.Ocean));
                    worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);
                    expandWater.Enqueue(new Position(x, y, z - 1));
                }
                if (worldInstance.GetBlock(x, y, z + 1).Type == Block.BlockType.Air)
                {
                    worldInstance.SetBlock(x, y, z + 1, new Block(Block.BlockType.Ocean));
                    worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);
                    expandWater.Enqueue(new Position(x, y, z + 1));
                }
            }

        }


    }
}

