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
		#region Constructors
		public Chunk(ChunkCoords chunkCoords)
		{
            ChunkCoords = chunkCoords;
            Blocks = new Blocks(CHUNK_SIZE, CHUNK_HEIGHT, CHUNK_SIZE);
            //HeightMap = new Array<int>(CHUNK_SIZE, CHUNK_SIZE);
			//Clutters = new HashSet<Clutter>();
			//LightSources = new ConcurrentDictionary<int, LightSource>();
			//Mobs = new HashSet<Mob>();
			//GameItems = new ConcurrentDictionary<int, GameItemDynamic>();
		}
		#endregion

		#region Properties
        public const int SIZE_IN_BYTES = CHUNK_SIZE * CHUNK_HEIGHT * CHUNK_SIZE * sizeof(ushort);
        private const int CLUTTER_RENDER_DISTANCE = CHUNK_SIZE * 4;
		private const int GAME_ITEM_RENDER_DISTANCE = CLUTTER_RENDER_DISTANCE;

		public ChunkCoords ChunkCoords;
		public Blocks Blocks;

        /// <summary>Heighest level in each vertical column containing a non transparent block. Sky light does not shine through this point. Used in rendering and lighting calculations.</summary>
        public Array<int> HeightMap;
		public Array<float> MineralMap;
		public byte[,,] SkyLightMapInitial;
		public byte[,,] ItemLightMapInitial;

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

        /// <summary>Clutter contained in this chunk. Clutter can be stored at the chunk level only because it can never move off the chunk.</summary>
        /// <remarks>HashSet because currently Clutter cannot be added outside of initial world generation. Collection is locked during removal.</remarks>
        //public HashSet<Clutter> Clutters;

        /// <summary>
        /// Light sources contained in this chunk. Light sources can be stored at the chunk level only because they can never move off the chunk.
        /// TBD: when a light source is destroyed, does it become a GameItem?
        /// </summary>
        //public ConcurrentDictionary<int, LightSource> LightSources;

		//public HashSet<Mob> Mobs; //also stored at World level in ConcurrentDictionary
		
		//public ConcurrentDictionary<int, GameItemDynamic> GameItems; //also stored at World level

		/// <summary>Distance of the chunk from the player in number of blocks.</summary>
        public double DistanceFromPlayer(Coords coords)
		{
			return Math.Sqrt(Math.Pow(coords.Xf - ChunkCoords.WorldCoordsX, 2) + Math.Pow(coords.Zf - ChunkCoords.WorldCoordsZ, 2));
		}
		
		/// <summary>Lookup for the Chunk Vbo containing the position, normal and texCoords Vbo's for this chunk and texture type.</summary>
		//private readonly ChunkVbo[] _chunkVbos = new ChunkVbo[Enum.GetNames(typeof(BlockTextureType)).Length];

		/// <summary>Total number of vbo's being rendered for blocks in this chunk.</summary>
		//public int VboCount { get { return _chunkVbos.Count(chunkVbo => chunkVbo != null); } }

		/// <summary>Total number of primitives being rendered for blocks in this chunk.</summary>
		//public int PrimitiveCount { get { return _chunkVbos.Where(chunkVbo => chunkVbo != null).Sum(chunkVbo => chunkVbo.PrimitiveCount); } }

		/// <summary>
		/// The build state of this chunk. When a chunk gets built it is set to 'Built' state and then marked dirty so the vbo will then get created/recreated.
		/// When a change happens in the chunk, its build state is set to 'Queued' for it to get rebuilt. When loading the initial chunk frustum, chunks are
		/// set to QueuedInitialFrustum because they dont need to be pushed to the ChangedChunkQueue. Chunks that should be built in order in the distance are
		/// set to QueuedFar and placed on the FarChunkQueue.
		/// </summary>
		public enum BuildState : byte
		{
			/// <summary>Chunk is not loaded.</summary>
			NotLoaded,
			/// <summary>Chunk is queued for build. It will be on the ChangedChunkQueue.</summary>
			Queued,
			/// <summary>Chunk is queued for build in the distance. It will be on the FarChunkQueue.</summary>
			QueuedFar,
			/// <summary>
			/// Chunk is queued for build as part of day/night lighting cycle. It will be on the FarChunkQueue.
			/// Useful because we can determine the reason the chunk is on the far queue.
			/// -this status could eventually have logic to just do light calcs and rebuffer stored arrays if we decide to store them
			/// </summary>
			QueuedDayNight,
			/// <summary>Chunk is queued for build as part of the initial frustum of chunks loaded before entering the world.</summary>
			QueuedInitialFrustum,
			/// <summary>Chunk is queued for build as part of the initial set of chunks outside the initial radius after entering the world.</summary>
			QueuedInitialFar,
			/// <summary>Chunk is currently building.</summary>
			Building,
			/// <summary>Chunk is built.</summary>
			Built
		}

        /*
		private volatile BuildState _chunkBuildState = BuildState.NotLoaded;
		public BuildState ChunkBuildState
		{
			get { return _chunkBuildState; }
			set
			{
				_chunkBuildState = value;
				switch (value)
				{
					case BuildState.Queued:
						WorldHost.ChangedChunkQueue.Enqueue(this);
						WorldHost.BuildChunkHandle.Set();
						break;
					case BuildState.QueuedDayNight:
					case BuildState.QueuedFar:
					case BuildState.QueuedInitialFar:
						WorldHost.FarChunkQueue.Enqueue(this);
						WorldHost.BuildChunkHandle.Set();
						break;
					case BuildState.Built:
						if (ChunkBufferState == BufferState.VboBuffered) ChunkBufferState = BufferState.VboDirty;
						break;
					case BuildState.NotLoaded:
						ChunkBufferState = BufferState.VboNotBuffered;
						UnloadData();
						break;
				}
			}
		}*/

		/// <summary>
		/// The buffer state of this chunk. Refers to whether a vbo is created 'VboBuffered', needs to be created or recreated 'VboDirty' or has not yet been buffered 'VboNotBuffered'.
		/// The reason the buffer state and build state are different enums is because the chunk needs to wait to be 'Built' before it can be buffered to a vbo.
		/// </summary>
		public enum BufferState { VboNotBuffered, VboDirty, VboBuffered }
		public volatile BufferState ChunkBufferState = BufferState.VboNotBuffered;
		#endregion

		#region Height Map
		/// <summary>Y level of the deepest transparent block in this chunk. When building the vbo, we only need to start at 1 level below this.</summary>
		public int DeepestTransparentLevel { get; set; }

		/// <summary>Y level of the highest non air block. Improves chunk build times. Nothing is rendered higher then this so when building the chunk vbo theres no need to go any higher.</summary>
		public int HighestNonAirLevel { get; set; }

		/// <summary>
		/// Build a heightmap for this chunk. This is the highest non transparent block in each vertical column.
		/// Leaves, water and other transparent blocks that light can shine through do not count.
		/// </summary>
		/// <remarks>The height map is used for lighting. Its also used to determine the players starting Y position.</remarks>
		public void BuildHeightMap()
		{
            HeightMap = new Array<int> (CHUNK_SIZE,CHUNK_SIZE);
            DeepestTransparentLevel = CHUNK_HEIGHT; //initialize to top of chunk until this gets calculated
			HighestNonAirLevel = 0; //initialize to bottom of chunk until this gets calculated
            for (var x = 0; x < CHUNK_SIZE; x++)
			{
                for (var z = 0; z < CHUNK_SIZE; z++)
				{
                    for (var y = CHUNK_HEIGHT - 1; y >= 0; y--) //loop from the highest block position downward until we find a solid block
					{
						var block = Blocks[x, y, z];
						if (y > HighestNonAirLevel && block.Type != Block.BlockType.Air) HighestNonAirLevel = y;
						if (block.IsTransparent) continue;
						HeightMap[x, z] = y;
						break;
					}

                    for (var y = 0; y < CHUNK_HEIGHT - 1; y++) //loop from the base of the world upwards until finding a transparent block
					{
						if (!Blocks[x, y, z].IsTransparent) continue;
						if (y < DeepestTransparentLevel) DeepestTransparentLevel = y; //record this as the deepest transparent level if it is deeper then what we had previously
						break;
					}
				}
			}
		}

		/// <summary>Updates the heightmap following a block placement. Usually a lot quicker then re-building the heightmap.</summary>
		public void UpdateHeightMap(ref Block block, int chunkRelativeX, int yLevel, int chunkRelativeZ)
		{
			var currentHeight = HeightMap[chunkRelativeX, chunkRelativeZ];
			if (block.IsTransparent) //transparent block
			{
				//update height map
				if (yLevel == currentHeight)
				{
					//transparent block being placed at the previous heightmap level, most likely removing a block (which places Air), so we need to find the next non transparent block for the heightmap
					for (var y = currentHeight - 1; y >= 0; y--) //start looking down from the previous heightmap level
					{
						if (y > 0 && Blocks[chunkRelativeX, y, chunkRelativeZ].IsTransparent) continue;
						//found the next non transparent block, update the heightmap and exit
						HeightMap[chunkRelativeX, chunkRelativeZ] = y;
						break;
					}
				}

				//update deepest transparent level
				if (yLevel < DeepestTransparentLevel) DeepestTransparentLevel = yLevel;
			}
			else //non transparent block
			{
				//update height map
				//when placing a non transparent block, check if its above the current heightmap value and if so update the heightmap
				if (yLevel > currentHeight) HeightMap[chunkRelativeX, chunkRelativeZ] = yLevel;

				//update deepest transparent level
				if (yLevel == DeepestTransparentLevel)
				{
					//this block is being set at the DeepestTransparentLevel of this chunk
					//we will need to calc if this is still the deepest level (because theres another transparent block at this depth) or what the new level is
					//the easiest way to do that is just rebuild the height map, even though all we really need to do is the portion that updates the deepest level
					BuildHeightMap();
					return; //no need to continue on to check anything else when doing a full heightmap rebuild
				}
			}

			//update HighestNonAirLevel property
			//1. if placing air (removing block), is it at same level as previous HighestNonAir?, just rebuild HeightMap in this case, otherwise do nothing
			//2. if placing anything other then air, simply check if its > HighestNonAirLevel and set it
			if (block.Type == Block.BlockType.Air) //removing a block
			{
				if (yLevel == HighestNonAirLevel) BuildHeightMap();
			}
			else //adding a block
			{
				if (yLevel > HighestNonAirLevel) HighestNonAirLevel = yLevel;
			}
		}
		#endregion

		/*
		#region Frustum
		// Shortest Face height. Used in frustum checks. Calculated while building vbo's
		// Use for frustum logic instead of deepestTransparentLevel because this will also
		// account for faces drawn below the DeepestTransparentLevel to be visible from adjacent chunks
		//private int _shortestFaceHeight; // TODO - not calculated

		/// <summary>Is this chunk in the players view frustum.</summary>
		/// <seealso cref="http://www.crownandcutlass.com/features/technicaldetails/frustum.html"/>
        public bool IsInFrustum(Vector4d nearFrustum, Vector4d farFrustum, Vector4d leftFrustum, Vector4d rightFrustum, Vector4d topFrustum, Vector4d bottomFrustum)
		{
				float minX = Coords.WorldCoordsX;
				var maxX = minX + CHUNK_SIZE;
				float minZ = Coords.WorldCoordsZ;
				var maxZ = minZ + CHUNK_SIZE;

				var nfXmin = nearFrustum.X * minX;
                var nfXmax = nearFrustum.X * maxX;
                var nfYmin = nearFrustum.Y * _shortestFaceHeight;
                var nfYmax = nearFrustum.Y * HighestNonAirLevel;
                var nfZmin = nearFrustum.Z * minZ;
                var nfZmax = nearFrustum.Z * maxZ;

                if (nfXmin + nfYmax + nfZmin + nearFrustum.W < -0.005f
                    && nfXmin + nfYmax + nfZmax + nearFrustum.W < -0.005f
                    && nfXmax + nfYmax + nfZmin + nearFrustum.W < -0.005f
                    && nfXmax + nfYmax + nfZmax + nearFrustum.W < -0.005f
                    && nfXmin + nfYmin + nfZmin + nearFrustum.W < -0.005f
                    && nfXmin + nfYmin + nfZmax + nearFrustum.W < -0.005f
                    && nfXmax + nfYmin + nfZmin + nearFrustum.W < -0.005f
                    && nfXmax + nfYmin + nfZmax + nearFrustum.W < -0.005f) return false;

                var ffXmin = farFrustum.X * minX;
                var ffXmax = farFrustum.X * maxX;
                var ffYmin = farFrustum.Y * _shortestFaceHeight;
                var ffYmax = farFrustum.Y * HighestNonAirLevel;
                var ffZmin = farFrustum.Z * minZ;
                var ffZmax = farFrustum.Z * maxZ;

                if (ffXmin + ffYmax + ffZmin + farFrustum.W < -0.005f
                    && ffXmin + ffYmax + ffZmax + farFrustum.W < -0.005f
                    && ffXmax + ffYmax + ffZmin + farFrustum.W < -0.005f
                    && ffXmax + ffYmax + ffZmax + farFrustum.W < -0.005f
                    && ffXmin + ffYmin + ffZmin + farFrustum.W < -0.005f
                    && ffXmin + ffYmin + ffZmax + farFrustum.W < -0.005f
                    && ffXmax + ffYmin + ffZmin + farFrustum.W < -0.005f
                    && ffXmax + ffYmin + ffZmax + farFrustum.W < -0.005f) return false;

				var lfXmin = leftFrustum.X * minX;
                var lfXmax = leftFrustum.X * maxX;
                var lfYmin = leftFrustum.Y * _shortestFaceHeight;
                var lfYmax = leftFrustum.Y * HighestNonAirLevel;
                var lfZmin = leftFrustum.Z * minZ;
                var lfZmax = leftFrustum.Z * maxZ;

                if (lfXmin + lfYmax + lfZmin + leftFrustum.W < -0.005f
                    && lfXmin + lfYmax + lfZmax + leftFrustum.W < -0.005f
                    && lfXmax + lfYmax + lfZmin + leftFrustum.W < -0.005f
                    && lfXmax + lfYmax + lfZmax + leftFrustum.W < -0.005f
                    && lfXmin + lfYmin + lfZmin + leftFrustum.W < -0.005f
                    && lfXmin + lfYmin + lfZmax + leftFrustum.W < -0.005f
                    && lfXmax + lfYmin + lfZmin + leftFrustum.W < -0.005f
                    && lfXmax + lfYmin + lfZmax + leftFrustum.W < -0.005f) return false;

				var rfXmin = rightFrustum.X * minX;
				var rfXmax = rightFrustum.X * maxX;
				var rfYmin = rightFrustum.Y * _shortestFaceHeight;
				var rfYmax = rightFrustum.Y * HighestNonAirLevel;
				var rfZmin = rightFrustum.Z * minZ;
				var rfZmax = rightFrustum.Z * maxZ;

				if (rfXmin + rfYmax + rfZmin + rightFrustum.W < -0.005f
					&& rfXmin + rfYmax + rfZmax + rightFrustum.W < -0.005f
					&& rfXmax + rfYmax + rfZmin + rightFrustum.W < -0.005f
					&& rfXmax + rfYmax + rfZmax + rightFrustum.W < -0.005f
					&& rfXmin + rfYmin + rfZmin + rightFrustum.W < -0.005f
					&& rfXmin + rfYmin + rfZmax + rightFrustum.W < -0.005f
					&& rfXmax + rfYmin + rfZmin + rightFrustum.W < -0.005f
					&& rfXmax + rfYmin + rfZmax + rightFrustum.W < -0.005f) return false;

				var tfXmin = topFrustum.X * minX;
				var tfXmax = topFrustum.X * maxX;
				var tfYmin = topFrustum.Y * _shortestFaceHeight;
				var tfYmax = topFrustum.Y * HighestNonAirLevel;
				var tfZmin = topFrustum.Z * minZ;
				var tfZmax = topFrustum.Z * maxZ;

				if (tfXmin + tfYmax + tfZmin + topFrustum.W < -0.005f
					&& tfXmin + tfYmax + tfZmax + topFrustum.W < -0.005f
					&& tfXmax + tfYmax + tfZmin + topFrustum.W < -0.005f
					&& tfXmax + tfYmax + tfZmax + topFrustum.W < -0.005f
					&& tfXmin + tfYmin + tfZmin + topFrustum.W < -0.005f
					&& tfXmin + tfYmin + tfZmax + topFrustum.W < -0.005f
					&& tfXmax + tfYmin + tfZmin + topFrustum.W < -0.005f
					&& tfXmax + tfYmin + tfZmax + topFrustum.W < -0.005f) return false;

				var bfXmin = bottomFrustum.X * minX;
				var bfXmax = bottomFrustum.X * maxX;
				var bfYmin = bottomFrustum.Y * _shortestFaceHeight;
				var bfYmax = bottomFrustum.Y * HighestNonAirLevel;
				var bfZmin = bottomFrustum.Z * minZ;
				var bfZmax = bottomFrustum.Z * maxZ;

				if (bfXmin + bfYmax + bfZmin + bottomFrustum.W < -0.005f
					&& bfXmin + bfYmax + bfZmax + bottomFrustum.W < -0.005f
					&& bfXmax + bfYmax + bfZmin + bottomFrustum.W < -0.005f
					&& bfXmax + bfYmax + bfZmax + bottomFrustum.W < -0.005f
					&& bfXmin + bfYmin + bfZmin + bottomFrustum.W < -0.005f
					&& bfXmin + bfYmin + bfZmax + bottomFrustum.W < -0.005f
					&& bfXmax + bfYmin + bfZmin + bottomFrustum.W < -0.005f
					&& bfXmax + bfYmin + bfZmax + bottomFrustum.W < -0.005f) return false;

				return true;
		}
		#endregion
		*/

        private const int UPDATES_PER_SECOND = 1;
        private const int CHUNK_UPDATE_INTERVAL = 1;
		public bool WaterExpanding { get; set; }
        private const int WATER_UPDATE_INTERVAL = (int)(UPDATES_PER_SECOND * 1.5) / CHUNK_UPDATE_INTERVAL; //1.5s




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

        public static Chunk Deserialize(byte[] data)
        {
            return null; // TODO
            /*
            for (var x = 0; x < Settings.CHUNK_SIZE; x++) {
                for (var z = 0; z < Settings.CHUNK_SIZE; z++) {
                    for (var y = 0; y <= Math.Min (Settings.CHUNK_HEIGHT - 1, HeightMap [x, z] + 1); y++) { //look +1 above heightmap as water directly above heightmap could change to ice
                    }
                }
            }
            */
        }

        public void Render()
        {
            HeightMap.Render();
        }

        #region Xml
        public XmlNode GetXml(XmlDocument xmlDocument)
		{
			var xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "C", string.Empty);
			if (xmlNode.Attributes == null) throw new Exception("Node attributes is null.");
			xmlNode.Attributes.Append(xmlDocument.CreateAttribute("X")).Value = ChunkCoords.X.ToString();
			xmlNode.Attributes.Append(xmlDocument.CreateAttribute("Z")).Value = ChunkCoords.Z.ToString();
			//xmlNode.Attributes.Append(xmlDocument.CreateAttribute("WaterExpanding")).Value = WaterExpanding.ToString();
			//xmlNode.Attributes.Append(xmlDocument.CreateAttribute("GrassGrowing")).Value = GrassGrowing.ToString();
			return xmlNode;
		}
		#endregion
	}
}
