using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiClient
{
    public class Engine
    {
        private Map map = new Map();

        public void Run()
        {
            map.Render();
            System.Threading.Thread.Sleep(1000);
            map.Render();
            System.Threading.Thread.Sleep(1000);
            map.Render();
            System.Threading.Thread.Sleep(1000);

            Console.ReadKey();
        }
    }
}
