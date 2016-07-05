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
            Refresh();
        }

        private void Refresh()
        {
            this.localPictureBox.Image = this.DrawLocal(this.localPictureBox.Width, this.localPictureBox.Height);
            this.worldPictureBox.Image = this.DrawWorld(this.worldPictureBox.Width, this.worldPictureBox.Height);
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
                Refresh();
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

            var xSize = width / (World.MaxXChunk - World.MinXChunk + 1);
            var zSize = height / (World.MaxZChunk - World.MinZChunk + 1);
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
                        graphics.DrawRectangle(new Pen(Color.FromArgb(255, 255,0,0)), xOri, zOri, World.ChunkSize, World.ChunkSize);
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

        private void Form1_Resize(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
