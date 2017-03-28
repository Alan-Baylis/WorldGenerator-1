using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiClient
{

    public class JobManager
    {
        private List<IJob> jobs;

        JobManager()
        {
            jobs = new List<IJob>();
        }

        public void AddJob(IJob newJob)
        {
            jobs.Add(newJob);
        }

    }
}
