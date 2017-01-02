using Sean.Shared.Comms;
using System;
using System.Threading;
using Sean.Shared;

namespace OpenTkClient
{
    public class ServerLocal : IServer
    {
        private void Start()
        {
            Console.WriteLine ("Local Server starting");
        }

        private void SendGetWorldMap()
        {
            ClientConnection.BroadcastMessage(new Message()
            {
                WorldMapRequest = new WorldMapRequestMessage()
            }
            );
        }

        private void SendGetMap(int x, int z)
		{
			string hash = $"{x},{z}";
			if (!sent.Contains(hash))
			{
				sent.Add(hash);
				ClientConnection.BroadcastMessage(new Message()
					{
						MapRequest = new MapRequestMessage()
						{
							Coords = new ChunkCoords(x,z)
						}
					}
				);
			}
		}

        public void ProcessMessage(Guid clientId, Message msg)
        {
            try
            {
                //Console.WriteLine ($"Processing response... {msg.ToString ()}");
                if (msg.Map != null)
                {
                    var position = msg.Map.MinPosition;
                    var coords = new ChunkCoords (position);
                    var chunk = Sean.Shared.Chunk.Deserialize (coords, msg.Data);
                    MapManager.AddChunk (coords, chunk);
                }
                if (msg.WorldMapResponse != null)
                {
                    var size = new ArraySize()
                    {
                        scale = msg.WorldMapResponse.Scale,
                        minX = msg.WorldMapResponse.MinPosition.X,
                        minY = msg.WorldMapResponse.MinPosition.Y,
                        minZ = msg.WorldMapResponse.MinPosition.Z,
                        maxX = msg.WorldMapResponse.MaxPosition.X,
                        maxY = msg.WorldMapResponse.MaxPosition.Y,
                        maxZ = msg.WorldMapResponse.MaxPosition.Z,
                    };
                    var map = new Array<byte>(size);
                    map.DeSerialize(msg.Data);
                    if (msg.WorldMapResponse.MapRequestType == Sean.Shared.Comms.MapRequestType.HeightMap)
                        MapManager.SetWorldMapHeight(map);
                    else if (msg.WorldMapResponse.MapRequestType == Sean.Shared.Comms.MapRequestType.Terrain)
                        MapManager.SetWorldMapTerrain(map);
                }
                if (msg.MapCharacterUpdate != null)
                {
                    CharacterManager.UpdateLocation (msg.MapCharacterUpdate.CharacterId, msg.MapCharacterUpdate.Position);
                }
                if (msg.MapUpdate != null)
                {
                    MapManager.SetBlock (msg.MapUpdate.Position, msg.MapUpdate.NewBlock);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught in ProcessMessage - {0}", e.ToString());
            }
        }

    }

}
