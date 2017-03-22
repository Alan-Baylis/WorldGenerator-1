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
        public bool IsValidBlockLocation (int x, int y, int z) { throw new NotImplementedException (); }
        public bool IsLoadedBlockLocation(Position position) { throw new NotImplementedException (); }
        public bool IsLoadedBlockLocation(int x, int y, int z) { throw new NotImplementedException (); }

        public Block GetBlock(Position position) { throw new NotImplementedException (); }
        public Block GetBlock(int x, int y, int z) { throw new NotImplementedException (); }

        public void SetBlock(Position position, Block block) { throw new NotImplementedException (); }
        public void SetBlock(int x, int y, int z, Block block) { throw new NotImplementedException (); }

        public bool IsChunkLoaded(ChunkCoords coords) { throw new NotImplementedException (); }
        public Chunk GetChunk(ChunkCoords coords) { throw new NotImplementedException (); }

        public Array<byte> GlobalMap { get; }
        public Array<byte> GlobalMapTerrain { get; }

        public Position GetRandomLocationOnLoadedChunk() { throw new NotImplementedException (); }

        public int LoadedChunkCount { get; }
        #endregion
    
        private const int MAPSIZE = 70;
        Array<Cell> world = new Array<Cell>(MAPSIZE,MAPSIZE);
        int timeslice = 1;
        Random rnd = new Random();

        public World()
        {
            for (int y = 0; y < MAPSIZE; y++)
            {
                for (int x = 0; x < MAPSIZE; x++)
                {
                    world[x, y] = new Cell();
                }
            }
            world[10, 10].AddItem(Item.Water);
            world[10, 11].AddItem(Item.Water);
            world[10, 12].AddItem(Item.Water);
            world[9, 11].AddItem(Item.Water);
            world[11, 11].AddItem(Item.Water);

            for (int i = 0; i < 20; i++)
                world[rnd.Next(MAPSIZE), rnd.Next(MAPSIZE)].AddItem(Item.Tree);
        }

        public void Add (int x, int y, Item item)
        {
            world [x, y].AddItem (item);
        }
        public void Remove (int x, int y, Item item)
        {
            world [x, y].RemoveItem (item);
        }
        public void Move (int x, int y, Item item, int x1, int y1)
        {
            Add (x1, y1, item);
            Remove (x, y, item);
        }
        public void Render()
        {
            Console.Clear();
            for (int y = 0; y < MAPSIZE; y++)
            {
                var str = new StringBuilder();
                for (int x = 0; x < MAPSIZE; x++)
                {
                    str.Append(world[x, y].Render(timeslice));
                }
                Console.WriteLine(str);
            }
            timeslice++;
        }
    }
}
