using System;
using System.Collections.Generic;
using System.Text;
using Sean.Shared;
using Sean.WorldGenerator;

namespace Tests
{
    public static class WaterTests
    {
        private enum BlockType
        {
            Air = 0,
            Dirt = 1,
            Water = 2,
            FallingWater = 3,
            WaterSource = 4
        }
        private class Coord
        {
            public int X;
            public int Y;
            public Coord(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object obj)
            {
                return X == ((Coord)obj).X && Y == ((Coord)obj).Y;
            }
        }
        private class Grid
        {
            private const int Width = 20;
            private const int Height = 10;
            private int[,] grid;

            public Grid()
            {
                grid = new int[Width, Height];
            }

            public int this[int x, int y]
            {
                get { return GetBlock(x, y); }
                set { SetBlock(x, y, value); }
            }

            public void Render()
            {
                for (int y = Height - 1; y >= 0; y--)
                {
                    var str = new StringBuilder();
                    for (int x = 0; x < Width; x++)
                    {
                        str.Append((int)GetBlock(x, y));
                    }
                    Console.WriteLine(str);
                }
            }

            public int GetBlock(Coord coord)
            {
                return GetBlock(coord.X, coord.Y);
            }
            public int GetBlock(int x, int y)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return 0;
                return grid[x, y];
            }
            public void SetBlock(Coord coord, int block)
            {
                SetBlock(coord.X, coord.Y, block);
            }
            public void SetBlock(int x, int y, int block)
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return;
                grid[x, y] = block;
            }
        }
        private class BlockGrid
        {
            private const int Width = 20;
            private const int Height = 10;
            private BlockType [,] grid;
            private static List<char> Characters = new List<char> { ' ', '#', '~', '"', '*' };

            public BlockGrid ()
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
                for (int y = Height-1; y >=0; y--) {
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

                        if (GetBlock (x+1, y+1) == block)
                            results.Add (new Coord(x+1,y+1));
                        if (GetBlock (x+1, y-1) == block)
                            results.Add (new Coord(x+1,y-1));
                        if (GetBlock (x-1, y-1) == block)
                            results.Add (new Coord(x-1,y-1));
                        if (GetBlock (x-1, y+1) == block)
                            results.Add (new Coord(x-1,y+1));
                    }
                }
                return results;
            }

            public List<Coord> FindStream(Coord coord)
            {
                var stream = new List<Coord> ();
                FollowStream(ref stream, coord.X, coord.Y);
                return stream;
            }
            private void FollowStream(ref List<Coord> s, int x,int y)
            {
                var coord = new Coord(x, y);
                if (s.Contains(coord))
                    return;
                s.Add(coord);
                var g = grid[x + 1, y];
                if (g== BlockType.Water || g == BlockType.FallingWater || g == BlockType.WaterSource)
                    FollowStream(ref s, x + 1, y);
                g = grid[x - 1, y];
                if (g == BlockType.Water || g == BlockType.FallingWater || g == BlockType.WaterSource)
                    FollowStream(ref s, x - 1, y);
                g = grid[x, y+1];
                if (g == BlockType.Water || g == BlockType.FallingWater || g == BlockType.WaterSource)
                    FollowStream(ref s, x, y+1);
                g = grid[x, y-1];
                if (g == BlockType.Water || g == BlockType.FallingWater || g == BlockType.WaterSource)
                    FollowStream(ref s, x, y-1);
            }

            private void AddPressure(int x, int y, int p, ref Grid results)
            {
                if (results.GetBlock (x, y) != 0)
                    return;
                results.SetBlock (x, y, p);
                if (grid [x + 1, y] == BlockType.Water)
                    AddPressure (x + 1, y, p, ref results);

                if (grid [x - 1, y] == BlockType.Water)
                    AddPressure (x - 1, y, p, ref results);

                if (grid [x, y-1] == BlockType.Water)
                    AddPressure (x, y-1, p+1, ref results);
            }

            public Grid CalcPressure()
            {
                // Find highest water
                var basePress = new Grid();
                for (int y = Height-1; y >=0; y--) {
                    for (int x = 0; x < Width; x++) {
                        if (grid [x, y] == BlockType.Water && grid[x,y+1] != BlockType.Dirt)
                        {
                            AddPressure (x, y, 1, ref basePress);
                        }
                    }
                }
                    
                //var cells = new UniqueQueue<Coord>();
                //foreach (var w in Find(BlockType.Water))
                //{
                //    if (grid[w.X, w.Y + 1] != BlockType.Water)
                //    {
                //        basePress[w.X, w.Y] = 1;
                //        cells.Enqueue(w);
                //    }
                //}
                //
                //for (int y = 0; y < Height; y++)
                //{
                //    for (int x = 0; x < Width; x++)
                //    {
                //        basePress[x, y] = CalcPressure(x, y);
                //    }
                //}
                return basePress;
            }
            public int CalcPressure(int x,int y)
            {
                int p = 1;
                if (grid[x,y] == BlockType.Water)
                    return p + CalcPressure(x, y + 1);
                //if (grid[x, y] == BlockType.WaterSource)
                //    return 9;
                return 0;
            }
        }

        public static void Test ()
        {
            var grid = new BlockGrid();
            grid.LoadRow (9, "####################");
            grid.LoadRow (8, "#                  #");
            grid.LoadRow (7, "#          #   #####");
            grid.LoadRow (6, "#  ####### #####   #");
            grid.LoadRow (5, "# #######          #");
            grid.LoadRow (4, "#  ##  ##      #   #");
            grid.LoadRow (3, "#* ##  ###     #   #");
            grid.LoadRow (2, "#  ##  #       #   #");
            grid.LoadRow (1, "#      #####       #");
            grid.LoadRow (0, "####################");

            for (int i = 0; i < 10; i++) {
                grid.Render ();
                Process (grid);
                System.Threading.Thread.Sleep (5000);
            }
        }
            
        private static void Process(BlockGrid grid)
        {            
            var pres = grid.CalcPressure();
            pres.Render();

            var water = grid.Find (BlockType.Water);
            water.AddRange(grid.Find (BlockType.WaterSource));
            var moveTo = new SortedList<int,Coord>();
            foreach (var w in water) {
                if (grid [w.X+1, w.Y] == BlockType.Air)
                    moveTo.Add (w.Y, new Coord (w.X+1, w.Y));
                if (grid [w.X, w.Y+1] == BlockType.Air)
                    moveTo.Add (w.Y+1, new Coord (w.X, w.Y+1));
                if (grid [w.X-1, w.Y] == BlockType.Air)
                    moveTo.Add (w.Y, new Coord (w.X-1, w.Y));
                if (grid [w.X, w.Y-1] == BlockType.Air)
                    moveTo.Add (w.Y-1, new Coord (w.X, w.Y-1));
            }

            grid.SetBlock(moveTo.Values[0], BlockType.Water);


            /*
            foreach (var s in grid.Find(BlockType.WaterSource))
            {
                var stream = grid.FindStream(s);
                var p = new List<Coord>();
                foreach(var c in stream)
                {
                    if (pres[c.X, c.Y] == 0)
                    {
                        // Falling
                        if (grid[c.X, c.Y - 1] == BlockType.Air)
                            p.Add(new Coord(c.X, c.Y - 1));
                        else if (grid[c.X + 1, c.Y - 1] == BlockType.Air)
                            p.Add(new Coord(c.X + 1, c.Y - 1));
                        else if (grid[c.X - 1, c.Y - 1] == BlockType.Air)
                            p.Add(new Coord(c.X - 1, c.Y - 1));
                    }
                    else if (pres[c.X,c.Y] < 0)
                    {
                        // Up
                        if (grid[c.X, c.Y + 1] == BlockType.Air)
                            p.Add(new Coord(c.X, c.Y + 1));
                        else if (grid[c.X + 1, c.Y + 1] == BlockType.Air)
                            p.Add(new Coord(c.X + 1, c.Y + 1));
                        else if (grid[c.X - 1, c.Y + 1] == BlockType.Air)
                            p.Add(new Coord(c.X - 1, c.Y + 1));
                    }
                    else
                    {
                        // Sideways
                        if (grid[c.X + 1, c.Y] == BlockType.Air)
                            p.Add(new Coord(c.X + 1, c.Y));
                        else if (grid[c.X - 1, c.Y] == BlockType.Air)
                            p.Add(new Coord(c.X - 1, c.Y));
                    }
                }
                grid.SetBlock(p[0], BlockType.Water);
            }
            */


            //var water = grid.FindNeighbours(BlockType.Water);
            //water.AddRange(grid.FindNeighbours(BlockType.FallingWater));
            //foreach (var s in water)
            //{
            //    if (grid[s.X,s.Y-1] == BlockType.Dirt)
            //    {
            //        grid[s.X, s.Y] = BlockType.Water;
            //    }
            //    if (grid[s.X, s.Y - 1] == BlockType.Air)
            //    {
            //        grid[s.X, s.Y-1] = BlockType.FallingWater;
            //    }
            //}

            //foreach (var s in grid.Find(BlockType.Water))
            //{
            //    if (grid[s.X,s.Y-1] == BlockType.Air)
            //    {
            //        grid[s.X, s.Y] = BlockType.FallingWater;
            //    }
            //    else if (grid[s.X+1,s.Y-1] == BlockType.Air)
            //    {
            //        grid[s.X, s.Y] = BlockType.Air;
            //        grid[s.X + 1, s.Y - 1] = BlockType.Water;
            //    }
            //    else if (grid[s.X-1,s.Y-1] == BlockType.Air)
            //    {
            //        grid[s.X, s.Y] = BlockType.Air;
            //        grid[s.X - 1, s.Y - 1] = BlockType.Water;
            //    }
            //}

            //foreach (var s in grid.Find(BlockType.Water))
            //{
            //    if (grid[s.X, s.Y - 1] == BlockType.Air)
            //    {
            //        grid[s.X, s.Y - 1] = BlockType.FallingWater;
            //        grid[s.X, s.Y] = BlockType.Air;
            //    }
            //}

        }
       
    }
}

