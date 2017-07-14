using Sean.Shared;
using System;
using System.Collections.Generic;

namespace Sean.Characters
{
    public class JobManager
    {
        private Character Owner;
        private Stack<BaseJob> jobs;

        public JobManager(Character Owner)
        {
            this.Owner = Owner;
            jobs = new Stack<BaseJob>();
        }

        public int JobCount {  get { return jobs.Count; } }

        public void AddJob(BaseJob newJob)
        {
            jobs.Push(newJob);
            Log.WriteInfo($">Adding job {newJob}");
        }


        public void ProcessJobs()
        {
            while (jobs.Count > 0)
            { 
                var job = jobs.Peek();
                if (job.Complete)
                {
                    jobs.Pop();
                }
                else
                {
                    job.ProcessJob(Owner);
                    return;
                }
            }
        }

    }
}
