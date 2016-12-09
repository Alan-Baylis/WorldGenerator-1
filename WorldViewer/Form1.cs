using Sean.WorldGenerator;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Sean.Shared;

namespace WorldViewer
{
    public partial class Form1 : Form
    {
        ParametersForm parametersForm;
        ElevationForm elevationForm;
        View selectedView;
        World WorldInstance;

        private enum View
        {
            HeightMap, Temp, Biosphere, Water
        }

        public Form1()
        {
            InitializeComponent();
            currentChunk = new Sean.Shared.ChunkCoords(64, 28);
            textBox1.Text = "Keys: W,A,S,D";
            WorldInstance = new World();
        }

        private void DrawMaps()
        {
            var currentCursor = this.Cursor;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.localPictureBox.Image = this.DrawLocal(this.localPictureBox.Width, this.localPictureBox.Height);
                this.worldPictureBox.Image = this.DrawAllChunks(this.worldPictureBox.Width, this.worldPictureBox.Height);
                this.pictureBox1.Image = this.DrawGlobalMap(this.pictureBox1.Width, this.pictureBox1.Height, selectedView);
                //this.terrainPictureBox.Image = this.DrawTerrain(this.terrainPictureBox.Width, this.terrainPictureBox.Height);

                this.elevationForm.DrawImages(currentChunk);
            }
            catch (Exception e)
            {
                Log.WriteError($"Exception - {e.Message}");
            }
            this.Cursor = currentCursor;
        }

        private void OnGlobalMapMouseDoubleClick(object sender, MouseEventArgs e)
        {
            var map = WorldInstance.GlobalMap;
            currentChunk = new Sean.Shared.ChunkCoords (
                Settings.globalChunkCount * e.X / this.pictureBox1.Width,
                Settings.globalChunkCount * e.Y / this.pictureBox1.Height);
            DrawMaps();
        }

        private Sean.Shared.ChunkCoords currentChunk;
        private const int WaterHeight = Sean.WorldGenerator.Settings.waterLevel;

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            bool update = false;
            switch (e.KeyChar)
            {
                case 'w':
                currentChunk = new Sean.Shared.ChunkCoords(currentChunk.X, currentChunk.Z - 1);
                    update = true;
                    break;
                case 's':
                currentChunk = new Sean.Shared.ChunkCoords(currentChunk.X, currentChunk.Z + 1);
                    update = true;
                    break;
                case 'a':
                currentChunk = new Sean.Shared.ChunkCoords(currentChunk.X - 1, currentChunk.Z);
                    update = true;
                    break;
                case 'd':
                currentChunk = new Sean.Shared.ChunkCoords(currentChunk.X + 1, currentChunk.Z);
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
            var chunk = WorldInstance.GetChunk(currentChunk);
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var avgHeight = 0;
            var xSize = width / chunk.ChunkSize;
            var zSize = height / chunk.ChunkSize;
            for (int x = 0; x < chunk.ChunkSize; x++)
            {
                for (int z = 0; z < chunk.ChunkSize; z++)
                {
                    var pt = chunk.HeightMap[x + chunk.HeightMap.Size.minX, z + chunk.HeightMap.Size.minZ];
                    avgHeight += pt;
                    var color = pt < 20 ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 0, pt, 0);
                    //var color = Color.FromArgb(255, 0, pt, 0);
                    graphics.FillRectangle(new SolidBrush(color), x * xSize, z * zSize, xSize, zSize);
                }
            }
            avgHeight /= (chunk.ChunkSize * chunk.ChunkSize);
            Log.WriteInfo($"Avg Height = {avgHeight}");
            return bitmap;
        }

        public Bitmap DrawAllChunks(int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int xOri = 0;
            for (int x = WorldInstance.MinXChunk; x <= WorldInstance.MaxXChunk; x++)
            {
                int zOri = 0;
                for (int z = WorldInstance.MinZChunk; z <= WorldInstance.MaxZChunk; z++)
                {
                    var coords = new Sean.Shared.ChunkCoords(x, z);
                    if (WorldInstance.IsChunkLoaded(coords))
                    {
                        var chunk = WorldInstance.GetChunk(coords);
                        DrawChunk(graphics, chunk, xOri, zOri);
                    }
                    if (coords == currentChunk)
                    {
                        graphics.DrawRectangle(new Pen(Color.FromArgb(255, 255,0,0)), xOri, zOri, Global.CHUNK_SIZE - 1, Global.CHUNK_SIZE - 1);
                    }
                    zOri = zOri + Global.CHUNK_SIZE;
                }
                xOri = xOri + Global.CHUNK_SIZE;
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
                    var color = pt < WaterHeight ? Color.FromArgb(255, 0, 0, 255) : Color.FromArgb(255, 0, pt, 0);
                    //var color = Color.FromArgb(255, 0, pt, 0);
                    graphics.FillRectangle(new SolidBrush(color), xOri + x, zOri + z, 1, 1);
                }
            }
        }

        private Bitmap DrawGlobalMap(int width, int height, View selectedView)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var map = WorldInstance.GlobalMap;
            var terrain = WorldInstance.GlobalMapTerrain;
            for (int w = 1; w<width; w++)
            {
                for (int h=1;h<height;h++)
                {
                    int x = map.Size.maxX * w / width;
                    int z = map.Size.maxZ * h / height;
                    var pt = map[x,z];
                    var isOcean = terrain[x, z] == 2;
                    Color color;
                    if (isOcean)
                    {
                        color = Color.FromArgb(255, 0, 0, 255);
                    }
                    else
                    {
                        switch (selectedView)
                        {
                            case View.Temp:
                                color = Color.FromArgb(255, 0, WorldInstance.GlobalTemperatureMap[x,z], 0);
                                break;
                            case View.Biosphere:
                                color = Color.FromArgb(255, 0, WorldInstance.GlobalBiosphereMap[x, z], 0);
                                break;
                            default:
                            case View.HeightMap:
                                color = Color.FromArgb(255, 0, pt, 0);
                                break;
                        }
                    }

                    graphics.FillRectangle(new SolidBrush(color), (width * x/map.Size.maxX), (height * z/map.Size.maxZ), Math.Max(width/map.Size.maxX,1),Math.Max(height/map.Size.maxZ,1));
                }
            }
            graphics.DrawRectangle(new Pen(Color.FromArgb(255, 255,0,0)), (width*currentChunk.WorldCoordsX/map.Size.maxX), (height*currentChunk.WorldCoordsZ/map.Size.maxZ), Math.Max(width / map.Size.maxX,1), Math.Max(height / map.Size.maxZ,1));
            return bitmap;
        }

        
        private Bitmap DrawTerrain(int width, int height)
        {
            var boxImage = imageList.Images["box_blue"];
            var chunk = WorldInstance.GetChunk(currentChunk);
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

        private void Form1_Shown(object sender, EventArgs e)
        {
            //parametersForm = new ParametersForm();
            //parametersForm.Show();
            elevationForm = new ElevationForm(WorldInstance);
            elevationForm.Show();
            DrawMaps();
        }

        //private void OnRadioButtonCheckedChanged(object sender, EventArgs e)
        //{
        //    selectedView = View.HeightMap;
        //    if (heightRadioButton.Checked) selectedView = View.HeightMap;
        //    if (tempRadioButton.Checked) selectedView = View.Temp;
        //    if (bioRadioButton.Checked) selectedView = View.Biosphere;
        //    if (waterRadioButton.Checked) selectedView = View.Water;
        //    DrawMaps();
        //}
    }
}
