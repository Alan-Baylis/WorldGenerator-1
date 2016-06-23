using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Sean.Shared;

namespace Sean.WorldGenerator
{
	/// <summary>
	/// World environment type. Integer value is saved in world settings XML, so these integer values cannot be changed without breaking existing worlds.
	/// Start at 1 so we can ensure this gets loaded properly and not defaulted to zero.
	/// </summary>
	public enum WorldType : byte
	{
		Grass = 1,
		Winter = 2,
		Desert = 3
	}

	internal class WorldData
	{
        public WorldData()
		{
			//Mobs = new ConcurrentDictionary<int, Mob>();
			GameItems = new ConcurrentDictionary<int, GameItemDynamic>();

            RawSeed = "123456"; //settingsNode.Attributes["RawSeed"].Value;
            GeneratorVersion = "1.0";// settingsNode.Attributes["GeneratorVersion"].Value;
            GameObjectIdSeq = 1; //int.Parse(settingsNode.Attributes["GameObjectIdSeq"].Value);
            WorldType = WorldType.Grass;// (WorldType)Convert.ToInt32(settingsNode.Attributes["WorldType"].Value);

            ChunkSize = 32;
            InitialSize = 80;
		}

		#region Properties (Saved)
        public WorldType WorldType { get; set; }
		/// <summary>Original Raw Seed used to generate this world. Blank if no seed was used.</summary>
        public string RawSeed { get; set; }
		/// <summary>Original program version used when this world was generated.</summary>
        public string GeneratorVersion { get; set; }
   
        public int ChunkSize { get; set; } 
        public int InitialSize { get; set; }

        public int GameObjectIdSeq;
        public int NextGameObjectId
		{
			get { return System.Threading.Interlocked.Increment(ref GameObjectIdSeq); }
		}


        private int _sizeInChunksX;
		/// <summary>Number of chunks in X direction that make up the world.</summary>
        public int SizeInChunksX
		{
			get { return _sizeInChunksX; }
			set
			{
				_sizeInChunksX = value;
				SizeInBlocksX = _sizeInChunksX * Chunk.CHUNK_SIZE;
			}
		}

        private int _sizeInChunksZ;
		/// <summary>Number of chunks in Z direction that make up the world.</summary>
        public int SizeInChunksZ
		{
			get { return _sizeInChunksZ; }
			set
			{
				_sizeInChunksZ = value;
				SizeInBlocksZ = _sizeInChunksZ * Chunk.CHUNK_SIZE;
			}
		}

		/// <summary>Number of blocks in X direction that make up the world.</summary>
        public int SizeInBlocksX { get; private set; }

		/// <summary>Number of blocks in Z direction that make up the world.</summary>
        public int SizeInBlocksZ { get; private set; }


		#endregion

		#region Properties (Dynamic)
		/// <summary>True when the world has been completely loaded from disk for server and single player or when world has been completely received in multiplayer.</summary>
        public bool IsLoaded { get; set; }
        //public static Chunks Chunks;
		public bool GenerateWithTrees = true;
		#endregion

    }
}
