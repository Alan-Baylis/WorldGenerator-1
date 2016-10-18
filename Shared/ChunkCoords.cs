using Sean.Shared;
using System.Runtime.Serialization;
using System;

namespace Sean.Shared
{
    [DataContract]
    public struct ChunkCoords : System.IComparable 
	{
        public const int CHUNK_SIZE = 32;
        public static int MaxChunkLimit = (int)Math.Sqrt(int.MaxValue);

		public ChunkCoords(int x, int z)
		{
			X = x;
			Z = z;
            WorldCoordsX = X * CHUNK_SIZE;
            WorldCoordsZ = Z * CHUNK_SIZE;
		}
            
        public ChunkCoords(ref Position position)
        {
            var coords = position.ToCoords ();
            X = coords.Xblock / CHUNK_SIZE;
            Z = coords.Zblock / CHUNK_SIZE;
            WorldCoordsX = X * CHUNK_SIZE;
            WorldCoordsZ = Z * CHUNK_SIZE;
        }
		public ChunkCoords(ref Coords coords)
		{
            X = coords.Xblock / CHUNK_SIZE;
            Z = coords.Zblock / CHUNK_SIZE;
            WorldCoordsX = X * CHUNK_SIZE;
            WorldCoordsZ = Z * CHUNK_SIZE;
		}

        [DataMember]
		public readonly int X;
        [DataMember]
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

        public int CompareTo (object obj)
        {
            return GetHashCode ().CompareTo (obj.GetHashCode ());
        }

		public override int GetHashCode()
		{
            return (X * MaxChunkLimit) + Z;
		}

		public override string ToString()
		{
			return string.Format("({0},{1})", X, Z);
		}
	}
}
