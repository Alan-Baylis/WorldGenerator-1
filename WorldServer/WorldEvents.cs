using System;
using System.Collections.Generic;
using Sean.WorldGenerator;

namespace Sean.WorldServer
{
    public static class WorldEvents
    {
        private static Dictionary<ChunkCoords, List<ClientConnection> > registrations
            = new Dictionary<ChunkCoords, List<ClientConnection> > ();

        public static void ChunkRegister(ChunkCoords chunkCoords, ClientConnection client)
        {
            var chunk = World.GetChunk(chunkCoords);
            if (!registrations.ContainsKey (chunkCoords)) {
                registrations [chunkCoords] = new List<ClientConnection> ();
            }
            registrations[chunkCoords].Add(client);
            client.SendMap (chunk);
        }
        public static void ChunkIgnore(ChunkCoords chunkCoords, ClientConnection client)
        {
            var reg = registrations [chunkCoords];
            if (reg != null)
            {
                reg.Remove (client);
                if (reg.Count == 0) {
                    registrations.Remove (chunkCoords);
                }
            }
        }
    }
}

