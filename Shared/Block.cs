using Sean.Shared.Textures;

namespace Sean.Shared
{
    public interface IBlock
    {
        bool IsSolid { get; }
        bool IsTransparent { get; }
        BlockType Type { get; }
    }

    /// <summary>Block types starting with 'Placeholder' are not included in the action picker buttons grid and will appear white because they dont have associated textures.</summary>
    public enum BlockType : byte
    {
        Unknown = 0,

        //Naturally occurring
        Air = 1,
        Ocean = 2,
        Dirt = 3,
        Grass,
        Snow,
        Sand,
        SandDark,
        Ice,
        Gravel,
        Rock,
        Coal,
        Copper,
        Iron,
        Gold,
        Oil,
        Tree,
        ElmTree,
        Leaves,
        SnowLeaves,
        Lava,
        LavaRock,

        // Water
        WaterSource = 30,
        Water1, // Puddle
        Water2,
        Water3,
        Water4,
        Water5,
        Water6,
        Water7,
        Water,// fill 1 block deep
        UnderWater,

        GrassSlopeN = 40,
        GrassSlopeS,
        GrassSlopeE,
        GrassSlopeW,
        GrassSlopeNW,
        GrassSlopeNE,
        GrassSlopeSE,
        GrassSlopeSW,
        GrassSlopeNEW,
        GrassSlopeNES,
        GrassSlopeESW,
        GrassSlopeNWS,

        //Crafted Material
        WoodTile1 = 60,
        WoodTile2,
        Bricks,
        Cobble = 64,

        //Crafted Item
        PlaceholderWorkBench = 100,
        PlaceholderStove,
        PlaceholderFurnace,
        PlaceholderPipeline,
        Crate,
        Placeholder2, //removed, leaving placeholder to not break worlds
        Shelf1,
        SteelDoorTop,
        SteelDoorBottom,
        Placeholder3, //removed, leaving placeholder to not break worlds
        Speaker,
        PrisonBars,

        //Other
        Placeholder4 = 220, //removed, leaving placeholder to not break worlds
        Placeholder5, //removed, leaving placeholder to not break worlds
        Placeholder1, //removed, leaving placeholder to not break worlds
        Placeholder6, //removed, leaving placeholder to not break worlds
        Placeholder7, //removed, leaving placeholder to not break worlds
        Placeholder8, //removed, leaving placeholder to not break worlds
        FancyBlack,
        FancyGreen,
        FancyRed,
        FancyWhite,
        Placeholder9, //removed, leaving placeholder to not break worlds
        SteelPlate,
        SteelPlate2,
    }

	public struct Block : IBlock
	{
        public ushort BlockData;

		public Block(BlockType type)
		{
			BlockData = (ushort)type;
		}

		public Block(ushort blockData)
		{
			BlockData = blockData;
		}

		public BlockType Type
		{
			get { return (BlockType)(BlockData & 0xFF); }
			set { BlockData = (ushort)(BlockData & 0xFF00 | (byte)value); }
		}

        public Facing Orientation
		{
            get { return (Facing)(BlockData >> 8 & 0x3); }
			set { BlockData = (ushort)((BlockData & 0xFCFF) | (byte)value << 8); }
		}

		/// <summary>Is this block solid. Solid blocks cause collision.</summary>
		/// <remarks>Note that some transparent blocks can be considered solid if they also cause collision.</remarks>
		public bool IsSolid
		{
			get { return IsBlockTypeSolid(Type); }
		}
        public bool IsWater {
            get { return IsBlockTypeWater (Type); }
        }
        public int WaterHeight
        {
            get { return GetWaterHeight(Type); }
        }

		public static bool IsBlockTypeSolid(BlockType type)
		{
			switch (type)
			{
				case BlockType.Air:
				case BlockType.Ocean:
					return false;
				default:
					return true;
			}
		}
        public static bool IsBlockTypeWater(BlockType type)
        {
            switch (type) {
            case BlockType.Water1:
            case BlockType.Water2:
            case BlockType.Water3:
            case BlockType.Water4:
            case BlockType.Water5:
            case BlockType.Water6:
            case BlockType.Water7:
            case BlockType.Water:
            case BlockType.WaterSource:
            case BlockType.Ocean:
            case BlockType.UnderWater:
                return true;
            default:
                return false;
            }
        }
        public static int GetWaterHeight(BlockType type)
        {
            switch (type)
            {
                case BlockType.Water1: return 1;
                case BlockType.Water2: return 2;
                case BlockType.Water3: return 3;
                case BlockType.Water4: return 4;
                case BlockType.Water5: return 5;
                case BlockType.Water6: return 6;
                case BlockType.Water7: return 7;
                case BlockType.Water: return 8;
                case BlockType.WaterSource: return 8;
                case BlockType.UnderWater: return 8;
                case BlockType.Ocean: return 8;
                default: return 0;
            }

        }

        public static bool IsBlockTypeTree(BlockType type)
        {
            switch (type)
            {
            case BlockType.ElmTree:
            case BlockType.Tree:
                return true;
            default:
                return false;
            }
        }

		/// <summary>Is this block transparent.</summary>
		/// <remarks>Note that some blocks are transparent but still considered solid.</remarks>
		public bool IsTransparent
		{
			get { return IsBlockTypeTransparent(Type); }
		}

