using Sean.WorldGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            currentChunk = new ChunkCoords(100, 100);
            textBox1.Text = "Keys: W,A,S,D";
            DrawMaps();
        }

        private void DrawMaps()
        {
            var currentCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            //this.localPictureBox.Image = this.DrawLocal(this.localPictureBox.Width, this.localPictureBox.Height);
            //this.worldPictureBox.Image = this.DrawAllChunks(this.worldPictureBox.Width, this.worldPictureBox.Height);
            //this.pictureBox1.Image = this.DrawGlobalMap(this.pictureBox1.Width, this.pictureBox1.Height);
            this.terrainPictureBox.Image = this.DrawTerrain(this.terrainPictureBox.Width, this.terrainPictureBox.Height);

            this.Cursor = currentCursor;
        }

        private ChunkCoords currentChunk;

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            bool update = false;
            switch (e.KeyChar)
            {
                case 'w':
                    currentChunk = new ChunkCoords(currentChunk.X, currentChunk.Z - 1);
                    update = true;
                    break;
                case 's':
                    currentChunk = new ChunkCoords(currentChunk.X, currentChunk.Z + 1);
                    update = true;
                    break;
                case 'a':
                    currentChunk = new ChunkCoords(currentChunk.X - 1, currentChunk.Z);
                    update = true;
                    break;
                case 'd':
                    currentChunk = new ChunkCoords(currentChunk.X + 1, currentChunk.Z);
                    update = true;
                    break;
            }
            if (update)
            {
                DrawMaps();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        public Bitmap DrawLocal(int width, int height)
        {
            var chunk = World.GetChunk(currentChunk, 1);
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var xSize = width / chunk.ChunkSize;
            var zSize = height / chunk.ChunkSize;
            for (int x = 0; x < chunk.ChunkSize; x++)
            {
                for (int z = 0; z < chunk.ChunkSize; z++)
                {
                    var pt = chunk.HeightMap[x + chunk.HeightMap.Size.minX, z + chunk.HeightMap.Size.minZ];
                    var color = pt < 20 ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 0, pt, 0);
                    graphics.FillRectangle(new SolidBrush(color), x * xSize, z * zSize, xSize, zSize);
                }
            }
            return bitmap;
        }

        public Bitmap DrawAllChunks(int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int xOri = 0;
            for (int x = World.MinXChunk; x <= World.MaxXChunk; x++)
            {
                int zOri = 0;
                for (int z = World.MinZChunk; z <= World.MaxZChunk; z++)
                {
                    var coords = new ChunkCoords(x, z);
                    if (World.IsChunkLoaded(coords))
                    {
                        var chunk = World.GetChunk(coords, 1);
                        DrawChunk(graphics, chunk, xOri, zOri);
                    }
                    if (coords == currentChunk)
                    {
                        graphics.DrawRectangle(new Pen(Color.FromArgb(255, 255,0,0)), xOri, zOri, World.ChunkSize-1, World.ChunkSize-1);
                    }
                    zOri = zOri + World.ChunkSize;
                }
                xOri = xOri + World.ChunkSize;
            }
            return bitmap;
        }
        private void DrawChunk(Graphics graphics, Chunk chunk, int xOri, int zOri)
        {
            for (int x = 0; x < chunk.ChunkSize; x++)
            {
                for (int z = 0; z < chunk.ChunkSize; z++)
                {
                    var pt = chunk.HeightMap[x + chunk.HeightMap.Size.minX, z + chunk.HeightMap.Size.minZ];
                    var color = pt < 20 ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 0, pt, 0);
                    graphics.FillRectangle(new SolidBrush(color), xOri + x, zOri + z, 1, 1);
                }
            }
        }

        private Bitmap DrawGlobalMap(int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var map = World.GetGlobalMap();
            int x1 = 1;
            for (int x = map.Size.minX; x < map.Size.maxX; x=x+map.Size.scale)
            {
                int z1 = 1;
                for (int z = map.Size.minZ; z < map.Size.maxZ; z=z+map.Size.scale)
                {
                    var pt = map[x,z];
                    var color = World.IsGlobalMapWater(x, z) ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 0, pt, 0);
                    //var color = Color.FromArgb(255, 0, pt, 0);
                    graphics.FillRectangle(new SolidBrush(color), x1, z1, 1,1);
                    if (x1 == currentChunk.X && z1 == currentChunk.Z)
                        graphics.DrawRectangle(new Pen(Color.FromArgb(255, 255,0,0)), x1, z1, 1, 1);
                    z1++;
                }
                x1++;
            }
            return bitmap;
        }

        
        private Bitmap DrawTerrain(int width, int height)
        {
            var boxImage = imageList.Images["box_blue"];
            var chunk = World.GetChunk(currentChunk, 1);
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var xSize = width / chunk.ChunkSize;
            var zSize = height / chunk.ChunkSize;
            for (int x = 0; x < chunk.ChunkSize; x++)
            {
                for (int z = 0; z < chunk.ChunkSize; z++)
                {
                    for (int y = 20; y < 64; y++)
                    {
                        //var pt = chunk.HeightMap[x + chunk.HeightMap.Size.minX, z + chunk.HeightMap.Size.minZ];
                        if (chunk.Blocks[x, y, z].IsSolid)
                        {
                            var color = Color.FromArgb(255, 0, 255, 0);
                            graphics.FillRectangle(new SolidBrush(color), (width/2)+((x-z)*2), (height/2)+((x+z)*2)-(y-64)*2, 2, 2);

                            //graphics.DrawImage(boxImage, new Point((width / 2) + ((x - z) * 8), (height / 2) + ((x + z) * 10) - (y - 64) * 10));
                        }
                    }
                }
            }
            return bitmap;
        }
        

        /*
        private int seed = 123;
        private int unitSize = 100;
        private int octaveCount = 3;
        private double persistence = 0.7;
        private int scale = 1;
        private Bitmap GetImage1(int width, int height)
        {
            int s = 4;
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            ArraySize size = new ArraySize()
            {
                minX = 0,
                maxX = width / s,
                minZ = 0,
                maxZ = height / s,
                minY = 0,
                maxY = 255 / s,
                scale = scale
            };
            PerlinNoise noise = new PerlinNoise(seed, unitSize);
            var map = noise.GetIntMap(size, octaveCount, persistence);
            int x1 = 1;
            for (int x = map.Size.minX; x < map.Size.maxX; x = x + map.Size.scale)
            {
                int z1 = 1;
                for (int z = map.Size.minZ; z < map.Size.maxZ; z = z + map.Size.scale)
                {
                    var pt = map[x, z] * s;
                    var color = Color.FromArgb(255, 0, pt, 0);
                    graphics.FillRectangle(new SolidBrush(color), x1 * s, z1 * s, s, s);
                    z1++;
                }
                x1++;
            }
            return bitmap;
        }
        */

        private void Form1_Resize(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
