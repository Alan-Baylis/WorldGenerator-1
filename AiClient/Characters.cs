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

        public int health { get; set; } = 100;
        public int tiredness { get; set; }
        public int hunger { get; set; }
        public int thirst { get; set; }


        public int Id { get;set;}

        public Stack<Position> WalkPath { get; set;}
        public Position Location { get; set; }
        public Position Destination { get ; set;}

    }

}
