using System;
using Sean.Shared;
using System.Collections.Generic;

namespace Sean.GameObjects
{
    public class Item
    {
        public Item ()
        {
        }

        public string Name { get; set; }
        public Coords Coords { get; set; }
        public ItemType ItemType { get; set; }
    }

    public enum ItemType
    {
        Block = 0,
        Food = 1,
        Weapon = 2,
    }

}

