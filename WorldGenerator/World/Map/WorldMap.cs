using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator
{
    internal class WorldMap
    {
        public int MaxXBlock { get; set; }
        public int MinXBlock { get; set; }
        public int MaxZBlock { get; set; }
        public int MinZBlock { get; set; }
        public int MaxXPosition { get { return MaxXBlock * World.ChunkSize; } }
        public int MinXPosition { get { return MinXBlock * World.ChunkSize; } }
        public int MaxZPosition { get { return MaxZBlock * World.ChunkSize; } }
        public int MinZPosition { get { return MinZBlock * World.ChunkSize; } }

        private Dictionary<int, MapChunk> mapChunks;
        private Generator generator;
        private static int MaxBlockLimit = (int)Math.Sqrt(int.MaxValue);

        public WorldMap()
        {
            MaxXBlock = 0;
            MinXBlock = 0;
            MaxZBlock = 0;
            MinZBlock = 0;
            mapChunks = new Dictionary<int, MapChunk> ();
            this.generator = new Generator();
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
            Console.WriteLine ($"Getting {x},{z}");
            int idx = x * MaxBlockLimit + z;
            if (!mapChunks.ContainsKey(idx))
            {
                Console.WriteLine ($"Generating {x},{z}");
                var mapChunk = new MapChunk();
                var chunkCoords = new ChunkCoords(x, z);
                mapChunk.Chunk = new Chunk(chunkCoords);
                generator.Generate(mapChunk.Chunk);
                mapChunks[idx] = mapChunk;
            }
            return mapChunks[isx];
        }

        /*
        public void Render ()
        {
            for (int x = 0; x < MapX; x++) {
                System.Text.StringBuilder builder = new System.Text.StringBuilder ();
                for (int z = 0; z < MapZ; z++) {
                    if (Cursor.WorldX == x && Cursor.WorldZ == z)
                        builder.Append ("*");
                    else
                        builder.Append (chunks [x,z].Render ());
                }
                Console.WriteLine (builder.ToString ());
            }
        }
        */

    }
}

