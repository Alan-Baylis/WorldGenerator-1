using System;
using System.Collections.Generic;
using Sean.Shared;

namespace AiClient
{
    public abstract class BaseJob
    {
        public int Priority;
        public bool Complete;
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
            if (!Complete) Process();
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
                Complete = true;
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
            else if (path.Count == 0)
            {
                TaskOwner.AddJob(new Pickup(TaskOwner, TaskOwner.Location, BlockType.Food));
                Complete = true;
            }
            else if (path.Count == 1)
            {
                TaskOwner.AddJob(new Pickup(TaskOwner, path.Pop(), BlockType.Food));
                Complete = true;
            }
            else
            {
                TaskOwner.AddJob(new WalkTo(TaskOwner, path));
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
           // TODO hmmmm
        }
    }

    public class Pickup : BaseJob
    {
        Position loc;
        BlockType item;
        public Pickup(Character taskOwner, Position loc, BlockType item) : base(taskOwner)
        {
            this.loc = loc;
            this.item = item;
        }
        protected override void Process()
        {
            var here = Program.Engine.World.GetBlock(loc);
            if (here.Type == item)
            {
                Program.Engine.World.SetBlock(loc, new Block(BlockType.Air));
                TaskOwner.AddItem(item);
                Complete = true;
            }
            else
            {
                Complete = true;
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
            if (Path.Count == 1)
            {
                Complete = true;
            }
            else
            {
                var next = Path.Pop();
                TaskOwner.Location = next;
            }
        }
    }
}
