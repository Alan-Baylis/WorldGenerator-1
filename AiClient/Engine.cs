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

        public Engine()
        {
            world = new World();
            characters = new Characters(world);
            pathFinder = new PathFinder (world);
        }

        public void Run()
        {
            var chr = new Character ();
            chr.Id = 1;
            chr.Location = new Position(20,0,20);
            chr.Destination = new Position(30,0,30);

            characters.AddCharacter (chr);
            chr.WalkPath = pathFinder.FindPath (chr.Location, chr.Destination);

            world.Render();
            System.Threading.Thread.Sleep(1000);

            if (chr.WalkPath.Count != 0) {
                Position newPosition = chr.WalkPath.Dequeue();
                chr.Location = newPosition;
            }

            world.Render();
            System.Threading.Thread.Sleep(1000);

            Console.ReadKey();
        }
    }
}
