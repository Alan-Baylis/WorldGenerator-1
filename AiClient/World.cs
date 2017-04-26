using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sean.Shared;

namespace AiClient
{
    public enum Item
    {
        Wall,
        Water,
        Table,
        Tree,

        Character = 100,
    }

    public class Cell
    {
        List<Item> items;

        public Cell()
        {
            items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }
        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }
        public bool IsLocationSolid()
        {
            foreach (var i in items) {
                if (i != Item.Water)
                    return true;
            }
            return false;
        }
        public bool IsLocationTransparent()
        {
            if (items.Count == 0)
                return true;
            foreach (var i in items) {
                if (i != Item.Water)
                    return false;
            }
            return true;
        }

        public string Render(int timeslice)
        {
            if (items.Count == 0) return ".";
            var i = items[timeslice % items.Count];
            var c = ".";
            switch (i)
            {
                case Item.Wall: c = "#"; break;
                case Item.Water: c = "~"; break;
                case Item.Table: c = "O"; break;
                case Item.Tree: c = "^"; break;
                case Item.Character: c = "@"; break;
                default: c = "?"; break;
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
            if (position.Y == 0)
                return true;
            return world [position.X, position.Z].IsLocationSolid ();
        }
        public bool IsLocationTransparent(Position position)
        { 
            return world [position.X, position.Z].IsLocationTransparent ();
        }

        public Block GetBlock(Position position) { throw new NotImplementedException (); }
        public Block GetBlock(int x, int y, int z) { throw new NotImplementedException (); }
        public int GetBlockHeight(int x, int z) { throw new NotImplementedException(); }

        public void SetBlock(Position position, Block block) { throw new NotImplementedException (); }
        public void SetBlock(int x, int y, int z, Block block) { throw new NotImplementedException (); }

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
            world[10, 10].AddItem(Item.Water);
            world[10, 11].AddItem(Item.Water);
            world[10, 12].AddItem(Item.Water);
            world[9, 11].AddItem(Item.Water);
            world[11, 11].AddItem(Item.Water);

            for (int i = 0; i < 20; i++)
                world[rnd.Next(mapsize), rnd.Next(mapsize)].AddItem(Item.Tree);
        }

        public void Add (int x, int z, Item item)
        {
            world [x, z].AddItem (item);
        }
        public void Remove (int x, int z, Item item)
        {
            world [x, z].RemoveItem (item);
        }
        public void Move (int x, int z, Item item, int x1, int z1)
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
                    str.Append(world[x, z].Render(timeslice));
                }
                gridWindow.WriteLine(str.ToString());
            }
            timeslice++;
        }

    }
}
