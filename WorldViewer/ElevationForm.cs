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
using Sean.Shared;

namespace WorldViewer
{
    public partial class ElevationForm : Form
    {
        IWorld WorldInstance;

        public ElevationForm(IWorld world)
        {
            WorldInstance = world;
            InitializeComponent();
        }

        public void DrawImages(ChunkCoords currentChunk)
        {
            this.zPictureBox.Image = DrawTerrainZ(currentChunk, this.zPictureBox.Width, this.zPictureBox.Height);
            this.xPictureBox.Image = DrawTerrainX(currentChunk, this.xPictureBox.Width, this.xPictureBox.Height);
        }


        private Bitmap DrawTerrainZ(ChunkCoords currentChunk, int width, int height)
        {
            var chunk = WorldInstance.GetChunk(currentChunk);
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var xSize = width / chunk.ChunkSize;
            var zSize = height / chunk.ChunkSize;
            int x = chunk.ChunkSize / 2;
            for (int z = 0; z < chunk.ChunkSize; z++)
            {
                for (int y = 0; y < 128; y++)
                {
                    if (chunk.Blocks[x, y, z].IsSolid)
                    {
                        var color = Color.FromArgb(255, 0, 255, 0);
                        graphics.FillRectangle(new SolidBrush(color), width * z / chunk.ChunkSize, height -( height * y / 128), width / chunk.ChunkSize, height / 128);
                    }
                }
            }
            return bitmap;
        }

        private Bitmap DrawTerrainX(ChunkCoords currentChunk, int width, int height)
        {
            var chunk = WorldInstance.GetChunk(currentChunk);
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var xSize = width / chunk.ChunkSize;
            var zSize = height / chunk.ChunkSize;
            int z = chunk.ChunkSize / 2;
            for (int x = 0; x < chunk.ChunkSize; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    if (chunk.Blocks[x, y, z].IsSolid)
                    {
                        var color = Color.FromArgb(255, 0, 255, 0);
                        graphics.FillRectangle(new SolidBrush(color), width * x / chunk.ChunkSize, height - (height * y / 128), width / chunk.ChunkSize, height / 128);
                    }
                }
            }
            return bitmap;
        }
    }
}
