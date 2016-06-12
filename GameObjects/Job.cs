using System;

namespace Sean.GameObjects
{
    public class Job
    {
        public Job ()
        {
        }

        public JobType JobType { get; set; }
    }

    public enum JobType
    {
        Rest = 0,
        MoveTo = 1,
        PickUp = 2,
        Drop = 3,
        UseItem = 4,
    }
}

