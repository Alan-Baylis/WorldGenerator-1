using System;
using Sean.Shared;
using System.Collections.Generic;
using Sean.WorldGenerator;

namespace OpenTkClient
{
    public class ServerLocal : IServer
    {
        private World WorldInstance = new World();

        public void Start()
        {
            Console.WriteLine ("Local Server starting");
        }

        public void GetWorldMap()
        {
            MapManager.SetWorldMapHeight(WorldInstance.GlobalMap);
            MapManager.SetWorldMapTerrain(WorldInstance.GlobalMapTerrain);
        }

        private static List<string> sent = new List<string>(); // TODO - do better
        public void GetMap(int x, int z)
		{
			string hash = $"{x},{z}";
			if (!sent.Contains(hash))
			{
				sent.Add(hash);
                var coords = new ChunkCoords (x,z);
                var chunk = WorldInstance.GetChunk(coords);
                MapManager.AddChunk (coords, chunk);
			}
		}
    }
}
