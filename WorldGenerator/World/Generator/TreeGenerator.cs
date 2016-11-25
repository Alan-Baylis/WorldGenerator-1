﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sean.Shared;

namespace Sean.WorldGenerator
{
	public static class TreeGenerator
	{
		private const byte MIN_TREES_PER_CHUNK = 2;
		private const byte MAX_TREES_PER_CHUNK = 5;
		private const byte MIN_TRUNK_HEIGHT = 7;
		private const byte MAX_TRUNK_HEIGHT = 9;
		private const byte DISTANCE_TOLERANCE = 4;

		public static void Generate(IWorld world, Chunk chunk)
        {
            Log.WriteInfo("Generating Trees");
            var takenPositions = new List <Position>();
			int numberOfTreesToGenerate = Settings.Random.Next(MIN_TREES_PER_CHUNK, MAX_TREES_PER_CHUNK + 1);
			for (int tree = 0; tree < numberOfTreesToGenerate; tree++)
			{
				//returns number avoiding upper chunk boundaries ensuring cross chunk placements dont touch each other
                int xProposedInChunk = Settings.Random.Next(0, Global.CHUNK_SIZE - 1);
                int zProposedInChunk = Settings.Random.Next(0, Global.CHUNK_SIZE - 1);
				int yProposed = chunk.HeightMap[xProposedInChunk, zProposedInChunk];

				var block = chunk.Blocks[xProposedInChunk, yProposed, zProposedInChunk];
				if (block.Type != Block.BlockType.Grass && block.Type != Block.BlockType.Snow) continue;
				int xProposedInWorld = chunk.ChunkCoords.WorldCoordsX + xProposedInChunk;
				int zProposedInWorld = chunk.ChunkCoords.WorldCoordsZ + zProposedInChunk;

				//ensure tree is not placed too close to another taken coord, otherwise skip it
				if (IsPositionTaken(takenPositions, xProposedInWorld, zProposedInWorld, DISTANCE_TOLERANCE)) continue;

				//generate a tree
				takenPositions.Add(new Position(xProposedInWorld, yProposed, zProposedInWorld));

				//create the tree blocks
				bool isElmTree = Settings.Random.Next(0, 6) == 0;
				int treeHeight = Settings.Random.Next(MIN_TRUNK_HEIGHT, MAX_TRUNK_HEIGHT + 1); //possible heights 7,8,9
				//int trunkHeight = treeHeight - 2; //top 2 levels get leaves, so actual trunks can be 5-7
				double leafRadius = Settings.Random.NextDouble() + 1.9 + ((treeHeight - MIN_TRUNK_HEIGHT) * 0.2); //will return 1.9-3.3 (influences taller trees to get a larger leaf radius)
				for (int yTrunkLevel = 1; yTrunkLevel <= treeHeight + 1; yTrunkLevel++)
				{
					var trunkPosition = new Position(xProposedInWorld, yProposed + yTrunkLevel, zProposedInWorld);
					if (yTrunkLevel < treeHeight) //place the trunk
					{
                        chunk.Blocks[trunkPosition] = new Block(Block.BlockType.Tree);// isElmTree ? Block.BlockType.ElmTree : Block.BlockType.Tree);
					}
					else //place leaves on the top 2 blocks of the trunk instead of more trunk pieces
					{
                        chunk.Blocks[trunkPosition] = new Block(Block.BlockType.Leaves);
                            //new Block(world.WorldType == WorldType.Winter ? Block.BlockType.SnowLeaves : Block.BlockType.Leaves);
                    }

					//place leaves at this trunk level
					if (yTrunkLevel < 3) continue;
					for (int leafX = -3; leafX <= 3; leafX++)
					{
						for (int leafZ = -3; leafZ <= 3; leafZ++)
						{
							if (leafX == 0 && leafZ == 0) continue; //dont replace the trunk
							if (Math.Sqrt(leafX * leafX + leafZ * leafZ + Math.Pow(treeHeight - leafRadius - yTrunkLevel + 1, 2)) > leafRadius) continue;
							var leafPosition = new Position(xProposedInWorld + leafX, yProposed + yTrunkLevel, zProposedInWorld + leafZ);
                            if (world.IsValidBlockLocation(leafPosition) && world.GetBlock(leafPosition).Type == Block.BlockType.Air)
							{
                                //need to get the chunk because this block could be expanding into an adjacent chunk
                                world.SetBlock(leafPosition, new Block(Block.BlockType.Leaves));
                                // World.WorldType == WorldType.Winter ? Block.BlockType.SnowLeaves : Block.BlockType.Leaves);
							}
						}
					}
				}
			}
            Log.WriteInfo($"{numberOfTreesToGenerate} trees generated");

        }

		/// <summary>Check if the proposed position has already been taken or would be within the distance tolerance of another taken position.</summary>
		private static bool IsPositionTaken(List<Position> takenPositions, int xProposed, int zProposed, byte distanceTolerance)
		{
			//dont use the Y in this check
			return takenPositions.Any(takenPosition => Math.Abs(xProposed - takenPosition.X) + Math.Abs(zProposed - takenPosition.Z) <= distanceTolerance);
		}
	}
}
