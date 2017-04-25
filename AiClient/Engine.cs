using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sean.PathFinding;
using Sean.Shared;

namespace AiClient
{
    public class Engine
    {
        private World world;
        private PathFinder pathFinder;
        private JobManager jobManager;

        private Dictionary<int, Character> chars;
        private DisplayConsole console;
        private DisplayConsoleWindow gridWindow;

        public Engine()
        {
            world = new World(38);
            pathFinder = new PathFinder (world);
            jobManager = new JobManager ();

            chars = new Dictionary<int, Character>();

            console = new DisplayConsole();
            gridWindow = console.AddWindow("grid", 2, 2, 40, 40);
        }

        private void AddCharacter(Character chr)
        {
            chars.Add (chr.Id, chr);
            map.Add (chr.Location.X, chr.Location.Z, Item.Character);
        }

        public void Run()
        {
            var chr = new Character ();
            chr.Id = 1;
            chr.Location = new Position(20,1,20);
            chr.Destination = new Position(30,1,30);

            characters.AddCharacter (chr);
            chr.WalkPath = pathFinder.FindPath (chr.Location, chr.Destination);

            for(int i=0; i<20; i++) {

                ProcessCharacters;
                gridWindow.Clear ();
                world.Render (gridWindow);
                System.Threading.Thread.Sleep (1000);

                if (chr.WalkPath.Count != 0) {
                    Position newPosition = chr.WalkPath.Pop();
                    world.Move (chr.Location.X, chr.Location.Z, Item.Character, newPosition.X, newPosition.Z);
                    chr.Location = newPosition;
                }
            }
        }

        private void ProcessCharacters()
        {
            foreach (var chr in chars.Values) {
                chr.thirst++;
                chr.tiredness++;
                chr.hunger++;

                if (chr.tiredness == 80)
                    jobManager.AddJob(new SleepTask (chr));
                if (chr.hunger == 80)
                    jobManager.AddJob(new EatTask (chr));
                if (chr.thirst == 80)
                    jobManager.AddJob (new DrinkTask (chr));

                if (chr.tiredness > 100) {
                    chr.DoAction (Exhausted);
                    chr.tiredness = 100;
                }
                if (chr.thirst > 100) {
                    chr.DoAction (Dehydrated);
                    chr.thirst = 100;
                }
                if (chr.hunger > 100) {
                    chr.DoAction (Starve);
                    chr.hunger = 100;
                }
            }
        }
    }
}
