﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sean.Shared;

namespace Sean.WorldGenerator
{
	internal class Generator
	{
		private const int waterLevel = 5;
        private const int globalMapSize = 256;
        private PerlinNoise perlinNoise;
        private PerlinNoise globalMapPerlinNoise;

        public Generator(int seed)
        {
            perlinNoise = new PerlinNoise(seed);
            globalMapPerlinNoise = new PerlinNoise(seed, 50);
        }

        public Array<int> GenerateGlobalMap()
        {
            Debug.WriteLine("Generating global map");

            const int MIN_SURFACE_HEIGHT = Chunk.CHUNK_HEIGHT / 2 - 40; //max amount below half
            const int MAX_SURFACE_HEIGHT = Chunk.CHUNK_HEIGHT / 2 + 8; //max amount above half

            var worldSize = new ArraySize()
            {
                minZ = 0,
                maxZ = globalMapSize,
                minX = 0,
                maxX = globalMapSize,
                minY = MIN_SURFACE_HEIGHT,
                maxY = MAX_SURFACE_HEIGHT,
                scale = 1,
            };

            var heightMap = globalMapPerlinNoise.GetIntMap(worldSize, 1);
            return heightMap;
        }

		public void Generate(Array<int> globalMap, Chunk chunk)
		{
            Debug.WriteLine("Generating new chunk: " + chunk.ChunkCoords);

            const int minNoiseHeight = 0;// Chunk.CHUNK_HEIGHT / 2 - 40; //max amount below half
            const int maxNoiseHeight = 10;// Chunk.CHUNK_HEIGHT / 2 + 8; //max amount above half

            var worldSize = new ArraySize()
            {
                minZ = chunk.ChunkCoords.WorldCoordsZ,
                maxZ = chunk.ChunkCoords.WorldCoordsZ + Chunk.CHUNK_SIZE,
                minX = chunk.ChunkCoords.WorldCoordsX,
                maxX = chunk.ChunkCoords.WorldCoordsX + Chunk.CHUNK_SIZE,
                minY = minNoiseHeight,
                maxY = maxNoiseHeight,
                scale = 1,
            };

            chunk.HeightMap = perlinNoise.GetIntMap(worldSize, 3);
            chunk.MineralMap = perlinNoise.GetFloatMap(worldSize, 2);

        	GenerateChunk(chunk);

            /*
			//loop through chunks again for actions that require the neighboring chunks to be built
			Debug.WriteLine("Completing growth in chunks and building heightmaps...");
            Debug.WriteLine("Completing growth in chunks...", 0, 0);
			foreach (Chunk chunk in World.Chunks)
			{
				//build heightmap here only so we know where to place trees/clutter (it will get built again on world load anyway)
				chunk.BuildHeightMap();

				var takenPositions = new List<Position>(); //positions taken by tree or clutter, ensures neither spawn on top of or directly touching another

				//generate trees
				if (World.GenerateWithTrees) TreeGenerator.Generate(chunk, takenPositions);

				//generate clutter
				//ClutterGenerator.Generate(chunk, takenPositions);
			}

			Debug.WriteLine("World generation complete.");
            */

			//default sun to directly overhead in new worlds
			//SkyHost.SunAngleRadians = OpenTK.MathHelper.PiOver2;
			//SkyHost.SunLightStrength = SkyHost.BRIGHTEST_SKYLIGHT_STRENGTH;

			//Debug.WriteLine("New world saving...");
			//World.SaveToDisk();
			//Debug.WriteLine("New world save complete.");
		}

