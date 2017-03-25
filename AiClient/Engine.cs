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
        private Characters characters;
        private PathFinder pathFinder;

        DisplayConsole console;
        DisplayConsoleWindow gridWindow;

        public Engine()
        {
            world = new World(38);
            characters = new Characters(world);
            pathFinder = new PathFinder (world);

            console = new DisplayConsole();
            gridWindow = console.AddWindow("grid", 2, 2, 40, 40);
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
                gridWindow.Clear ();
                world.Render (gridWindow);
                System.Threading.Thread.Sleep (1000);

                if (chr.WalkPath.Count != 0) {
                    Position newPosition = chr.WalkPath.Dequeue ();
                    world.Move (chr.Location.X, chr.Location.Z, Item.Character, newPosition.X, newPosition.Z);
                    chr.Location = newPosition;
                }
            }
        }
    }
}
