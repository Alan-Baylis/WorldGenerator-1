using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sean.Shared;

namespace AiClient
{
    public class Character
    {
        public Character()
        {
            JobManager = new JobManager(this);
        }

        public JobManager JobManager { get; private set; }

        public int health { get; set; } = 100;
        public int tiredness { get; set; }
        public int hunger { get; set; }
        public int thirst { get; set; }


        public int Id { get;set;}

        public Position Location { get; set; }


        public void Process()
        {
            thirst++;
            tiredness++;
            hunger += 20;

            if (hunger > 100 && !JobManager.HasJob(typeof(FindFood), this))
            {
                hunger = 100;
                JobManager.AddJob(new FindFood(this));
            }

            JobManager.ProcessJobs();
        }
    }

}
