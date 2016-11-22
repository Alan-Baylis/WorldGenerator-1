using System;
using System.Collections.Generic;
using System.Text;
using Sean.Shared;

namespace Tests
{
    public static class WaterTests
    {
        private enum BlockType
        {
            Air = 0,
            Dirt = 1,
            Water = 2,
            WaterSource = 3,
        }
        private class Coord
        {
            public int X;
            public int Y;
            public Coord(int x,int y)
            {
                X = x;
                Y = y;
            }
        }
        private class Grid
        {
            private const int Width = 20;
            private const int Height = 10;
            private BlockType [,] grid;
            private static List<char> Characters = new List<char> { ' ', '#', '~', '*' };

            public Grid ()
            {
                grid = new BlockType [Width, Height];
            }

            public BlockType this [int x, int y] {
                get { return GetBlock (x, y); }
                set { SetBlock (x, y, value); }
            }

            public void LoadRow (int y, string row)
            {
                int x = 0;
                foreach (var chr in row) {
                    var block = (BlockType)Characters.IndexOf (chr);
                    SetBlock (x, y, block);
                    x++;
                }
            }

            public void Render ()
            {
                for (int y = Height - 1; y >= 0; y--) {
                    var str = new StringBuilder ();
                    for (int x = 0; x < Width; x++) {
                        char chr = Characters [(int)GetBlock (x, y)];
                        str.Append (chr);
                    }
                    Console.WriteLine (str);
                }
            }

            public BlockType GetBlock (Coord coord)
            {
                return GetBlock (coord.X, coord.Y);
            }
            private BlockType GetBlock (int x, int y)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return BlockType.Dirt;
                return grid [x, y];
            }
            public void SetBlock (Coord coord, BlockType block)
            {
                SetBlock (coord.X, coord.Y, block);
            }
            private void SetBlock (int x, int y, BlockType block)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return;
                grid [x, y] = block;
            }
            public List<Coord> Find (BlockType block)
            {
                var results = new List<Coord> ();
                for (int y = 0; y < Height; y++) {
                    for (int x = 0; x < Width; x++) {
                        if (GetBlock (x, y) == block)
                            results.Add (new Coord(x,y));
                    }
                }
                return results;
            }
            public List<Coord> FindNeighbours (BlockType block)
            {
                var results = new List<Coord> ();
                for (int y = 0; y < Height; y++) {
                    for (int x = 0; x < Width; x++) {
                        if (GetBlock (x+1, y) == block)
                            results.Add (new Coord(x+1,y));
                        if (GetBlock (x-1, y) == block)
                            results.Add (new Coord(x-1,y));
                        if (GetBlock (x, y+1) == block)
                            results.Add (new Coord(x,y+1));
                        if (GetBlock (x, y-1) == block)
                            results.Add (new Coord(x,y-1));
                    }
                }
                return results;
            }
        }


        public static void Test ()
        {
            var grid = new Grid();
            grid.LoadRow (9, "####################");
            grid.LoadRow (8, "#                  #");
            grid.LoadRow (7, "#          #   #####");
            grid.LoadRow (6, "#  #####   ####    #");
            grid.LoadRow (5, "# #######          #");
            grid.LoadRow (4, "# ###  ##      #   #");
            grid.LoadRow (3, "#*###  ###     #   #");
            grid.LoadRow (2, "# ###  ##      #   #");
            grid.LoadRow (1, "#  #      ##       #");
            grid.LoadRow (0, "####################");

            for (int i = 0; i < 10; i++) {
                grid.Render ();
                Process (grid);
                System.Threading.Thread.Sleep (5000);
            }
        }

        private static void Process(Grid grid)
        {
            foreach (var source in grid.FindNeighbours(BlockType.Water))
            {
                if (grid [source.X, source.Y] == BlockType.Air) {
                }
            }

            foreach (var source in grid.Find(BlockType.Water))
            {
                if (grid [source.X, source.Y - 1] == BlockType.Air) {
                    grid [source.X, source.Y - 1] = BlockType.Water;
                    grid [source.X, source.Y] = BlockType.Air;
                }
            }
            
            foreach (var source in grid.Find(BlockType.WaterSource))
            {
                if (grid [source.X + 1, source.Y] == BlockType.Air)
                    grid [source.X + 1, source.Y] = BlockType.Water;
                if (grid[source.X-1,source.Y] == BlockType.Air)
                    grid [source.X - 1, source.Y] = BlockType.Water;
                if (grid[source.X,source.Y+1] == BlockType.Air)
                    grid [source.X, source.Y+1] = BlockType.Water;
                if (grid[source.X,source.Y-1] == BlockType.Air)
                    grid [source.X, source.Y-1] = BlockType.Water;
            }
        }
       
    }
}