		public static bool IsBlockTypeTransparent(BlockType type)
		{
			switch (type)
			{
                case BlockType.Air:
				case BlockType.Leaves:
				case BlockType.SnowLeaves:
                case BlockType.Water1:
                case BlockType.Water2:
                case BlockType.Water3:
                case BlockType.Water4:
                case BlockType.Water5:
                case BlockType.Water6:
                case BlockType.Water7:
                case BlockType.Water:
                case BlockType.WaterSource:
                case BlockType.UnderWater:
                case BlockType.Ocean:
                case BlockType.PrisonBars:
				case BlockType.SteelDoorTop:
					return true;
				default:
					return false;
			}
		}

		/// <summary>Is this block a light emitting source.</summary>
		public bool IsLightSource
		{
			get { return LightStrength > 0; }
		}

		/// <summary>Light strength this block emits.</summary>
		public byte LightStrength
		{
			get
			{
				switch (Type)
				{
					case BlockType.Lava:
						return 11;
					case BlockType.LavaRock:
						return 10;
					default:
						return 0;
				}
			}
		}

		public static BlockTextureType FaceTexture(BlockType type, Face face)
		{
			switch (type)
			{
				case BlockType.Ocean:
					return BlockTextureType.Water;
				case BlockType.Grass:
					switch (face)
					{
						case Face.Top:
							return BlockTextureType.Grass;
						case Face.Bottom:
							return BlockTextureType.Dirt;
						default:
							return BlockTextureType.GrassSide;
					}
				case BlockType.Bricks:
					return BlockTextureType.Bricks;
				case BlockType.Coal:
					return BlockTextureType.Coal;
				case BlockType.Cobble:
					return BlockTextureType.Cobble;
				case BlockType.Copper:
					return BlockTextureType.Copper;
				case BlockType.Crate:
					switch (face)
					{
						case Face.Top:
						case Face.Bottom:
							return BlockTextureType.CrateSide;
						default:
							return BlockTextureType.Crate;
					}
				case BlockType.Dirt:
					return BlockTextureType.Dirt;
				case BlockType.FancyBlack:
					return BlockTextureType.FancyBlack;
				case BlockType.FancyGreen:
					return BlockTextureType.FancyGreen;
				case BlockType.FancyRed:
					return BlockTextureType.FancyRed;
				case BlockType.FancyWhite:
					return BlockTextureType.FancyWhite;
				case BlockType.Gold:
					return BlockTextureType.Gold;
				case BlockType.Gravel:
					return BlockTextureType.Gravel;
				case BlockType.WoodTile1:
					return BlockTextureType.WoodTile1;
				case BlockType.Ice:
					return BlockTextureType.Ice;
				case BlockType.Iron:
					return BlockTextureType.Iron;
				case BlockType.Lava:
					return BlockTextureType.Lava;
				case BlockType.LavaRock:
					return BlockTextureType.LavaRock;
				case BlockType.Leaves:
					return BlockTextureType.Leaves;
				case BlockType.WoodTile2:
					return BlockTextureType.WoodTile2;
				case BlockType.Oil:
					return BlockTextureType.Oil;
				case BlockType.PrisonBars:
					return BlockTextureType.PrisonBars;
				case BlockType.Sand:
					//this will prevent beaches in winter worlds; side effect is that any sand placed will instantly get snow on top
					//might be nice to have a SandSnowSide texture to make it look better, however that would require another block type, so this is ok for now
// TODO					if (WorldData.WorldType == WorldType.Winter && face == Face.Top) return BlockTextureType.Snow;
					return BlockTextureType.Sand;
				case BlockType.SandDark:
					return BlockTextureType.SandDark;
				case BlockType.Shelf1:
					return BlockTextureType.Shelf1;
				case BlockType.SteelDoorTop:
					return BlockTextureType.SteelDoorTop;
				case BlockType.SteelDoorBottom:
					return BlockTextureType.SteelDoorBottom;
				case BlockType.Snow:
					switch (face)
					{
						case Face.Top:
							return BlockTextureType.Snow;
						case Face.Bottom:
							return BlockTextureType.Dirt;
						default:
							return BlockTextureType.SnowSide;
					}
				case BlockType.SnowLeaves:
					return BlockTextureType.SnowLeaves;
				case BlockType.Speaker:
					return BlockTextureType.Speaker;
				case BlockType.SteelPlate:
					return BlockTextureType.SteelPlate;
				case BlockType.SteelPlate2:
					return BlockTextureType.SteelPlate2;
				case BlockType.Rock:
					return BlockTextureType.Rock;
				case BlockType.Tree:
					switch (face)
					{
						case Face.Top:
						case Face.Bottom:
							return BlockTextureType.TreeTrunk;
						default:
							return BlockTextureType.Tree;
					}
				case BlockType.ElmTree:
					switch (face)
					{
						case Face.Top:
						case Face.Bottom:
							return BlockTextureType.TreeTrunk;
						default:
							return BlockTextureType.ElmTree;
					}
				default:
					return BlockTextureType.Air;
			}
		}

        public bool IsDirty
		{
			get { return (BlockData & 0x8000) != 0; }
		}
	}
}
