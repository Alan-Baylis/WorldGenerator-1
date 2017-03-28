using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sean.Shared;

namespace AiClient
{
    public class Character
    {
        public Character()
        {
        }

        public int Id { get;set;}

        public Stack<Position> WalkPath { get; set;}
        public Position Location { get; set; }
        public Position Destination { get ; set;}
    }

    public class Characters
    {
        Dictionary<int, Character> chars = new Dictionary<int, Character>();
        public Characters(World map)
        {
            this.map = map;
        }

        World map;

        public void AddCharacter(Character chr)
        {
            chars.Add (chr.Id, chr);
            map.Add (chr.Location.X, chr.Location.Z, Item.Character);
        }
    }
}
