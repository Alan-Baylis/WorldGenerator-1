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
        private Array<int> waterMap;

        private const int oceanLevel = 40;

        public WorldMap(int seed)
        {
            this.generator = new Generator(seed);
        }

        public void Generate()
        { 
            globalMap = generator.GenerateGlobalMap();
            waterMap = new Array<int>(globalMap.Size);

            /*
            // Springs
            for (int i=0; i<1; i++)
            {
                var x = rnd.Next(globalMap.Size.minX, globalMap.Size.maxX);
                var z = rnd.Next(globalMap.Size.minZ, globalMap.Size.maxZ);
                RecurseWaterFlow(x, z, globalMap[x,z]);
            }
            */
        }
        /*
        private void RecurseWaterFlow(int x,int z,int h)
        {
            if (h <= oceanLevel) return; // Hit the ocean or another river
            waterMap[x, z] = h;
            switch (rnd.Next(1, 3))
            { 
            case 1:
                if (globalMap[x - 1, z] < h)
                    RecurseWaterFlow(x - 1, z, globalMap[x - 1, z]);
                else if (globalMap[x + 1, z] < h)
                    RecurseWaterFlow(x + 1, z, globalMap[x + 1, z]);
                else if (globalMap[x, z - 1] < h)
                    RecurseWaterFlow(x, z - 1, globalMap[x, z - 1]);
                else if (globalMap[x, z + 1] < h)
                    RecurseWaterFlow(x, z + 1, globalMap[x, z + 1]);
                else
                {
                    // Stuck, build lake
                    RecurseWaterFlow(x, z, h + 1);
                }
                break;
            case 2:
                if (globalMap[x, z + 1] < h)
                    RecurseWaterFlow(x, z + 1, globalMap[x, z + 1]);
                else if (globalMap[x, z - 1] < h)
                    RecurseWaterFlow(x, z - 1, globalMap[x, z - 1]);
                else if (globalMap[x + 1, z] < h)
                    RecurseWaterFlow(x + 1, z, globalMap[x + 1, z]);
                else if (globalMap[x - 1, z] < h)
                    RecurseWaterFlow(x - 1, z, globalMap[x - 1, z]);
                else
                {
                    // Stuck, build lake
                    RecurseWaterFlow(x, z, h + 1);
                }
                break;
            }
        }
        */
        public Array<int> GetMap()
        {
            return globalMap;
        }

    }
}
