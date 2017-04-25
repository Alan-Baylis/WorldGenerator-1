using System;
using System.Collections.Generic;

namespace Sean.Shared
{
    public class Chunk
	{
        #region Constructors
        public Chunk(ChunkCoords chunkCoords)
        {
            ChunkCoords = chunkCoords;
            Blocks = new Blocks();
            //HeightMap = new Array<int>(CHUNK_SIZE, CHUNK_SIZE);
            //Clutters = new HashSet<Clutter>();
            //LightSources = new ConcurrentDictionary<int, LightSource>();
            //Mobs = new HashSet<Mob>();
            //GameItems = new ConcurrentDictionary<int, GameItemDynamic>();
        }
        #endregion


        #region Properties
        public const int SIZE_IN_BYTES = Global.CHUNK_SIZE * Global.CHUNK_HEIGHT * Global.CHUNK_SIZE * sizeof(ushort);
        private const int CLUTTER_RENDER_DISTANCE = Global.CHUNK_SIZE * 4;
        private const int GAME_ITEM_RENDER_DISTANCE = CLUTTER_RENDER_DISTANCE;

        public ChunkCoords ChunkCoords;
        public Blocks Blocks;

        /// <summary>Heighest level in each vertical column containing a non transparent block. Sky light does not shine through this point. Used in rendering and lighting calculations.</summary>
        public Array<int> HeightMap;
        public Array<float> MineralMap;
        public byte[,,] SkyLightMapInitial;
        public byte[,,] ItemLightMapInitial;

        public bool FinishedGeneration { get; set; }
        public int ChunkSize {  get { return Global.CHUNK_SIZE; } }
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
                        ChunkCoords.WorldCoordsX + Global.CHUNK_SIZE-1,
                        Global.CHUNK_HEIGHT,
                        ChunkCoords.WorldCoordsZ + Global.CHUNK_SIZE-1);
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
        #endregion

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

        public IEnumerable<Tuple<Position, BlockType>> GetVisibleIterator(Facing direction)
        {
            foreach (var item in Blocks.GetVisibleIterator(direction))
            {
                yield return new Tuple<Position, BlockType>
                    (new Position(item.Item1.X + ChunkCoords.WorldCoordsX, item.Item1.Y, item.Item1.Z + ChunkCoords.WorldCoordsZ), 
                    item.Item2);
            }
        }

        #region Height Map
        /// <summary>Y level of the deepest transparent block in this chunk. When building the vbo, we only need to start at 1 level below this.</summary>
        public int DeepestTransparentLevel { get; set; }

        /// <summary>Y level of the highest non air block. Improves chunk build times. Nothing is rendered higher then this so when building the chunk vbo theres no need to go any higher.</summary>
        public int HighestNonAirLevel { get; set; }

        public int GetHeight (int x, int z)
        {
            return HeightMap [x % Global.CHUNK_SIZE, z % Global.CHUNK_SIZE];
        }

