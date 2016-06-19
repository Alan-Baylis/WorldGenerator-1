namespace Sean.WorldGenerator.Render
{
	/// <summary>Used for buffering tex coord data to a VBO.</summary>
	/// <remarks>Using short uses the smallest amount of memory possible for tex coord data.</remarks>
	public struct TexCoordsShort
	{
		public TexCoordsShort(short x, short y) { X = x; Y = y; }
		public short X;
		public short Y;
		public const int SIZE = 4; //2 bytes each
		public short[] Array { get { return new[] { X, Y }; } }
	}
}
