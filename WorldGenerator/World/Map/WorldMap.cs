using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator
{
    internal class WorldMap
    {
        public int MaxXChunk { get; set; }
        public int MinXChunk { get; set; }
        public int MaxZChunk { get; set; }
        public int MinZChunk { get; set; }
        public int MaxXPosition { get { return MaxXChunk * World.ChunkSize; } }
        public int MinXPosition { get { return MinXChunk * World.ChunkSize; } }
        public int MaxZPosition { get { return MaxZChunk * World.ChunkSize; } }
        public int MinZPosition { get { return MinZChunk * World.ChunkSize; } }

        Array<int> globalMap;
        private Dictionary<int, MapChunk> mapChunks;
        private Generator generator;
        private static int MaxChunkLimit = (int)Math.Sqrt(int.MaxValue);

        public WorldMap(int seed)
        {
            MaxXChunk = int.MinValue;
            MinXChunk = int.MaxValue;
            MaxZChunk = int.MinValue;
            MinZChunk = int.MaxValue;
            mapChunks = new Dictionary<int, MapChunk> ();
            this.generator = new Generator(seed);
            globalMap = generator.GenerateGlobalMap();
        }
         
        /*
        // TODO generate low-res map
        public void Generate()
        {
            var size = new ArraySize(){
                minX=0, maxX=MaxXPosition, minZ=0, maxZ=MaxZPosition, 
                scale=MapScale};
            heightMap = PerlinNoise.GetIntMap(size, 3);
        }

        //private Array<int> heightMap;
        */


        /// <summary>Get a chunk from the array. Based on the x,z of the chunk in the world. Note these are chunk coords not block coords.</summary>
        public Chunk Chunk(int x, int z)
        {
            return GetOrCreate(x, z); 
        }

        public bool IsChunkLoaded(ChunkCoords chunkCoords)
        {
            if (chunkCoords.X > MaxChunkLimit || chunkCoords.X < -MaxChunkLimit || chunkCoords.Z > MaxChunkLimit || chunkCoords.Z < -MaxChunkLimit) throw new ArgumentException("Chunk index exceeded");
            int idx = chunkCoords.X * MaxChunkLimit + chunkCoords.Z;
            return mapChunks.ContainsKey(idx);
        }

        public Chunk Chunk(ChunkCoords chunkCoords)
        {
            return GetOrCreate (chunkCoords.X, chunkCoords.Z);
        }

        /// <summary>Get a chunk from the array. Based on world block coords.</summary>
        public Chunk Chunk(Position position)
        {
            int x = (position.X / World.ChunkSize);
            int z = (position.Z / World.ChunkSize);
            return GetOrCreate(x, z);
        }

        /// <summary>Get a chunk from the array. Based on more accurate world object coords.</summary>
        public Chunk Chunk(Coords coords)
        {
            int x = (coords.Xblock / World.ChunkSize);
            int z = (coords.Zblock / World.ChunkSize);
            return GetOrCreate(x, z);
        }

        private Chunk GetOrCreate(int x, int z)
        {
            Console.WriteLine ($"Getting chunk {x},{z}");
            if (x > MaxChunkLimit || x < -MaxChunkLimit || z > MaxChunkLimit || z < -MaxChunkLimit) throw new ArgumentException ("Chunk index exceeded");
            int idx = x * MaxChunkLimit + z;
            if (!mapChunks.ContainsKey(idx))
            {
                Console.WriteLine ($"Generating {x},{z}");
                var mapChunk = new MapChunk();
                var chunkCoords = new ChunkCoords(x, z);
                mapChunk.Chunk = new Chunk(chunkCoords);
                generator.Generate(globalMap, mapChunk.Chunk);
                mapChunks[idx] = mapChunk;

                if (x > MaxXChunk)
                    MaxXChunk = x;
                if (x < MinXChunk)
                    MinXChunk = x;
                if (z > MaxZChunk)
                    MaxZChunk = z;
                if (z < MinZChunk)
                    MinZChunk = z;
            }
            return mapChunks[idx].Chunk;
        }

        public Array<int> GetGlobalMap()
        {
            return globalMap;
        }

        public void Render ()
        {
            for (int x = MinZChunk; x < MaxXChunk; x++) {
                System.Text.StringBuilder builder = new System.Text.StringBuilder ();
                for (int z = MinZChunk; z < MaxZChunk; z++)
                {
                    int idx = x * MaxChunkLimit + z;
                    if (!mapChunks.ContainsKey(idx)) continue;
                    mapChunks[idx].Chunk.Render();
                }
            }
        }


    }
}