        /// <summary>
        /// Build a heightmap for this chunk. This is the highest non transparent block in each vertical column.
        /// Leaves, water and other transparent blocks that light can shine through do not count.
        /// </summary>
        /// <remarks>The height map is used for lighting. Its also used to determine the players starting Y position.</remarks>
        public void BuildHeightMap()
        {
            HeightMap = new Array<int> (Global.CHUNK_SIZE,Global.CHUNK_SIZE);
            DeepestTransparentLevel = Global.CHUNK_HEIGHT; //initialize to top of chunk until this gets calculated
            HighestNonAirLevel = 0; //initialize to bottom of chunk until this gets calculated
            for (var x = 0; x < Global.CHUNK_SIZE; x++)
            {
                for (var z = 0; z < Global.CHUNK_SIZE; z++)
                {
                    for (var y = Global.CHUNK_HEIGHT - 1; y >= 0; y--) //loop from the highest block position downward until we find a solid block
                    {
                        var block = Blocks[x, y, z];
                        if (y > HighestNonAirLevel && block.Type != BlockType.Air) HighestNonAirLevel = y;
                        if (block.IsTransparent) continue;
                        HeightMap[x, z] = y;
                        if (block.Type == BlockType.Dirt)
                            Blocks[x, y, z] = new Block(BlockType.Grass);
                        break;
                    }

                    for (var y = 0; y < Global.CHUNK_HEIGHT - 1; y++) //loop from the base of the world upwards until finding a transparent block
                    {
                        if (!Blocks[x, y, z].IsTransparent) continue;
                        if (y < DeepestTransparentLevel) DeepestTransparentLevel = y; //record this as the deepest transparent level if it is deeper then what we had previously
                        break;
                    }
                }
            }

            return; // TODO - reenable slopes

            // Create slopes
            for (var x = 1; x < Global.CHUNK_SIZE-1; x++) {
                for (var z = 1; z < Global.CHUNK_SIZE-1; z++) {
                    var y = HeightMap [x, z];
                    var ne = HeightMap [x, z-1];
                    var nw = HeightMap [x - 1, z];
                    var se = HeightMap [x+1, z ];
                    var sw = HeightMap [x, z +1];

                    // TODO - define random slope sprites
                    if (y == ne + 1 && y == nw + 1 && y == se + 1 && y == sw + 1)
                        continue;
                    if (y <= ne && y <= nw && y <= se && y == sw + 1)
                        Blocks [x, y, z] = new Block (BlockType.GrassSlopeNW);
                    if (y <= ne && y <= nw && y == se + 1 && y <= sw)
                        Blocks [x, y, z] = new Block (BlockType.GrassSlopeNE);
                    if (y == ne + 1 && y <= nw && y <= se && y <= sw )
                        Blocks [x, y, z] = new Block (BlockType.GrassSlopeSE);
                    if (y <= ne && y == nw + 1 && y <= se && y <= sw )
                        Blocks [x, y, z] = new Block (BlockType.GrassSlopeSW);
                }
            }

            for (var x = 1; x < Global.CHUNK_SIZE-1; x++) {
                for (var z = 1; z < Global.CHUNK_SIZE-1; z++) {
                    var y = HeightMap [x, z];
                    var n = HeightMap [x - 1, z - 1];
                    var s = HeightMap [x + 1, z + 1];
                    var e = HeightMap [x + 1, z - 1];
                    var w = HeightMap [x - 1, z + 1];
                    var ne = HeightMap [x, z-1];
                    var nw = HeightMap [x - 1, z];
                    var se = HeightMap [x+1, z ];
                    var sw = HeightMap [x, z +1];

                    if (Blocks [x, y, z].Type == BlockType.Grass) {
                        if (y <= nw && y <= ne && y <= se && y <= sw) {
                            if (y <= n && y <= e && y > s && y <= w)
                                Blocks [x, y, z] = new Block (BlockType.GrassSlopeNEW);
                            if (y <= n && y <= e && y <= s && y > w)
                                Blocks [x, y, z] = new Block (BlockType.GrassSlopeNES);
                            if (y > n && y <= e && y <= s && y <= w)
                                Blocks [x, y, z] = new Block (BlockType.GrassSlopeESW);
                            if (y <= n && y > e && y <= s && y <= w)
                                Blocks [x, y, z] = new Block (BlockType.GrassSlopeNWS);
                        }

                        /*
                        if (y == nw && y == ne && y == se + 1 && y == sw + 1) 
                            Blocks [x, y, z] = new Block (Block.BlockType.GrassSlopeN);
                        if (y == nw +1 && y == ne +1 && y == se && y == sw) 
                            Blocks [x, y, z] = new Block (Block.BlockType.GrassSlopeN);
                        if (y == nw && y == ne && y == se + 1 && y == sw + 1) 
                            Blocks [x, y, z] = new Block (Block.BlockType.GrassSlopeN);
                        if (y == nw && y == ne && y == se + 1 && y == sw + 1) 
                            Blocks [x, y, z] = new Block (Block.BlockType.GrassSlopeN);
                        */
                        /*
                            if (Blocks [x, ne, z - 1].Type == Block.BlockType.GrassSlopeNW
                            && Blocks [x - 1, nw, z].Type == Block.BlockType.Grass)
                                Blocks [x, y, z] = new Block (Block.BlockType.GrassSlopeN);
                            if (Blocks [x, ne, z - 1].Type == Block.BlockType.GrassSlopeNW
                            && Blocks [x - 1, nw, z].Type == Block.BlockType.GrassSlopeNE)
                                Blocks [x, y, z] = new Block (Block.BlockType.GrassSlopeNEW);
                        */
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
            if (block.Type == BlockType.Air) //removing a block
            {
                if (yLevel == HighestNonAirLevel) BuildHeightMap();
            }
            else //adding a block
            {
                if (yLevel > HighestNonAirLevel) HighestNonAirLevel = yLevel;
            }
        }
        #endregion

        private const int UPDATES_PER_SECOND = 1;
        private const int CHUNK_UPDATE_INTERVAL = 1;
        public bool WaterExpanding { get; set; }
        private const int WATER_UPDATE_INTERVAL = (int)(UPDATES_PER_SECOND * 1.5) / CHUNK_UPDATE_INTERVAL; //1.5s
        /// <summary>Only called for SinglePlayer and Servers.</summary>
/*      private void WaterExpand()
        {
            Debug.WriteLine("Water expanding in chunk {0}...", ChunkCoords);
            var newWater = new List<Position>();
            for (var i = 0; i < Global.CHUNK_SIZE; i++)
            {
                for (var j = 0; j < Global.CHUNK_HEIGHT; j++)
                {
                    for (var k = 0; k < Global.CHUNK_SIZE; k++)
                    {
                        if (Blocks[i, j, k].Type != Block.BlockType.Water) continue;
                        var belowCurrent = new Position();
                        for (var q = 0; q < 5; q++)
                        {
                            Position adjacent;
                            switch (q)
                            {
                            case 0:
                                adjacent = new Position(ChunkCoords.WorldCoordsX + i, j - 1, ChunkCoords.WorldCoordsZ + k);
                                belowCurrent = adjacent;
                                break;
                            case 1:
                                adjacent = new Position(ChunkCoords.WorldCoordsX + i + 1, j, ChunkCoords.WorldCoordsZ + k);
                                break;
                            case 2:
                                adjacent = new Position(ChunkCoords.WorldCoordsX + i - 1, j, ChunkCoords.WorldCoordsZ + k);
                                break;
                            case 3:
                                adjacent = new Position(ChunkCoords.WorldCoordsX + i, j, ChunkCoords.WorldCoordsZ + k + 1);
                                break;
                            default:
                                adjacent = new Position(ChunkCoords.WorldCoordsX + i, j, ChunkCoords.WorldCoordsZ + k - 1);
                                break;
                            }

                            if (newWater.Contains(adjacent)) continue;

                            //if there's air or water below the current block, don't spread sideways
                            if (q != 0 && World.IsValidBlockLocation(belowCurrent) && (Blocks[belowCurrent].Type == Block.BlockType.Air || Blocks[belowCurrent].Type == Block.BlockType.Water)) continue;
                            if (World.IsValidBlockLocation(adjacent) && World.GetBlock(adjacent).Type == Block.BlockType.Air) newWater.Add(adjacent);
                        }
                    }
                }
            }

            if (newWater.Count == 0)
            {
                WaterExpanding = false;
                Debug.WriteLine("Water finished expanding in chunk {0}", ChunkCoords);
                return;
            }

            var addBlocks = new List<AddBlock>();
            Settings.ChunkUpdatesDisabled = true; //change blocks while updates are disabled so chunk is only rebuilt once
            foreach (var newWaterPosition in newWater.Where(newWaterCoords => World.GetBlock(newWaterCoords).Type != Block.BlockType.Water))
            {
                World.PlaceBlock(newWaterPosition, Block.BlockType.Water);
                var temp = newWaterPosition;
                addBlocks.Add(new AddBlock(ref temp, Block.BlockType.Water));
            }
            Settings.ChunkUpdatesDisabled = false;

        }*/

        public bool GrassGrowing { get; set; }
        private const int GRASS_UPDATE_INTERVAL = UPDATES_PER_SECOND * 75 / CHUNK_UPDATE_INTERVAL; //75s
        //private readonly int _grassOffset = Settings.Random.Next(0, GRASS_UPDATE_INTERVAL); //stagger grass growth randomly for each chunk
        /// <summary>Only called for SinglePlayer and Servers.</summary>
/*        private void GrassGrow()
        {
            var possibleChanges = new List<Tuple<Block.BlockType, Position>>();
            for (var x = 0; x < Global.CHUNK_SIZE; x++)
            {
                int worldX = ChunkCoords.WorldCoordsX + x;
                for (var z = 0; z < Global.CHUNK_SIZE; z++)
                {
                    int worldZ = ChunkCoords.WorldCoordsZ + z;
                    for (var y = 0; y <= Math.Min(Global.CHUNK_HEIGHT - 1, HeightMap[x, z] + 1); y++) //look +1 above heightmap as water directly above heightmap could change to ice
                    {
                        var blockType = Blocks[x, y, z].Type;
                        switch (blockType)
                        {
                        case Block.BlockType.Grass:
                        case Block.BlockType.Dirt:
                        case Block.BlockType.Snow:
                        case Block.BlockType.Water:
                        case Block.BlockType.Sand:
                        case Block.BlockType.SandDark:
                            break;
                        default:
                            continue; //continue if this block type can never cause changes
                        }

                        bool hasAirAbove = y >= Global.CHUNK_HEIGHT - 1 || Blocks[x, y + 1, z].Type == Block.BlockType.Air;
                        bool isReceivingSunlight = y > HeightMap[x, z] || (hasAirAbove && World.HasAdjacentBlockReceivingDirectSunlight(worldX, y, worldZ));

                        switch (World.WorldType)
                        {
                        case WorldType.Grass:
                            if (isReceivingSunlight)
                            {
                                switch (blockType)
                                {
                                case Block.BlockType.Dirt:
                                    if (hasAirAbove) possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Grass, new Position(worldX, y, worldZ)));
                                    continue;
                                case Block.BlockType.Snow:
                                    possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Dirt, new Position(worldX, y, worldZ)));
                                    continue;
                                }
                            }
                            else
                            {
                                switch (blockType)
                                {
                                case Block.BlockType.Grass:
                                case Block.BlockType.Snow:
                                    possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Dirt, new Position(worldX, y, worldZ)));
                                    continue;
                                }
                            }
                            break;
                        case WorldType.Desert: //lighting doesnt matter for deserts
                            switch (blockType)
                            {
                            case Block.BlockType.Grass:
                            case Block.BlockType.Snow:
                                possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Sand, new Position(worldX, y, worldZ)));
                                continue;
                            case Block.BlockType.SandDark:
                                if (hasAirAbove) possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Sand, new Position(worldX, y, worldZ)));
                                continue;
                            }
                            break;
                        case WorldType.Winter:
                            switch (blockType)
                            {
                            case Block.BlockType.Water:
                                //water with air above and without more water below can freeze
                                //note: this will cause multiple lightbox updates and chunk queues if multiple water freezes at once because water -> ice is a change in transparency; therefore this is acceptable
                                if (hasAirAbove)
                                {
                                    var hasWaterBelow = y > 0 && Blocks[x, y - 1, z].Type == Block.BlockType.Water;
                                    if (!hasWaterBelow) possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Ice, new Position(worldX, y, worldZ)));
                                }
                                continue;
                            }

                            if (isReceivingSunlight)
                            {
                                switch (blockType)
                                {
                                case Block.BlockType.Dirt:
                                    if (hasAirAbove) possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Snow, new Position(worldX, y, worldZ)));
                                    continue;
                                case Block.BlockType.Grass:
                                    possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Snow, new Position(worldX, y, worldZ)));
                                    continue;
                                }
                            }
                            else
                            {
                                switch (blockType)
                                {
                                case Block.BlockType.Grass:
                                case Block.BlockType.Snow:
                                    possibleChanges.Add(new Tuple<Block.BlockType, Position>(Block.BlockType.Dirt, new Position(worldX, y, worldZ)));
                                    continue;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            if (possibleChanges.Count == 0)
            {
                //this happens after a change is made in the chunk that did not cause any possible grass grow style changes
                GrassGrowing = false;
                Debug.WriteLine("Grass finished growing in chunk {0} No possible changes found", ChunkCoords);
                return;
            }
            Debug.WriteLine("Grass growing in chunk {0} {1} possible change(s)", ChunkCoords, possibleChanges.Count);

            var changesMade = 0;
            var addBlocks = new List<AddBlock>(); //only gets used for servers
            Settings.ChunkUpdatesDisabled = true; //change blocks while updates are disabled so chunk is only rebuilt once
            {
                foreach (var change in possibleChanges)
                {
                    //add some randomness so the changes dont happen all at once
                    if (possibleChanges.Count > 1)
                    {
                        switch (change.Item1) //can assign different percentages based on block type
                        {
                        case Block.BlockType.Ice:
                            if (Settings.Random.NextDouble() > 0.05) continue; //give ice forming a very low chance because its a change in transparency and causes lightbox updates and must queue multiple chunks
                            break;
                        default:
                            if (Settings.Random.NextDouble() > 0.18) continue;
                            break;
                        }
                    }
                    else //when only one possible change is left, greatly increase its chance; prevents tons of chunks lingering performing the logic until the final change gets made
                    {
                        if (Settings.Random.NextDouble() > 0.5) continue;
                    }

                    changesMade++;
                    var changePosition = change.Item2;
                    World.PlaceBlock(changePosition, change.Item1);
                    addBlocks.Add(new AddBlock(ref changePosition, change.Item1));
                }
            }
            Settings.ChunkUpdatesDisabled = false;

            //send updates to multiplayer clients
            if (addBlocks.Count > 0)
            {
                // TODO - send to all players
            }

            if (changesMade == possibleChanges.Count)
            {
                //when all possible changes have been made we can stop GrassGrowing here without waiting for the next iteration to confirm it
                GrassGrowing = false;
                Debug.WriteLine("Grass finished growing in chunk {0} All possible changes made", ChunkCoords);
            }
        }*/
            
    }
}
