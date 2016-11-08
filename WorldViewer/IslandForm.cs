using Sean.WorldGenerator;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WorldViewer
{
    public partial class IslandForm : Form
    {
        World WorldInstance;
        int waterLevel = 27;
        uint octaves = 8;
        double freq = 1.2;
        double x = 5.65;
        double z = 2.52;
        double scale = 0.22;

        public IslandForm(World world)
        {
            WorldInstance = world;
            InitializeComponent();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            waterLevel = (int)this.param1.Value;
            octaves = (uint)this.param2.Value;
            freq = (double)this.param3.Value;
            x = (double)this.param4.Value;
            z = (double)this.param5.Value;
            scale = (double)this.param6.Value;
            DrawMaps();
        }

        private void IslandForm_Load(object sender, EventArgs e)
        {
            DrawMaps();
            this.param1.Value = waterLevel;
            this.param2.Value = octaves;
            this.param3.Value = (decimal)freq;
            this.param4.Value = (decimal)x;
            this.param5.Value = (decimal)z;
            this.param6.Value = (decimal)scale;
        }

        private void DrawMaps()
        {
            var currentCursor = this.Cursor;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.pictureBox1.Image = this.DrawIslandMap(this.pictureBox1.Width, this.pictureBox1.Height);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception - {e.Message}");
            }
            this.Cursor = currentCursor;
        }

        private Bitmap DrawIslandMap(int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var map = WorldInstance.IslandMap(octaves, freq, x, z, scale);

            int scrScale = 2;
            int w = 0;
            for (int z = map.Size.minZ; z < map.Size.maxZ; z += map.Size.scale)
            {
                w++;
                if (w >= width) break;
                int h = 0;
                for (int x = map.Size.minX; x < map.Size.maxX; x += map.Size.scale)
                {
                    h++;
                    if (h >= height) break;

                    var pt = map[x, z];
                    var isOcean = pt < waterLevel;
                    Color color;
                    if (isOcean)
                    {
                        color = Color.FromArgb(255, 0, 0, 255);
                    }
                    else
                    {
                        color = Color.FromArgb(255, 0, pt, 0);
                    }

                    graphics.FillRectangle(new SolidBrush(color),  w*scrScale, h*scrScale, scrScale, scrScale);
                }
            }
            return bitmap;
        }

    }
}
