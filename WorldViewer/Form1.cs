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
            this.localPictureBox.Image = this.DrawLocal(this.localPictureBox.Width, this.localPictureBox.Height);
            this.worldPictureBox.Image = this.DrawWorld(this.worldPictureBox.Width, this.worldPictureBox.Height);
            this.globalPictureBox.Image = this.DrawGlobalMap(this.globalPictureBox.Width, this.globalPictureBox.Height);
            //this.terrainPictureBox.Image = this.DrawTerrain(this.terrainPictureBox.Width, this.terrainPictureBox.Height);
        }

        private ChunkCoords currentChunk;

        private void splitContainer1_KeyPress(object sender, KeyPressEventArgs e)
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
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, pt * 255 / chunk.HeightMap.Size.maxY, 0)), x * xSize, z * zSize, xSize, zSize);
                }
            }
            return bitmap;
        }

        public Bitmap DrawWorld(int width, int height)
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
                        DrawMiniMap(graphics, chunk, xOri, zOri);
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
        private void DrawMiniMap(Graphics graphics, Chunk chunk, int xOri, int zOri)
        {
            for (int x = 0; x < chunk.ChunkSize; x++)
            {
                for (int z = 0; z < chunk.ChunkSize; z++)
                {
                    var pt = chunk.HeightMap[x + chunk.HeightMap.Size.minX, z + chunk.HeightMap.Size.minZ];
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, pt * 255 / chunk.HeightMap.Size.maxY, 0)), xOri + x, zOri + z, 1, 1);
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
                    //var color = World.IsGlobalMapWater(x, z) ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 0, pt, 0);
                    var color = Color.FromArgb(255, 0, pt, 0);
                    graphics.FillRectangle(new SolidBrush(color), x1, z1, 1,1);
                    if (x1 == currentChunk.X && z1 == currentChunk.Z)
                        graphics.DrawRectangle(new Pen(Color.FromArgb(255, 255,0,0)), x1, z1, 1, 1);
                    z1++;
                }
                x1++;
            }
            return bitmap;
        }

        /*
        private Bitmap DrawTerrain(int width, int height, int zoom=4)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var map = World.GetGlobalMap();
            for (int x = 0; x < map.Size.xHeight; x++)
            {
                for (int z = 0; z < map.Size.zWidth; z++)
                {
                    var pt = map[x, z];
                    var color = World.IsGlobalMapWater(x,z) ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 0, pt, 0);
                    graphics.FillRectangle(new SolidBrush(color), (width/2)+((x-z)*zoom), (height/4)+((x+z))-pt, zoom, pt);
                }
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
