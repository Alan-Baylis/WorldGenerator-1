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
        private List<BaseJob> removeJobs;
        private List<BaseJob> addJobs;

        public JobManager()
        {
            jobs = new List<BaseJob>();
            addJobs = new List<BaseJob>();
            removeJobs = new List<BaseJob>();
        }

        public bool HasJob(Type jobType, Character chr)
        {
            foreach (var job in jobs)
            {
                if (job.TaskOwner == chr && job.GetType() == jobType)
                    return true;
            }
            return false;
        }

        public void AddJob(BaseJob newJob)
        {
            addJobs.Add(newJob);
            Program.Engine.WriteLog($"Adding job {newJob}");
        }
        public void RemoveJob(BaseJob job)
        {
            removeJobs.Add(job);
            Program.Engine.WriteLog($"Removing job {job}");
        }

        public void ProcessJobs()
        {
            foreach (var job in jobs) {
                var finished = job.ProcessJob ();
                if (finished) RemoveJob(job);
            }
            foreach (var job in addJobs)
                jobs.Add(job);
            foreach (var job in removeJobs)
                jobs.Remove(job);
        }

    }
}
