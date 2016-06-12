using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add these usings:
using System.Web.Http;
using System.Net.Http;

namespace Sean.WorldServer
{
    public class WorldController : ApiController
    {
        public HttpResponseMessage LookingAt(int playerId, int x, int z)
        {
            Console.WriteLine ("Player {0} at {1},{2}", playerId, x,z);
            return Ok ();
        }

        public int GetObjectId(int playerId, int x,int y,int z)
        {
            return 0;
        }

        public HttpResponseMessage QueryObject(int playerId, int objectId)
        {
            return Ok ();
        }

        public HttpResponseMessage DoAction(int playerId, int objectId, Action action)
        {
        }

        /*
        public IHttpActionResult Post(Company company)
        {
            return Ok();
        }
        */
    }
}