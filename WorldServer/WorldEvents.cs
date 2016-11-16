using System;
using System.Collections.Generic;
using Sean.WorldGenerator;
using Sean.Shared;

namespace Sean.WorldServer
{
    public static class WorldEvents
    {
        private static Dictionary<ChunkCoords, List<Guid>> registrations;
        private static bool _firstChunk = true;

        static WorldEvents()
        {
            registrations = new Dictionary<ChunkCoords, List<Guid>>();
            MainClass.WorldInstance.WorldEvents += OnGeneratorWorldEvents;
        }

        private static void OnGeneratorWorldEvents(WorldEventArgs e)
        {
            var chunkCoords = new ChunkCoords(e.blockLocation);
            if (!registrations.ContainsKey(chunkCoords))
            {
                return;
            }
            foreach (var client in registrations[chunkCoords])
                MessageProcessor.SendMapUpdate(client, e.blockLocation, e.action, e.block);
        }

        public static void RequestWorldMap(Guid clientId)
        {
            MessageProcessor.SendWorldMap(clientId, Shared.Comms.MapRequestType.HeightMap, MainClass.WorldInstance.GlobalMap);
            MessageProcessor.SendWorldMap(clientId, Shared.Comms.MapRequestType.Terrain, MainClass.WorldInstance.GlobalMapTerrain);
        }
        public static void ChunkRegister(ChunkCoords chunkCoords, Guid clientId)
        {
            var chunk = MainClass.WorldInstance.GetChunk(chunkCoords);
            if (!registrations.ContainsKey (chunkCoords)) {
                registrations [chunkCoords] = new List<Guid> ();
            }
            registrations[chunkCoords].Add(clientId);
            MessageProcessor.SendMap (clientId, chunk);

            if (_firstChunk)
            {
                _firstChunk = false;
                CharacterManager.AddRandomCharacters(new Position(chunkCoords.WorldCoordsX + Global.CHUNK_SIZE / 2, 0, chunkCoords.WorldCoordsZ + Global.CHUNK_SIZE / 2));
            }
        }
        public static void ChunkIgnore(ChunkCoords chunkCoords, Guid clientId)
        {
            var reg = registrations [chunkCoords];
            if (reg != null)
            {
                reg.Remove (clientId);
                if (reg.Count == 0) {
                    registrations.Remove (chunkCoords);
                }
            }
        }
    }
}

