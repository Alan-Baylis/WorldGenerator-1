using System;
using System.Collections.Generic;
using System.Text;
using Sean.Shared;

namespace AiClient
{
    public class Cell
    {
        List<BlockType> items;
        Block block;

        public Cell()
        {
            items = new List<BlockType>();
        }

        public void AddBlock(Block block)
        {
            this.block = block;
        }
        public Block GetBlock()
        {
            return block;
        }
        public void AddItem(BlockType item)
        {
            items.Add(item);
        }
        public void RemoveItem(BlockType item)
        {
            items.Remove(item);
        }
        public bool IsLocationSolid()
        {
            return block.IsSolid ;
        }
        public bool IsLocationTransparent()
        {
            if (block.Type == BlockType.Unknown)
                return true;
            return block.IsTransparent ;
        }

        public string Render(int timeslice)
        {
            var c = ".";
            switch (block.Type) {
            case BlockType.Unknown:
                c = ".";
                break;
            case BlockType.Wall:
                c = "#";
                break;
            case BlockType.Water:
                c = "~";
                break;
            case BlockType.Tree:
                c = "^";
                break;
            case BlockType.Food:
                c = "+";
                break;
            case BlockType.Character:
                c = "@";
                break;
            default:
                c = "?";
                break;
            }
            return c;
        }
    }

    public class World : IWorld
    {
        #region IWorld
        public bool IsValidBlockLocation (int x, int y, int z) {
            return x >= 0 && z >= 0 && x < mapsize && z < mapsize;
        }
        public bool IsLoadedBlockLocation(Position position) { 
            return IsValidBlockLocation(position.X, position.Y, position.Z);
        }
        public bool IsLoadedBlockLocation(int x, int y, int z) {
            return IsValidBlockLocation (x, y, z);
        }

        public bool IsLocationSolid(Position position)
        { 
            if (position.Y <= 0)
                return true;
            return world [position.X, position.Z].IsLocationSolid ();
        }
        public bool IsLocationTransparent(Position position)
        { 
            return world [position.X, position.Z].IsLocationTransparent ();
        }

        public Block GetBlock(Position position)
        {
            return GetBlock (position.X, position.Y, position.Z);
        }
        public Block GetBlock(int x, int y, int z) {
            if (y <= 0)
                return new Block(BlockType.Rock);
            return world [x, z].GetBlock ();
        }
        public int GetBlockHeight(int x, int z) { throw new NotImplementedException(); }

        public void SetBlock(Position position, Block block) { 
            SetBlock (position.X, position.Y, position.Z, block);
        }
        public void SetBlock(int x, int y, int z, Block block){
            world[x,z].AddBlock(block);
        }

        public bool IsChunkLoaded(ChunkCoords coords) { throw new NotImplementedException (); }
        public Chunk GetChunk(ChunkCoords coords) { throw new NotImplementedException (); }

        public Array<byte> GlobalMap { get; }
        public Array<byte> GlobalMapTerrain { get; }

        public Position GetRandomLocationOnLoadedChunk() { throw new NotImplementedException (); }

        public int LoadedChunkCount { get; }
        #endregion
    
        readonly int mapsize;
        Array<Cell> world;
        int timeslice = 1;
        Random rnd = new Random();

        public World(int mapsize)
        {
            this.mapsize = mapsize;
            world = new Array<Cell>(mapsize,mapsize);

            for (int z = 0; z < mapsize; z++)
            {
                for (int x = 0; x < mapsize; x++)
                {
                    world[x, z] = new Cell();
                }
            }
            world[10, 10].AddBlock(new Block(BlockType.Water));
            world[10, 11].AddBlock(new Block(BlockType.Water));
            world[10, 12].AddBlock(new Block(BlockType.Water));
            world[9, 11].AddBlock(new Block(BlockType.Water));
            world[11, 11].AddBlock(new Block(BlockType.Water));

            for (int i = 0; i < 20; i++)
                world[rnd.Next(mapsize), rnd.Next(mapsize)].AddBlock(new Block(BlockType.Tree));
            for (int i = 0; i < 20; i++)
                world[rnd.Next(mapsize), rnd.Next(mapsize)].AddBlock(new Block(BlockType.Food));
        }

        public void Add (int x, int z, BlockType item)
        {
            world [x, z].AddItem (item);
        }
        public void Remove (int x, int z, BlockType item)
        {
            world [x, z].RemoveItem (item);
        }
        public void Move (int x, int z, BlockType item, int x1, int z1)
        {
            Add (x1, z1, item);
            Remove (x, z, item);
        }
        public void Render(DisplayConsoleWindow gridWindow)
        {
            for (int z = 0; z < mapsize; z++)
            {
                var str = new StringBuilder();
                for (int x = 0; x < mapsize; x++)
                {
                    if (Program.Engine.CharacterAt(x, z))
                        str.Append('@');
                    else
                        str.Append(world[x, z].Render(timeslice));
                }
                gridWindow.WriteLine(str.ToString());
            }
            timeslice++;
        }

    }
}