		private void GenerateChunk(Chunk chunk)
		{
			for (var x = chunk.ChunkCoords.WorldCoordsX; x < chunk.ChunkCoords.WorldCoordsX + Chunk.CHUNK_SIZE; x++)
			{
				for (var z = chunk.ChunkCoords.WorldCoordsZ; z < chunk.ChunkCoords.WorldCoordsZ + Chunk.CHUNK_SIZE; z++)
				{
                    for (var y = 0; y <= Math.Max(chunk.HeightMap[x,z], waterLevel); y++)
					{
						Block.BlockType blockType;
						if (y == 0) //world base
						{
							blockType = Block.BlockType.LavaRock;
						}
						else if (y <= 10) //within 10 blocks of world base
						{
							//dont use gravel at this depth (a quick test showed this cuts 15-20% from world file size)
							blockType = Block.BlockType.Rock;
						}
                        else if (y == chunk.HeightMap[x, z]) //ground level
						{
							if (y > waterLevel)
							{
								switch (World.WorldType)
								{
									case WorldType.Winter: blockType = Block.BlockType.Snow; break;
									case WorldType.Desert: blockType = Block.BlockType.Sand; break;
									default: blockType = Block.BlockType.Grass; break;
								}
							}
							else
							{
								switch (waterLevel - y)
								{
									case 0:
									case 1:
										blockType = Block.BlockType.Sand; //place sand on shoreline and one block below waterline
										break;
									case 2:
									case 3:
									case 4:
									case 5:
										blockType = Block.BlockType.SandDark; //place dark sand under water within 5 blocks of surface
										break;
									default:
										blockType = Block.BlockType.Gravel; //place gravel lower then 5 blocks under water
										break;
								}
							}
						}
                        else if (y > chunk.HeightMap[x, z] && y <= waterLevel)
						{
                            blockType = (World.WorldType == WorldType.Winter && y == waterLevel && y - chunk.HeightMap[x, z] <= 3) ? Block.BlockType.Ice : Block.BlockType.Water;
						}
                        else if (y > chunk.HeightMap[x, z] - 5) //within 5 blocks of the surface
						{
							switch (Settings.Random.Next(0, 37))
							{
								case 0:
									blockType = Block.BlockType.SandDark; //place dark sand below surface
									break;
								case 1:
								case 2:
								case 3:
									blockType = Block.BlockType.Gravel;
									break;
								default:
									blockType = Block.BlockType.Dirt;
									break;
							}
						}
						else
						{
							blockType = Settings.Random.Next(0, 5) == 0 ? Block.BlockType.Gravel : Block.BlockType.Rock;
							//blockType = Block.BlockType.Air; //replace with this to do some quick seismic on what the mineral generator is doing
						}
						chunk.Blocks[x % Chunk.CHUNK_SIZE, y, z % Chunk.CHUNK_SIZE] = new Block(blockType);
					}

                    if (chunk.MineralMap[x, z] < chunk.HeightMap[x, z] - 5 && chunk.MineralMap[x, z] % 1f > 0.80f)
					{
						Block.BlockType mineralType;
                        switch ((int)(chunk.MineralMap[x, z] % 0.01 * 1000))
						{
							case 0:
							case 1:
								mineralType = Block.BlockType.Coal;
								break;
							case 2:
							case 3:
								mineralType = Block.BlockType.Iron;
								break;
							case 4:
							case 5:
								mineralType = Block.BlockType.Copper;
								break;
							case 6:
							case 7:
								mineralType = Block.BlockType.Gold;
								break;
							case 8:
							case 9:
								mineralType = Block.BlockType.Oil;
								break;
							default:
								mineralType = Block.BlockType.Iron;
								break;
						}
                        var mineralPosition = new Position(x, (int)chunk.MineralMap[x, z], z);
						chunk.Blocks[mineralPosition] = new Block(mineralType);
						
						//expand this mineral node
						for (int nextRandom = Settings.Random.Next(); nextRandom % 3600 > 1000; nextRandom = Settings.Random.Next())
						{
							switch (nextRandom % 6)
							{
								case 0:
									mineralPosition.X = Math.Min(mineralPosition.X + 1, World.SizeInBlocksX - 1);
									break;
								case 1:
									mineralPosition.X = Math.Max(mineralPosition.X - 1, 0);
									break;
								case 2:
									mineralPosition.Z = Math.Min(mineralPosition.Z + 1, World.SizeInBlocksZ - 1);
									break;
								case 3:
									mineralPosition.Z = Math.Max(mineralPosition.Z - 1, 0);
									break;
								case 4:
                                mineralPosition.Y = Math.Min(mineralPosition.Y + 1, chunk.HeightMap[x, z] - 5 - 1);
									break;
								case 5:
									mineralPosition.Y = Math.Max(mineralPosition.Y - 1, 0);
									break;
							}
                            //need to get via the chunk because this block could be expanding into an adjacent chunk
                            World.LocalMap.Chunk(mineralPosition).Blocks[mineralPosition] = new Block(mineralType);
						}
					}
				}
			}
		}
	}
}