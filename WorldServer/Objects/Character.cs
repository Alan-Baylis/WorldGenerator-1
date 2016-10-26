using Sean.Shared;
using System;
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
    }
}

