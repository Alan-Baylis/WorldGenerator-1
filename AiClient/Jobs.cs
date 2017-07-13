using System;
using System.Collections.Generic;
using Sean.Shared;

namespace AiClient
{
    public abstract class BaseJob
    {
        public bool Complete;

        public void ProcessJob(Character character)
        {
            if (!Complete) Process(character);
        }

        protected abstract void Process(Character character);
    }




    public class EatFood : BaseJob
    {
        public EatFood() { }
        protected override void Process(Character character)
        {
            if (character.RemoveItem(BlockType.Food))
            {
                Complete = true;
            }
            else
                character.AddJob(new FindItem(BlockType.Food));
        }
    }

    public class FindItem : BaseJob
    {
        public FindItem(BlockType item) { this.item = item; }
        BlockType item;
        protected override void Process(Character character)
        {
            var path = Program.Engine.PathFinder.FindPathToNearestBlock(character.Location, item);
            if (path == null)
            {
                character.AddJob(new CantFindItem());
            }
            else if (path.Count == 0)
            {
                character.AddJob(new Pickup(character.Location, item));
                Complete = true;
            }
            else if (path.Count == 1)
            {
                character.AddJob(new Pickup(path.Pop(), item));
                Complete = true;
            }
            else
            {
                character.AddJob(new WalkTo(path));
            }
        }
    }

    public class CantFindItem : BaseJob
    {
        public CantFindItem() { }
        protected override void Process(Character character)
        {
           // TODO hmmmm
        }
    }

    public class Pickup : BaseJob
    {
        Position loc;
        BlockType item;
        public Pickup(Position loc, BlockType item)
        {
            this.loc = loc;
            this.item = item;
        }
        protected override void Process(Character character)
        {
            var here = Program.Engine.World.GetBlock(loc);
            if (here.Type == item)
            {
                Program.Engine.World.SetBlock(loc, new Block(BlockType.Air));
                character.AddItem(item);
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
        public WalkTo(Stack<Position> path)
        {
            Path = path;
        }
        protected override void Process(Character character)
        {
            if (Path.Count == 1)
            {
                Complete = true;
            }
            else
            {
                var next = Path.Pop();
                character.Location = next;
            }
        }
    }
}
