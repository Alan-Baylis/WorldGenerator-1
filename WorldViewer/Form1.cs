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
            chunk = World.GetChunk(new ChunkCoords(100, 100), 1);
            this.pictureBox.Image = this.Draw(this.pictureBox.Width, this.pictureBox.Height);
        }

        private Chunk chunk;

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public Bitmap Draw(int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var xSize = width / chunk.ChunkSize;
            var zSize = height / chunk.ChunkSize;
            for (int x=0;  x<chunk.ChunkSize; x++)
            {
                for (int z=0; z<chunk.ChunkSize; z++)
                {
                    var pt = chunk.HeightMap[x+chunk.HeightMap.Size.minX, z+chunk.HeightMap.Size.minZ];
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(255,0,pt*255/chunk.HeightMap.Size.maxY,0)), x*xSize, z*zSize, xSize, zSize);
                }
            }

            return bitmap;
        }
    }
}
