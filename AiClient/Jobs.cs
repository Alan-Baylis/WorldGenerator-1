using System;
using System.Collections.Generic;
using Sean.Shared;

namespace AiClient
{
    public abstract class BaseJob
    {
        protected BaseJob(Character taskOwner = null)
        {
            this.TaskOwner = taskOwner;
        }
        protected Stack<Position> FindPathToNearestItem(Position start, BlockType item)
        {
            return Program.Engine.PathFinder.FindPathToNearestBlock (start, item);
        }

        public bool ProcessJob()
        {
            if (WaitingOn != null && !WaitingOn.TaskComplete) return false;
            Process();
            return TaskComplete;
        }

        protected abstract void Process();

        public bool TaskComplete;
        public Character TaskOwner;
        public BaseJob WaitingOn;
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
                TaskComplete = true;
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
                TaskComplete = true;
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
            TaskComplete = true;
        }
    }
}
