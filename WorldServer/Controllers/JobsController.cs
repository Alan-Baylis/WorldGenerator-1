using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add these usings:
using System.Web.Http;
using System.Net.Http;
using Sean.GameObjects;

namespace Sean.WorldServer
{
    public class JobsController : ApiController
    {
        [HttpPost]
        [Route("Player/{playerId}/Jobs")]
        public async Task<int> AddJob(int playerId, int objectId, Job job)
        {
            return 1;
        }

        [HttpDelete]
        [Route("Player/{playerId}/Jobs/{jobId}")]
        public async Task<bool> DeleteJob(int playerId, int objectId, int jobId)
        {
            return true;
        }

        [HttpGet]
        [Route("Player/{playerId}/Jobs/{jobId}")]
        public async Task<Job> GetJob(int playerId, int objectId, int jobId)
        {
            return new Job ();
        }

        [HttpGet]
        [Route("Player/{playerId}/Jobs")]
        public async Task<List<Job>> GetJobs(int playerId, [FromUri] int objectId)
        {
            return new Job ();
        }

    }
}