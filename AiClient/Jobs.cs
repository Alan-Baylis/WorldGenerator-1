using System;
using System.Collections.Generic;
using Sean.Shared;

namespace AiClient
{
    public enum JobState
    {
        InProgress,
        WaitingOnJob,
        Complete,
        Failed
    }

    public abstract class BaseJob
    {
        public int Priority;
        public JobState State;
        public Character TaskOwner;
        public BaseJob WaitingOn;

        protected BaseJob(Character taskOwner)
        {
            this.TaskOwner = taskOwner;
        }
        protected Stack<Position> FindPathToNearestItem(Position start, BlockType item)
        {
            return Program.Engine.PathFinder.FindPathToNearestBlock (start, item);
        }

        public void ProcessJob()
        {
            switch (State)
            {
                case JobState.InProgress:
                    Process();
                    break;
                case JobState.WaitingOnJob:
                case JobState.Complete:
                case JobState.Failed:
                default:
                    break;
            }
        }

        protected abstract void Process();
    }




    public class EatFood : BaseJob
    {
        public EatFood(Character taskOwner) : base(taskOwner)
        {
        }
        protected override void Process()
        {
            if (TaskOwner.RemoveItem(BlockType.Food))
            {
                Program.Engine.WriteLog($"=Eating food");
                State = JobState.Complete;
            }
            else
                TaskOwner.AddJob(new FindFood(TaskOwner));
        }
    }

    public class FindFood : BaseJob
    {
        public FindFood(Character taskOwner) : base(taskOwner)
        {
        }
        protected override void Process()
        {
            var path = FindPathToNearestItem(TaskOwner.Location, BlockType.Food);
            if (path == null)
            {
                TaskOwner.AddJob(new CantFindFood(TaskOwner));
            }
            else if (path.Count > 0)
            {
                TaskOwner.AddJob(new WalkTo(TaskOwner, path));
            }
            else
            {
                Program.Engine.WriteLog($"=Found food");
                TaskOwner.AddJob(new Pickup(TaskOwner, BlockType.Food));
                State = JobState.Complete;
            }
        }
    }

    public class CantFindFood : BaseJob
    {
        public CantFindFood(Character taskOwner) : base(taskOwner)
        {
        }
        protected override void Process()
        {
           Program.Engine.WriteLog($"=Can't find food");
           // TODO hmmmm
        }
    }

    public class Pickup : BaseJob
    {
        BlockType item;
        public Pickup(Character taskOwner, BlockType item) : base(taskOwner)
        {
            this.item = item;
        }
        protected override void Process()
        {
            var here = Program.Engine.World.GetBlock(TaskOwner.Location);
            if (here.Type == item)
            {
                Program.Engine.WriteLog($"=Picking up item");
                Program.Engine.World.SetBlock(TaskOwner.Location, new Block(BlockType.Air));
                TaskOwner.AddItem(item);
                State = JobState.Complete;
            }
            else
            {
                Program.Engine.WriteLog($"=Can't pickup item as no longer here");
                State = JobState.Complete;
            }
        }
    }

    public class WalkTo : BaseJob
    {
        Stack<Position> Path;
        public WalkTo(Character taskOwner, Stack<Position> path) : base(taskOwner)
        {
            Path = path;
        }
        protected override void Process()
        {
            if (Path.Count == 0)
            {
                Program.Engine.WriteLog($"=Walked to target");
                State = JobState.Complete;
            }
            else
            {
                Program.Engine.WriteLog($"=Walking to target");
                var next = Path.Pop();
                TaskOwner.Location = next;
            }
        }
    }
}
