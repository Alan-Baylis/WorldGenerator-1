using System;
using Sean.Shared;
using System.Collections.Generic;

namespace Sean.GameObjects
{
    public class Unit
    {
        public Unit ()
        {
        }

        public string Name { get; set; }
        public Position Position { get; set; }

        public List<Position> MovingPath { get; set; }
        public Item Carrying { get; set; }
    }
}

