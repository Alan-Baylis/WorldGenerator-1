using System;
using System.Collections.Generic;
using Sean.Shared;

namespace AiClient
{
    public enum JobState
    {
        Free,
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

        protected BaseJob(Character taskOwner = null)
        {
            this.TaskOwner = taskOwner;
        }
        protected Stack<Position> FindPathToNearestItem(Position start, BlockType item)
        {
            return Program.Engine.PathFinder.FindPathToNearestBlock (start, item);
        }

        public void ProcessJob(Character taskOwner = null)
        {
            switch (State)
            {
                case JobState.Free:
                    State = JobState.InProgress;
                    this.TaskOwner = taskOwner;
                    break;
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


    struct RequireItem
    {
        string Item;
        int Count;
    }

    public class BuildFire
    {
        List<RequireItem> Requires { { Wood, 1} }

        bool CanProceed()
        {
        }
    }






    public class FindFood : BaseJob
    {
        public FindFood(Character taskOwner) : base(taskOwner)
        {
        }
        protected override void Process()
        {
            var path = FindPathToNearestItem (TaskOwner.Location, BlockType.Food);
            if (path.Count == 1) {
                State = JobState.Complete;
                WaitingOn = new EatFood(TaskOwner, path.Pop());
                Program.Engine.JobManager.AddJob (WaitingOn);
            }
            WaitingOn = new WalkTo(TaskOwner, path);
            Program.Engine.JobManager.AddJob (WaitingOn);
        }
    }

    public class WalkTo : BaseJob
    {
        public WalkTo(Character taskOwner, Stack<Position> path) : base(taskOwner)
        {
            Path = path;
        }
        protected override void Process()
        {
            if (Path.Count == 0) {
                State = JobState.Complete;
                return;
            }
            var next = Path.Pop();
            TaskOwner.Location = next;
        }
        Stack<Position> Path;
    }

    public class EatFood : BaseJob
    {
        public EatFood(Character taskOwner, Position pos) : base(taskOwner)
        {
            Pos = pos;
        }
        Position Pos;
        protected override void Process()
        {
            Program.Engine.World.SetBlock(Pos, new Block(BlockType.Unknown));
            State = JobState.Complete;
        }
    }
}
