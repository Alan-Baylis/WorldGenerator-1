using System;
using Sean.Shared;

namespace Sean.WorldGenerator
{
	/// <summary>
	/// Note that the data is stored as [Y,X,Z]. When we Buffer.BlockCopy, we want the data accessed Y-by-Y. This improves compression and results in a ~10% smaller world
	/// </summary>
	public struct Blocks
	{
		public Blocks(int chunkSizeX, int chunkHeight, int chunkSizeZ)
		{
			Array = new ushort[chunkHeight, chunkSizeX, chunkSizeZ];
		}

		public Block this[Coords coords]
		{
            get { return new Block(Array[coords.Yblock, coords.Xblock % Settings.CHUNK_SIZE, coords.Zblock % Settings.CHUNK_SIZE]); }
            set { Array[coords.Yblock, coords.Xblock % Settings.CHUNK_SIZE, coords.Zblock % Settings.CHUNK_SIZE] = value.BlockData; }
		}

		public Block this[Position position]
		{
            get { return new Block(Array[position.Y, position.X % Settings.CHUNK_SIZE, position.Z % Settings.CHUNK_SIZE]); }
            set { Array[position.Y, position.X % Settings.CHUNK_SIZE, position.Z % Settings.CHUNK_SIZE] = value.BlockData; }
		}

		/// <summary>Get a block from the array.</summary>
		/// <param name="x">Chunk relative X.</param>
		/// <param name="y">Chunk relative Y.</param>
		/// <param name="z">Chunk relative Z.</param>
		public Block this[int x, int y, int z]
		{
			get { return new Block(Array[y, x, z]); }
			set { Array[y, x, z] = value.BlockData; }
		}

		public readonly ushort[, ,] Array;

		[Obsolete("This was for world diffs. Not being used currently.")]
		public ushort[,,] DiffArray
		{
			get
			{
                var diffArray = new ushort[Settings.CHUNK_HEIGHT,Settings.CHUNK_SIZE,Settings.CHUNK_SIZE];
                for (var y = 0; y < Settings.CHUNK_HEIGHT; y++)
				{
                    for (var x = 0; x < Settings.CHUNK_SIZE; x++)
					{
                        for (var z = 0; z < Settings.CHUNK_SIZE; z++)
						{
							//only copy the dirty blocks
							if ((Array[y, x, z] & 0x8000) != 0) diffArray[y, x, z] = Array[y, x, z];
						}
					}
				}
				return diffArray;
			}
		}
	}
}
