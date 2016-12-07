using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator
{
    internal class LocalMap
    {
        public int MaxXChunk { get; set; }
        public int MinXChunk { get; set; }
        public int MaxZChunk { get; set; }
        public int MinZChunk { get; set; }
        public int MaxXPosition { get { return MaxXChunk * Global.CHUNK_SIZE; } }
        public int MinXPosition { get { return MinXChunk * Global.CHUNK_SIZE; } }
        public int MaxZPosition { get { return MaxZChunk * Global.CHUNK_SIZE; } }
        public int MinZPosition { get { return MinZChunk * Global.CHUNK_SIZE; } }

        private Dictionary<int, MapChunk> mapChunks;
        private Generator generator;
        private static int MaxChunkLimit = (int)Math.Sqrt(int.MaxValue);

        public LocalMap(IWorld world, int seed)
        {
            MaxXChunk = int.MinValue;
            MinXChunk = int.MaxValue;
            MaxZChunk = int.MinValue;
            MinZChunk = int.MaxValue;
            mapChunks = new Dictionary<int, MapChunk> ();
            this.generator = new Generator(world, seed);
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
        /*
        internal bool IsChunkLoaded(int x, int z)
        {
            lock (mapChunks)
            {
                if (x > MaxChunkLimit || x < -MaxChunkLimit || z > MaxChunkLimit || z < -MaxChunkLimit) throw new ArgumentException("Chunk index exceeded");
                int idx = x * MaxChunkLimit + z;
                return mapChunks.ContainsKey(idx);
            }
        }
        */
        public bool IsChunkLoaded(ChunkCoords chunkCoords)
        {
            lock (mapChunks)
            {
                if (chunkCoords.X > MaxChunkLimit || chunkCoords.X < -MaxChunkLimit || chunkCoords.Z > MaxChunkLimit || chunkCoords.Z < -MaxChunkLimit) throw new ArgumentException("Chunk index exceeded");
                int idx = chunkCoords.X * MaxChunkLimit + chunkCoords.Z;
                return mapChunks.ContainsKey(idx);
            }
        }

        public Chunk Chunk(ChunkCoords chunkCoords)
        {
            return GetOrCreate (chunkCoords.X, chunkCoords.Z);
        }
        public List<ChunkCoords> LoadedChunks()
        {
            lock (mapChunks)
            {
                var chunks = new List<ChunkCoords>();
                foreach(var chunk in mapChunks)
                {
                    chunks.Add(chunk.Value.Chunk.ChunkCoords);
                }
                return chunks;
            }
        }

        public int LoadedChunksCount()
        {
            lock (mapChunks)
            {
                return mapChunks.Count;
            }
        }

        /// <summary>Get a chunk from the array. Based on world block coords.</summary>
        public Chunk Chunk(Position position)
        {
            int x = (position.X / Global.CHUNK_SIZE);
            int z = (position.Z / Global.CHUNK_SIZE);
            return GetOrCreate(x, z);
        }

        /// <summary>Get a chunk from the array. Based on more accurate world object coords.</summary>
        public Chunk Chunk(Coords coords)
        {
            int x = (coords.Xblock / Global.CHUNK_SIZE);
            int z = (coords.Zblock / Global.CHUNK_SIZE);
            return GetOrCreate(x, z);
        }

        private Chunk GetOrCreate(int x, int z)
        {
            //Log.WriteInfo ($"Getting chunk {x},{z}");
            Chunk chunk = null;
            lock (mapChunks) {
                if (x > MaxChunkLimit || x < -MaxChunkLimit || z > MaxChunkLimit || z < -MaxChunkLimit)
                    throw new ArgumentException ("Chunk index exceeded");
                int idx = x * MaxChunkLimit + z;
                if (mapChunks.ContainsKey (idx)) {
                    return mapChunks [idx].Chunk;
                }

                // Create Chunk
                Log.WriteInfo($"Generating {x},{z}");
                var mapChunk = new MapChunk ();
                var chunkCoords = new ChunkCoords (x, z);
                chunk = new Chunk (chunkCoords);
                mapChunk.Chunk = chunk;
                mapChunks [idx] = mapChunk;

                if (x > MaxXChunk)
                    MaxXChunk = x;
                if (x < MinXChunk)
                    MinXChunk = x;
                if (z > MaxZChunk)
                    MaxZChunk = z;
                if (z < MinZChunk)
                    MinZChunk = z;
            }
            generator.Generate(chunk);
            return chunk;
        }

    }
}

