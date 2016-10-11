using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace Sean.Shared
{
	public class Chunk
	{
        public const int CHUNK_SIZE = 32; // TODO - move
        public const int CHUNK_HEIGHT = 128; // TODO - move

		public Chunk(ChunkCoords chunkCoords)
		{
            ChunkCoords = chunkCoords;
            Blocks = new Blocks(CHUNK_SIZE, CHUNK_HEIGHT, CHUNK_SIZE);
		}

		public ChunkCoords ChunkCoords;
		public Blocks Blocks;

        public int ChunkSize {  get { return CHUNK_SIZE; } }
        public Position MinPosition { get { 
                return 
                new Position (
                    ChunkCoords.WorldCoordsX,
                    0,
                    ChunkCoords.WorldCoordsZ);
            }
        }
        public Position MaxPosition { get { 
                return 
                    new Position (
                        ChunkCoords.WorldCoordsX + CHUNK_SIZE,
                        CHUNK_HEIGHT,
                        ChunkCoords.WorldCoordsZ + CHUNK_SIZE);
            }
        }

        public byte[] Serialize()
        {
            using (var memoryStream = new System.IO.MemoryStream ()) {
                for (var x = 0; x < CHUNK_SIZE; x++) {
                    for (var z = 0; z < CHUNK_SIZE; z++) {
                        for (var y = 0; y < CHUNK_HEIGHT; y++) {
                            memoryStream.WriteByte ((byte)Blocks[x, y, z].Type);
                        }
                    }
                }
                return memoryStream.ToArray ();
            }
        }

        public static Chunk Deserialize(ChunkCoords chunkCoords, byte[] data)
        {
            var chunk = new Chunk(chunkCoords);
            using (var memoryStream = new System.IO.MemoryStream(data)) {
                for (var x = 0; x < CHUNK_SIZE; x++) {
                    for (var z = 0; z < CHUNK_SIZE; z++) {
                        for (var y = 0; y < CHUNK_HEIGHT; y++) {
                            ushort blockType = (ushort)memoryStream.ReadByte();
                            chunk.Blocks[x, y, z] = new Block(blockType);
                        }
                    }
                }
            }
            return chunk;
        }
	}
}
