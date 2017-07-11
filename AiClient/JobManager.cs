﻿using System;
using System.Collections.Generic;

namespace AiClient
{
    public class JobManager
    {
        private Character Owner;
        private Stack<BaseJob> jobs;

        public JobManager(Character Owner)
        {
            jobs = new Stack<BaseJob>();
        }

        public int JobCount {  get { return jobs.Count; } }

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
            jobs.Push(newJob);
            Program.Engine.WriteLog($">Adding job {newJob}");
        }

        public void ProcessJobs()
        {
            var job = jobs.Peek();
            job.ProcessJob();
            if (job.State == JobState.Complete)
            {
                Program.Engine.WriteLog($"<Completed job {job}");
                jobs.Pop();
            }
        }

    }
}
