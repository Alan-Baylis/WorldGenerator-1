using Sean.Shared.Comms;
using System;
using System.Net;
using System.Net.Sockets;
using Sean.Shared;
using System.Collections.Generic;

namespace OpenTkClient
{
    public class ServerRemote : IServer
    {
        public void Start()
        {
            Console.WriteLine ("Remote Server");

            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Global.ServerName);
				//IPHostEntry ipHostInfo = Dns.Resolve("zen");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8084);

                TcpClient client = new TcpClient();
                Console.WriteLine ("Connecting...");
                client.Connect(remoteEP);

                var connection = ClientConnection.CreateClientConnection(client, ProcessMessage);
                connection.StartClient();

                ClientConnection.BroadcastMessage(new Message()
                {
                    Ping = new PingMessage()
                    {
                        Message = "Hi"
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught in ServerRemote - {0}", e.ToString());
            }
        }

        public void ClearExistingChunks()
        {
            // NoOp
        }

        public void GetWorldMap()
        {
            ClientConnection.BroadcastMessage(new Message()
            {
                WorldMapRequest = new WorldMapRequestMessage()
            }
            );
        }

        private static List<string> sent = new List<string>(); // TODO - do better
        public void GetMap(int x, int z)
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

        private void ProcessMessage(Guid clientId, Message msg)
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
