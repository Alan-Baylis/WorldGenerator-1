using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CmdLineClient
{
    public class WorldClient
    {
        string _hostUri;
        public WorldClient(string hostUri)
        {
            _hostUri = hostUri;
        }

        public HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(new Uri(_hostUri), "/World/");
            return client;
        }

        public bool LookingAt(int playerId, int x, int z)
        {
            HttpResponseMessage response;
            using (var client = CreateClient())
            {
                response = client.GetAsync(
                    new Uri(client.BaseAddress, playerId.ToString())).Result;
            }
            var result = response.Content.ReadAsAsync<bool>().Result;
            return result;
        }

        /*
        public Company GetCompany(int id)
        {
            HttpResponseMessage response;
            using (var client = CreateClient())
            {
                response = client.GetAsync(
                    new Uri(client.BaseAddress, id.ToString())).Result;
            }
            var result = response.Content.ReadAsAsync<Company>().Result;
            return result;
        }
        */

        /*
        public System.Net.HttpStatusCode AddCompany(Company company)
        {
            HttpResponseMessage response;
            using (var client = CreateClient())
            {
                response = client.PostAsJsonAsync(client.BaseAddress, company).Result;
            }
            return response.StatusCode;
        }
        */
    }
}
