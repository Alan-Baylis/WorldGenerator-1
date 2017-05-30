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
        public Dictionary<int, Character> Characters { get; private set; }

        private DisplayConsole console;
        private DisplayConsoleWindow gridWindow;
        private DisplayConsoleWindow logWindow;

        public Engine()
        {
            World = new World(38);
            PathFinder = new PathFinder (World);
            JobManager = new JobManager ();

            Characters = new Dictionary<int, Character>();

            console = new DisplayConsole();
            gridWindow = console.AddWindow("grid", 2, 2, 40, 40);
            logWindow = console.AddWindow("log", 44, 2, 30, 40);
        }

        public void WriteLog(string message)
        {
            logWindow.WriteLine(message);
        }

        public void Run()
        {
            var chr = new Character ();
            chr.Id = 1;
            chr.Location = new Position(20,1,20);

            Characters.Add (chr.Id, chr);
            World.SetBlock (chr.Location, new Block (BlockType.Character));


            for(int i=0; i<20; i++) {
                JobManager.ProcessJobs ();
                ProcessCharacters();
                gridWindow.Clear ();
                World.Render (gridWindow);
                System.Threading.Thread.Sleep (1000);
            }
        }

        public bool CharacterAt(int x,int z)
        {
            foreach (var chr in Characters.Values)
            {
                if (chr.Location.X == x && chr.Location.Z == z)
                    return true;
            }
            return false;
        }
        private void ProcessCharacters()
        {
            foreach (var chr in Characters.Values) {
                chr.thirst++;
                chr.tiredness++;
                chr.hunger+=20;

                if (chr.hunger > 100 && !JobManager.HasJob(typeof(FindFood), chr)) {
                    chr.hunger = 100;
                    JobManager.AddJob (new FindFood (chr));
                }
            }
        }
    }
}
