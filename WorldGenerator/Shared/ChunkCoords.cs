using Sean.Shared;

namespace Sean.WorldGenerator
{
	public struct ChunkCoords
	{
		public ChunkCoords(int x, int z)
		{
			X = x;
			Z = z;
            WorldCoordsX = X * Settings.CHUNK_SIZE;
            WorldCoordsZ = Z * Settings.CHUNK_SIZE;
		}
            
		public ChunkCoords(ref Coords coords)
		{
            X = coords.Xblock / Settings.CHUNK_SIZE;
            Z = coords.Zblock / Settings.CHUNK_SIZE;
            WorldCoordsX = X * Settings.CHUNK_SIZE;
            WorldCoordsZ = Z * Settings.CHUNK_SIZE;
		}

		public readonly int X;
		public readonly int Z;

		/// <summary>X coord of this chunk in world block coords.</summary>
		public int WorldCoordsX;
		/// <summary>Z coord of this chunk in world block coords.</summary>
		public int WorldCoordsZ;

		public static bool operator ==(ChunkCoords c1, ChunkCoords c2)
		{
			return c1.X == c2.X && c1.Z == c2.Z;
		}

		public static bool operator !=(ChunkCoords c1, ChunkCoords c2)
		{
			return c1.X != c2.X || c1.Z != c2.Z;
		}

		public override bool Equals(object obj)
		{
			return X == ((ChunkCoords)obj).X && Z == ((ChunkCoords)obj).Z;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("({0},{1})", X, Z);
		}
	}
}
