using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sean.Shared
{
	/// <summary>
	/// Specifies a block position in world coordinates using 3 integers.
	/// Used for buffering vertex position data to a VBO.
	/// </summary>
    [DataContract]
    public class Position
	{
        public Position(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>Construct from a byte array containing the Position values.</summary>
		/// <param name="bytes">Byte array containing position values.</param>
		/// <param name="startIndex">Index position to start at in the byte array. Needed because sometimes other data has been sent first in the same byte array.</param>
		public Position(byte[] bytes, int startIndex)
		{
			X = BitConverter.ToInt32(bytes, startIndex);
			Y = BitConverter.ToInt32(bytes, startIndex + sizeof(int));
			Z = BitConverter.ToInt32(bytes, startIndex + sizeof(int) * 2);
		}

        [DataMember]
		public readonly int X;
        [DataMember]
		public readonly int Y;
        [DataMember]
		public readonly int Z;

        public const int SIZE = sizeof(int) * 3;

        public byte[] ToByteArray()
		{
			var bytes = new byte[SIZE];
			BitConverter.GetBytes(X).CopyTo(bytes, 0);
			BitConverter.GetBytes(Y).CopyTo(bytes, sizeof(int));
			BitConverter.GetBytes(Z).CopyTo(bytes, sizeof(int) * 2);
			return bytes;
		}

		[Obsolete("If this is needed on a Position then you should be using Coords instead.")]
        public bool IsValidPlayerLocation
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>Is this position and the compare coords the same block. Fast check as no math is required.</summary>
		/// <remarks>Use this to prevent building on blocks a player is standing on, etc.</remarks>
        public bool IsOnBlock(ref Coords coords)
		{
			return X == coords.Xblock && Y == coords.Yblock && Z == coords.Zblock;
		}

		
        [Obsolete]public int[] Array { get { return new[] {X, Y, Z}; } }

		/// <summary>Get the exact distance from the supplied coords.</summary>
		public float GetDistanceExact(Position position)
		{
			return (float)Math.Sqrt(Math.Pow(X - position.X, 2) + Math.Pow(Y - position.Y, 2) + Math.Pow(Z - position.Z, 2));
		}

        public int GetDistanceRough(Position position)
        {   
            // TODO - improve speed
            return (int)Math.Sqrt(Math.Pow(X - position.X, 2) + Math.Pow(Y - position.Y, 2) + Math.Pow(Z - position.Z, 2));
        }

        public static Position operator+ (Position p1, Position p2)
        {
            return new Position (p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }
        public static Position operator- (Position p1, Position p2)
        {
            return new Position (p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }
        public static Position operator* (Position p1, Position p2)
        {
            return new Position (p1.X * p2.X, p1.Y * p2.Y, p1.Z * p2.Z);
        }
		public Position Abs ()
		{
			return new Position (Math.Abs (X), Math.Abs (Y), Math.Abs (Z));
		}

		public override bool Equals(object obj)
		{
			return X == ((Position)obj).X && Y == ((Position)obj).Y && Z == ((Position)obj).Z;
		}

		public override int GetHashCode()
		{
            return X * Y * Z;
		}

		/// <summary>Returns block position in the format (x={0}, y={1}, z={2})</summary>
		public override string ToString()
		{
			return string.Format("(x={0}, y={1}, z={2})", X, Y, Z);
		}

		/// <summary>Get a Coords struct from this position. Pitch and direction will default to zero.</summary>
        public Coords ToCoords()
		{
			return new Coords(X, Y, Z);
		}
        
	}
}
