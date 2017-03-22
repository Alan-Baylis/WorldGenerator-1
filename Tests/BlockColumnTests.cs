using System;
using Sean.Shared;

namespace Tests
{
    public static class BlocksColumnTests
    {
        public static void Test ()
        {
            var blocks = new BlocksColumn (30);
            Console.WriteLine(blocks);

            AddBlock (blocks, BlockType.Dirt, 5);
            AddBlock (blocks, BlockType.Dirt, 20);
            AddBlock (blocks, BlockType.Dirt, 25);
            AddBlock (blocks, BlockType.Dirt, 15);

            AddBlock (blocks, BlockType.Dirt, 16);
            AddBlock (blocks, BlockType.Dirt, 14);

            AddBlock (blocks, BlockType.Air, 16);
            AddBlock (blocks, BlockType.Air, 14);
            AddBlock (blocks, BlockType.Air, 15);

            AddBlock (blocks, BlockType.Air, 25);
            AddBlock (blocks, BlockType.Air, 20);
            AddBlock (blocks, BlockType.Air, 5);

            AddBlock (blocks, BlockType.Dirt, 0);
            AddBlock (blocks, BlockType.Dirt, 29);
            AddBlock (blocks, BlockType.Air, 0);
            AddBlock (blocks, BlockType.Air, 29);
        }

        private static void AddBlock(BlocksColumn blocks, BlockType type, int position)
        {
            Console.WriteLine ($"Add {type} at {position}");
            blocks [position] = new Block (type);
            Console.WriteLine (blocks);
            Console.WriteLine(blocks.Render());
        }
    }
}

