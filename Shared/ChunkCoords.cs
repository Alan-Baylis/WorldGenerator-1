using Sean.Shared;
using System.Runtime.Serialization;
using System;

namespace Sean.Shared
{
    [DataContract]
    public struct ChunkCoords : System.IComparable 
	{
        public static int MaxChunkLimit = (int)Math.Sqrt(int.MaxValue);

		public ChunkCoords(int x, int z)
		{
			X = x;
			Z = z;
		}
            
        public ChunkCoords(Position position)
        {
            var coords = position.ToCoords ();
            X = coords.Xblock / Global.CHUNK_SIZE;
            Z = coords.Zblock / Global.CHUNK_SIZE;
        }
		public ChunkCoords(ref Coords coords)
		{
            X = coords.Xblock / Global.CHUNK_SIZE;
            Z = coords.Zblock / Global.CHUNK_SIZE;
		}

        [DataMember]
		public readonly int X;
        [DataMember]
		public readonly int Z;

		/// <summary>X coord of this chunk in world block coords.</summary>
        public int WorldCoordsX { get { return X * Global.CHUNK_SIZE; } }
		/// <summary>Z coord of this chunk in world block coords.</summary>
        public int WorldCoordsZ { get { return Z * Global.CHUNK_SIZE; } }

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
