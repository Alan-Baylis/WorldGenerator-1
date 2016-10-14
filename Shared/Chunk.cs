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
            Blocks = new Blocks();
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
            return Blocks.Serialize();
        }

        public static Chunk Deserialize(ChunkCoords chunkCoords, byte[] data)
        {
            var chunk = new Chunk(chunkCoords);
            chunk.Blocks.Deserialize(data);
            return chunk;
        }

        public IEnumerable<Tuple<Position, Block.BlockType>> GetVisibleIterator()
        {
            foreach (var item in Blocks.GetVisibleIterator())
            {
                yield return new Tuple<Position, Block.BlockType>
                    (new Position(item.Item1.X + ChunkCoords.WorldCoordsX, item.Item1.Y, item.Item1.Z + ChunkCoords.WorldCoordsZ), 
                    item.Item2);
            }
        }
    }
}
