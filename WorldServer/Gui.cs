using Sean.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sean.WorldServer
{
    public class Gui
    {
        public bool IsRunning = true;
        private DisplayConsole console;
        private DisplayConsoleWindow gridWindow;
        private DisplayConsoleWindow chunksWindow;
        private Thread thread;
        private int y = 128;
        private int selectedChunk = 0;
        private int numChunks = 0;
        private int chunkX;
        private int chunkZ;

        public Gui()
        {
            console = new DisplayConsole();
            gridWindow = console.AddWindow("grid", 2, 2, 18, 18);
            chunksWindow = console.AddWindow("chunks", 23, 2, 10, 18);
        }

        public void Run()
        {
            thread = new Thread(new ThreadStart(StartThread));
            thread.Start();
        }

        private void StartThread()
        {
            try
            {
                MainClass.WorldInstance.GetChunk(new ChunkCoords(112,54)); // Get Something
                while (true)
                {
                    Render();
                    var key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.PageUp:
                            y++;
                            break;
                        case ConsoleKey.PageDown:
                            y--;
                            break;
                        case ConsoleKey.DownArrow:
                            selectedChunk = Math.Min(numChunks, ++selectedChunk);
                            break;
                        case ConsoleKey.UpArrow:
                            selectedChunk = Math.Max(1, --selectedChunk);
                            break;
                    case ConsoleKey.Escape:
                            IsRunning = false;
                            return;
                            break;
                    }
                    //System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex) {
                Log.WriteError ($"Gui Thread crashed - {ex.Message}");
            }
        }

        private void Render()
        {
            if (chunkX != 0 && chunkZ != 0)
            {
                var chunk = MainClass.WorldInstance.GetChunk(new ChunkCoords(chunkX, chunkZ));
                //var y = chunk.DeepestTransparentLevel + 5;
                for (var x = 0; x < Global.CHUNK_SIZE; x++)
                {
                    for (var z = 0; z < Global.CHUNK_SIZE; z++)
                    {
                        var block = chunk.Blocks[x, y, z];
                        char c = block.IsSolid ? '#' : ' ';
                        if (block.Type == Block.BlockType.Unknown) c = '.';
                        else if (block.Type == Block.BlockType.Water1) c = '~';
                        else if (block.Type == Block.BlockType.WaterSource) c = '~';
                        else if (block.Type == Block.BlockType.Ocean) c = '~';
                        else if (block.Type == Block.BlockType.Tree) c = '$';
                        else if (block.Type == Block.BlockType.Leaves) c = '%';

                        gridWindow.WriteChar(x, z, c);
                    }
                }
            }
            chunksWindow.Clear();
            numChunks = 0;
            chunkX = 0;
            chunkZ = 0;
            foreach (var chunk in MainClass.WorldInstance.LoadedChunks())
            {
                numChunks++;
                if (numChunks == selectedChunk)
                {
                    chunkX = chunk.X;
                    chunkZ = chunk.Z;
                    chunksWindow.Write(">");
                }
                else
                {
                    chunksWindow.Write(" ");
                }

                chunksWindow.Write(chunk.ToString());
                chunksWindow.WriteNewLine();
            }
        }
    }
}
