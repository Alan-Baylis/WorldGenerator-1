using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sean.Shared;

namespace Sean.WorldGenerator
{
    internal class WorldMap
    {
        private Generator generator;
        private const int oceanLevel = 40;

        public Array<int> IslandMap { get; private set; }
        public Array<int> GlobalMap { get; private set; }
        public Array<bool> OceanMap { get; private set; }
        public Array<int> TemperatureMap { get; private set; }
        public Array<int> BiosphereMap { get; private set; }

        public WorldMap(IWorld world, int seed)
        {
            this.generator = new Generator(world, seed);
            Generate();
        }

        public void Generate()
        {
            GlobalMap = generator.GenerateGlobalMap();

            OceanMap = DefineOcean(GlobalMap);
            TemperatureMap = DefineTemperature(GlobalMap);
            BiosphereMap = DefineBiosphere(GlobalMap, TemperatureMap);
        }

        public Array<int> GenerateIslandMap(uint octaves, double freq, double x, double z, double scale)
        {
            IslandMap = generator.GenerateIslandMap(octaves, freq, x, z, scale);
            return IslandMap;
        }

        private Array<bool> DefineOcean(Array<int> globalMap)
        {
            var isOcean = new Array<bool>(globalMap.Size);
            for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
            {
                if (globalMap[globalMap.Size.minX, z] < Settings.waterLevel)
                    ExpandOcean(globalMap, ref isOcean, globalMap.Size.minX, z);
                if (globalMap[globalMap.Size.maxX-1, z] < Settings.waterLevel)
                    ExpandOcean(globalMap, ref isOcean, globalMap.Size.maxX-1, z);
            }
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                if (globalMap[x, globalMap.Size.minZ] < Settings.waterLevel)
                    ExpandOcean(globalMap, ref isOcean, x, globalMap.Size.minZ);
                if (globalMap[x, globalMap.Size.maxZ-1] < Settings.waterLevel)
                    ExpandOcean(globalMap, ref isOcean, x, globalMap.Size.maxZ-1);
            }
            return isOcean;
        }
        private void ExpandOcean(Array<int> globalMap, ref Array<bool> isOcean, int x, int z)
        {
            if (globalMap.IsValidCoord(x, z) && isOcean[x, z] == false && globalMap[x, z] < Settings.waterLevel)
            {
                isOcean[x, z] = true;
                ExpandOcean(globalMap, ref isOcean, x + globalMap.Size.scale, z);
                ExpandOcean(globalMap, ref isOcean, x - globalMap.Size.scale, z);
                ExpandOcean(globalMap, ref isOcean, x, z + globalMap.Size.scale);
                ExpandOcean(globalMap, ref isOcean, x, z - globalMap.Size.scale);
            }
        }
        private Array<int> DefineTemperature(Array<int> globalMap)
        {
            var temp = new Array<int>(globalMap.Size);
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
                {
                    var h = (globalMap.Size.maxY - globalMap[x, z]) * 50 / globalMap.Size.maxY;
                    var l = z * 50 / globalMap.Size.maxZ;
                    temp[x, z] = (h + l) / 2;
                }
            }
            return temp;
        }
        private Array<int> DefineBiosphere(Array<int> globalMap, Array<int> temperatureMap)
        {
            var bio = new Array<int>(globalMap.Size);
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
                {
                    bio[x,z] = (int)(generator.CalcGlobalBiosphere(x, z) * 255);
                }
            }
            return bio;
        }
    }
}
