using System;
using Sean.Shared;

namespace OpenTkClient
{
	public static class Global
	{
		public const int CHUNK_SIZE = 16; // TODO - move to common location
		public const int CHUNK_HEIGHT = 256; // TODO - move to common location
		public static float Scale = 2.0f;
		public static Position LookingAt = new Position((56*32)+14, 157, (27*32)+15);
		public static Facing Direction;
		public static int MaxChunkLimit = (int)Math.Sqrt(int.MaxValue);
        public static string ServerName = "localhost";
        public static bool IsLocalServer;
        public static bool ClearExistingChunks;
    }
}

