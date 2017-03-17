using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiClient
{
    class Character
    {
        Character()
        {
        }

        public int Id { get;set;}
        public int X { get;set;}
        public int Y { get;set;}
    }

    class Characters
    {
        Dictionary<int, Character> chars = new Dictionary<int, Character>();
        Characters(Map map)
        {
            this.map = map;
        }

        Map map;

        public void AddCharacter(Character chr)
        {
            chars.Add (chr.Id, chr);
            map.Add (chr.X, chr.Y, Item.Character);
        }
    }
}
