using System;
using System.Collections.Generic;
using Sean.WorldGenerator;
using Sean.Shared;

namespace Sean.WorldServer
{
    public static class WorldEvents
    {
        private static Dictionary<ChunkCoords, List<Guid>> registrations;

        static WorldEvents()
        {
            registrations = new Dictionary<ChunkCoords, List<Guid>>();
        }

        public static void RequestWorldMap(Guid clientId)
        {
            var map = World.GlobalMap;
            MessageProcessor.SendWorldMap(clientId, map);
        }
        public static void ChunkRegister(ChunkCoords chunkCoords, Guid clientId)
        {
            var chunk = World.GetChunk(chunkCoords);
            if (!registrations.ContainsKey (chunkCoords)) {
                registrations [chunkCoords] = new List<Guid> ();
            }
            registrations[chunkCoords].Add(clientId);
            MessageProcessor.SendMap (clientId, chunk);
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

