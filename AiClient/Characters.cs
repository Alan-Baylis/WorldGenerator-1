using Sean.Shared;
using System.Collections.Generic;

namespace AiClient
{
    public class Character
    {
        public Character()
        {
            JobManager = new JobManager(this);
            items = new Dictionary<BlockType, int>();
        }

        public JobManager JobManager { get; private set; }


        public int Id { get;set;}

        public Position Location { get; set; }

        Dictionary<BlockType, int> items;

        public void AddItem(BlockType item)
        {
            if (items.ContainsKey(item))
                items[item]++;
            else
                items[item] = 1;
        }
        public bool RemoveItem(BlockType item, int count = 1)
        {
            if (!items.ContainsKey(item) || items[item] < count)
                return false;
            if (items[item] == count)
                items.Remove(item);
            else
                items[item] -= count;
            return true;
        }

        public void AddJob(BaseJob job)
        {
            JobManager.AddJob(job);
        }

        public void Process()
        {
            if (JobManager.JobCount == 0)
            {
                AddJob(new EatFood());
            }

            JobManager.ProcessJobs();
        }
    }

}
