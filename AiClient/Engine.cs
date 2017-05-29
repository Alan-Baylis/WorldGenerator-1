using System.Collections.Generic;
using Sean.PathFinding;
using Sean.Shared;

namespace AiClient
{
    public class Engine
    {
        public World World { get; private set; }
        public PathFinder PathFinder { get; private set; }
        public JobManager JobManager { get; private set; }

        private Dictionary<int, Character> chars;
        private DisplayConsole console;
        private DisplayConsoleWindow gridWindow;

        public Engine()
        {
            World = new World(38);
            PathFinder = new PathFinder (World);
            JobManager = new JobManager ();

            chars = new Dictionary<int, Character>();

            console = new DisplayConsole();
            gridWindow = console.AddWindow("grid", 2, 2, 40, 40);
        }


        public void Run()
        {
            var chr = new Character ();
            chr.Id = 1;
            chr.Location = new Position(20,1,20);

            chars.Add (chr.Id, chr);
            World.SetBlock (chr.Location, new Block (BlockType.Character));


            for(int i=0; i<20; i++) {
                JobManager.ProcessJobs ();
                ProcessCharacters();
                gridWindow.Clear ();
                World.Render (gridWindow);
                System.Threading.Thread.Sleep (1000);
            }
        }

        private void ProcessCharacters()
        {
            foreach (var chr in chars.Values) {
                chr.thirst++;
                chr.tiredness++;
                chr.hunger+=20;

                if (chr.hunger > 100) {
                    chr.hunger = 100;
                    JobManager.AddJob (new FindFood (chr));
                }
            }
        }
    }
}
