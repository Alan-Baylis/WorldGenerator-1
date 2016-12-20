using System;
using Sean.Shared;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sean.WorldGenerator
{
    internal class WorldMap
    {
        private Generator generator;
        private const int oceanLevel = 40;
        private const string fileName = "global.map";

        public Array<byte> GlobalMap { get; private set; }
        public Array<byte> GlobalMapTerrain { get; private set; }
        public Array<byte> TemperatureMap { get; private set; }
        public Array<byte> BiosphereMap { get; private set; }

        public WorldMap(IWorld world, Generator generator)
        {
            this.generator = generator;
            Load ();
            if (GlobalMap == null) {
                Generate ();
                Save ();
            }
        }
        private void Generate()
        {
            Log.WriteInfo ($"[WorldMap.Generate] Generating...");
            GlobalMap = generator.GenerateGlobalMap();
            GlobalMapTerrain = DefineTerrain(GlobalMap);
            TemperatureMap = DefineTemperature(GlobalMap);
            BiosphereMap = DefineBiosphere(GlobalMap, TemperatureMap);
        }

        private void Save()
        {
            try
            {
                Log.WriteInfo ($"[WorldMap.Saving] Saving '{fileName}'...");
                using (FileStream stream = File.Create(fileName))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize( stream, GlobalMap );
                    formatter.Serialize( stream, GlobalMapTerrain );
                    formatter.Serialize( stream, TemperatureMap );
                    formatter.Serialize( stream, BiosphereMap );
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                Log.WriteError ($"[WorldMap.Save] Failed - {e.Message}");
            }
        }

        private void Load()
        {
            try
            {
                if (File.Exists(fileName))
                {
                    Log.WriteInfo ($"[WorldMap.Load] Loading '{fileName}'...");
                    using (var stream = File.OpenRead(fileName))
                    {
                        var formatter = new BinaryFormatter();
                        GlobalMap = (Array<byte>)formatter.Deserialize( stream );
                        GlobalMapTerrain = (Array<byte>)formatter.Deserialize( stream );
                        TemperatureMap = (Array<byte>)formatter.Deserialize( stream );
                        BiosphereMap = (Array<byte>)formatter.Deserialize( stream );
                        stream.Close();
                    }
                    Log.WriteInfo ($"[WorldMap.Load] Loaded '{fileName}'");
                }
            }
            catch (Exception e)
            {
                Log.WriteError ($"[WorldMap.Load] Failed - {e.Message}");
                GlobalMap = null;
                File.Delete (fileName);
            }
        }

        // TODO - stick these somewhere
        private const int WATER = 2;
        private const int GRASS = 4; 
        private Array<byte> DefineTerrain(Array<byte> globalMap)
        {
            var terrain = new Array<byte>(globalMap.Size);
            for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
            {
                for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
                {
                    terrain[x, z] = GRASS;
                }
            }
            UniqueQueue<ChunkCoords> checkChunks = new UniqueQueue<ChunkCoords>();
            for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
            {
                if (globalMap[globalMap.Size.minX, z] < Settings.waterLevel)
                    checkChunks.Enqueue(new ChunkCoords(globalMap.Size.minX, z));
                if (globalMap[globalMap.Size.maxX-1, z] < Settings.waterLevel)
                    checkChunks.Enqueue(new ChunkCoords(globalMap.Size.maxX-1, z));
            }
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                if (globalMap[x, globalMap.Size.minZ] < Settings.waterLevel)
                    checkChunks.Enqueue(new ChunkCoords(x, globalMap.Size.minZ));
                if (globalMap[x, globalMap.Size.maxZ-1] < Settings.waterLevel)
                    checkChunks.Enqueue(new ChunkCoords(x, globalMap.Size.maxZ-1));
            }
            while (checkChunks.Count > 0)
            {
                var pos = checkChunks.Dequeue();
                if (globalMap.IsValidCoord(pos.X, pos.Z) && terrain[pos.X, pos.Z] != WATER && globalMap[pos.X, pos.Z] < Settings.waterLevel)
                {
                    terrain[pos.X, pos.Z] = WATER;
                    checkChunks.Enqueue(new ChunkCoords(pos.X + globalMap.Size.scale, pos.Z));
                    checkChunks.Enqueue(new ChunkCoords(pos.X - globalMap.Size.scale, pos.Z));
                    checkChunks.Enqueue(new ChunkCoords(pos.X, pos.Z + globalMap.Size.scale));
                    checkChunks.Enqueue(new ChunkCoords(pos.X, pos.Z - globalMap.Size.scale));
                }
            }

            return terrain;
        }
        private Array<byte> DefineTemperature(Array<byte> globalMap)
        {
            var temp = new Array<byte>(globalMap.Size);
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
                {
                    var h = (globalMap.Size.maxY - globalMap[x, z]) * 50 / globalMap.Size.maxY;
                    var l = z * 50 / globalMap.Size.maxZ;
                    temp[x, z] = (byte)((h + l) / 2);
                }
            }
            return temp;
        }
        private Array<byte> DefineBiosphere(Array<byte> globalMap, Array<byte> temperatureMap)
        {
            var bio = new Array<byte>(globalMap.Size);
            for (int x = globalMap.Size.minX; x < globalMap.Size.maxX; x += globalMap.Size.scale)
            {
                for (int z = globalMap.Size.minZ; z < globalMap.Size.maxZ; z += globalMap.Size.scale)
                {
                    bio[x,z] = (byte)(generator.CalcGlobalBiosphere(x, z) * 255);
                }
            }
            return bio;
        }
    }
}
