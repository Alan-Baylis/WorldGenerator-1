using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace CmdLineClient
{
    public struct Location
    {
        public Location(int x,int z,int y) {
            X = x; Y = y; Z = z;
        }
        public int X;
        public int Y;
        public int Z;
    }

    public struct Block
    {
        public Block(int blockType) {
            BlockType = blockType;
        }
        public int BlockType;
    }

    public class WorldClient
    {
        string _hostUri;
        int _playerId;
        public WorldClient(string hostUri, int playerId)
        {
            _hostUri = hostUri;
            _playerId = playerId;
        }

        public HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(new Uri(_hostUri), "/World/");
            return client;
        }

        public bool LookingAt(Location loc)
        {
            HttpResponseMessage response;
            using (var client = CreateClient())
            {
                response = client.GetAsync($"{client.BaseAddress}/{_playerId}/{loc.X}/{loc.Z}/{loc.Y}").Result;
            }
            var result = response.Content.ReadAsAsync<bool>().Result;
            return result;
        }

        public HttpStatusCode AddBlock(Location loc, Block block)
        {
            HttpResponseMessage response;
            using (var client = CreateClient())
            {
                response = client.PostAsJsonAsync($"{client.BaseAddress}/{_playerId}/{loc.X}/{loc.Z}/{loc.Y}", block).Result;
            }
            return response.StatusCode;
        }
            
        public HttpStatusCode RemoveBlock(Location loc)
        {
            HttpResponseMessage response;
            using (var client = CreateClient())
            {
                response = client.DeleteAsync($"{client.BaseAddress}/{_playerId}/{loc.X}/{loc.Z}/{loc.Y}").Result;
            }
            return response.StatusCode;
        }
    }
}
