using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiClient
{
    public class JobManager
    {
        private List<BaseJob> jobs;

        public JobManager()
        {
            jobs = new List<BaseJob>();
        }

        public void AddJob(BaseJob newJob)
        {
            jobs.Add(newJob);
        }

        public void ProcessJobs()
        {
            foreach (var job in jobs) {
                var finished = job.ProcessJob ();
                if (finished) jobs.Remove (job);
            }
        }

    }
}
