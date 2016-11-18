using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sean.Shared;

namespace Sean.WorldGenerator
{
    internal class WorldMap
    {
        private IWorld worldInstance;
        private Generator generator;
        private const int oceanLevel = 40;

        public Array<int> IslandMap { get; private set; }
        public Array<byte> GlobalMap { get; private set; }
        public Array<byte> GlobalMapTerrain { get; private set; }
        public Array<byte> TemperatureMap { get; private set; }
        public Array<byte> BiosphereMap { get; private set; }

        public WorldMap(IWorld world, int seed)
        {
            this.generator = new Generator(world, seed);
            Generate();
        }

        public void Generate()
        {
            GlobalMap = generator.GenerateGlobalMap();
            GlobalMapTerrain = DefineTerrain(GlobalMap);
            TemperatureMap = DefineTemperature(GlobalMap);
            BiosphereMap = DefineBiosphere(GlobalMap, TemperatureMap);
        }


        private List<Position> WaterSources;
        private const int WATER = 2;
        private const int GRASS = 4;
        private void GenerateRivers()
        {
            WaterSources = new List<Position> ();
            for (var i = 0; i < Settings.RiverCount; i++)
            {
                int x=0, y=255, z=0; // TODO set y
                bool isWater = true;
                while (isWater)
                {
                    x = Settings.Random.Next(GlobalMap.Size.minX, GlobalMap.Size.maxX);
                    z = Settings.Random.Next(GlobalMap.Size.minZ, GlobalMap.Size.maxZ);
                    isWater = (GlobalMapTerrain[x, z] == WATER);
                }
                var pos = new Position (x, y, z);
                var chunk = worldInstance.GetChunk (new ChunkCoords (pos));
                WaterSources.Add(pos);
                var block = new Block (Block.BlockType.Water);
                worldInstance.SetBlock (x, y, z, block);
            }

            var expandWater = new UniqueQueue<Position> ();
            foreach (var pos in WaterSources) {                
                expandWater.Enqueue (pos);
            }

            while (expandWater.Count > 0) {
                var pos = expandWater.Dequeue();
                var x = pos.X;
                var y = pos.Y;
                var z = pos.Z;
                if (worldInstance.GetBlock (x, y - 1, z).Type == Block.BlockType.Air) {
                    worldInstance.SetBlock (x, y - 1, z, new Block (Block.BlockType.Water));
                    expandWater.Enqueue (new Position (x, y - 1, z));
                    continue; 
                }

                if (worldInstance.GetBlock (x-1, y, z).Type == Block.BlockType.Air) {
                    worldInstance.SetBlock (x-1, y, z, new Block (Block.BlockType.Water));
                    expandWater.Enqueue (new Position (x-1, y, z));
                }
                if (worldInstance.GetBlock (x+1, y, z).Type == Block.BlockType.Air) {
                    worldInstance.SetBlock (x+1, y, z, new Block (Block.BlockType.Water));
                    expandWater.Enqueue (new Position (x+1, y, z));
                }
                if (worldInstance.GetBlock (x, y, z-1).Type == Block.BlockType.Air) {
                    worldInstance.SetBlock (x, y, z-1, new Block (Block.BlockType.Water));
                    expandWater.Enqueue (new Position (x, y, z-1));
                }
                if (worldInstance.GetBlock (x, y, z+1).Type == Block.BlockType.Air) {
                    worldInstance.SetBlock (x, y, z+1, new Block (Block.BlockType.Water));
                    expandWater.Enqueue (new Position (x, y, z+1));
                }
            }

        }


        public Array<int> GenerateIslandMap(uint octaves, double freq, double x, double z, double scale)
        {
            IslandMap = generator.GenerateIslandMap(octaves, freq, x, z, scale);
            return IslandMap;
        }
            
        private Array<byte> DefineTerrain(Array<byte> globalMap)
        {
            var terrain = new Array<byte>(globalMap.Size);
            for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
            {
                for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
                {
                    terrain[x, z] = GRASS;
                }
            }
            UniqueQueue<ChunkCoords> checkChunks = new UniqueQueue<ChunkCoords>();
            for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
            {
                if (globalMap[globalMap.Size.minX, z] < Settings.waterLevel)
                    checkChunks.Enqueue(new ChunkCoords(globalMap.Size.minX, z));
                if (globalMap[globalMap.Size.maxX-1, z] < Settings.waterLevel)
                    checkChunks.Enqueue(new ChunkCoords(globalMap.Size.maxX-1, z));
            }
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                if (globalMap[x, globalMap.Size.minZ] < Settings.waterLevel)
                    checkChunks.Enqueue(new ChunkCoords(x, globalMap.Size.minZ));
                if (globalMap[x, globalMap.Size.maxZ-1] < Settings.waterLevel)
                    checkChunks.Enqueue(new ChunkCoords(x, globalMap.Size.maxZ-1));
            }
            while (checkChunks.Count > 0)
            {
                var pos = checkChunks.Dequeue();
                if (globalMap.IsValidCoord(pos.X, pos.Z) && terrain[pos.X, pos.Z] != WATER && globalMap[pos.X, pos.Z] < Settings.waterLevel)
                {
                    terrain[pos.X, pos.Z] = WATER;
                    checkChunks.Enqueue(new ChunkCoords(pos.X + globalMap.Size.scale, pos.Z));
                    checkChunks.Enqueue(new ChunkCoords(pos.X - globalMap.Size.scale, pos.Z));
                    checkChunks.Enqueue(new ChunkCoords(pos.X, pos.Z + globalMap.Size.scale));
                    checkChunks.Enqueue(new ChunkCoords(pos.X, pos.Z - globalMap.Size.scale));
                }
            }

            return terrain;
        }
        private Array<byte> DefineTemperature(Array<byte> globalMap)
        {
            var temp = new Array<byte>(globalMap.Size);
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
                {
                    var h = (globalMap.Size.maxY - globalMap[x, z]) * 50 / globalMap.Size.maxY;
                    var l = z * 50 / globalMap.Size.maxZ;
                    temp[x, z] = (byte)((h + l) / 2);
                }
            }
            return temp;
        }
        private Array<byte> DefineBiosphere(Array<byte> globalMap, Array<byte> temperatureMap)
        {
            var bio = new Array<byte>(globalMap.Size);
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
                {
                    bio[x,z] = (byte)(generator.CalcGlobalBiosphere(x, z) * 255);
                }
            }
            return bio;
        }
    }
}
