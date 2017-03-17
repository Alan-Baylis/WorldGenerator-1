using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class Map
    {
        private const int MAPSIZE = 70;
        int[,] array = new int[4, 2];
        private Cell[,] world;
        int timeslice = 1;
        Random rnd = new Random();

        public Map()
        {
            world = new Cell[MAPSIZE, MAPSIZE];
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
