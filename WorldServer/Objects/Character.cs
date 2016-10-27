using Sean.Shared;
using System;
using System.Collections.Generic;

namespace Sean.WorldServer
{
    public class Character
    {
        public Character ()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Position Location { get; set; }
        public Position Destination { get; set; }
        public Queue<Position> WalkPath { get; set; }
    }
}

