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
            Process();
            return TaskComplete;
        }

        protected abstract void Process();

        public bool TaskComplete;
        protected Character TaskOwner;
    }

    public class FindFood : BaseJob
    {
        public FindFood(Character taskOwner) : base(taskOwner)
        {
            Log.WriteInfo("Adding FindFood job");
        }
        protected override void Process()
        {
            var path = FindPathToNearestItem (TaskOwner.Location, BlockType.Food);
            if (path.Count == 1) {
                TaskComplete = true;
            }
            Program.Engine.JobManager.AddJob (new WalkTo (TaskOwner, path));
        }
    }

    public class WalkTo : BaseJob
    {
        public WalkTo(Character taskOwner, Stack<Position> path) : base(taskOwner)
        {
            Log.WriteInfo("Adding WalkTo job");
            Path = path;
        }
        protected override void Process()
        {
            if (Path.Count == 0) {
                TaskComplete = true;
                return;
            }
            var next = Path.Peek ();
            if (Program.Engine.World.IsLocationSolid (next)) {
                Program.Engine.World.SetBlock (next, new Block (BlockType.Character));
                Program.Engine.World.SetBlock (TaskOwner.Location, new Block (BlockType.Air));
                TaskOwner.Location = next;
            }
            Path.Pop ();
        }
        Stack<Position> Path;
    }

}
