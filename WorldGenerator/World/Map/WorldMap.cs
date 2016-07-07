using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sean.WorldGenerator
{
    internal class WorldMap
    {
        private Generator generator;
        private Array<int> globalMap;

        public WorldMap(int seed)
        {
            this.generator = new Generator(seed);
            globalMap = generator.GenerateGlobalMap();
        }
    }
}
