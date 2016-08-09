using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sean.Shared;
using NoiseLibrary;

namespace Sean.WorldGenerator
{
	internal class Generator
	{
		public const int waterLevel = 20;
        private const int FRACTAL_SIZE = Chunk.CHUNK_SIZE * 5;
        private const int globalMapSize = 128*Chunk.CHUNK_SIZE;
        private PerlinNoise perlinNoise;
        private const int octaves = 1;
        private const double persistence = 0.4;
        private const int minNoiseHeight = -127;
        private const int maxNoiseHeight = 127;
        private CImplicitModuleBase noiseGenerator;

        public Generator(int seed)
        {
            perlinNoise = new PerlinNoise(seed, 100);

            var ground_gradient = new CImplicitGradient(x1: 0, x2: 0, y1: 0, y2: 1);

            var lowland_shape_fractal = new CImplicitFractal(type: EFractalTypes.BILLOW, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 2, freq: 0.25);
            var lowland_autocorrect = new CImplicitAutoCorrect(source: lowland_shape_fractal, low: 0, high: 1);
            var lowland_scale = new CImplicitScaleOffset(source: lowland_autocorrect, scale: 0.125, offset: 0.25);
            var lowland_cache = new CImplicitCache(lowland_scale);
            var lowland_y_scale = new CImplicitScaleDomain(source: lowland_cache, y: 0);
            var lowland_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: lowland_y_scale, tz: 0.0);

            var highland_shape_fractal = new CImplicitFractal(type: EFractalTypes.FBM, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 4, freq: 2);
            var highland_autocorrect = new CImplicitAutoCorrect(source: highland_shape_fractal, low: -1, high: 1);
            var highland_scale = new CImplicitScaleOffset(source: highland_autocorrect, scale: 0.25, offset: 0);
            var highland_cache = new CImplicitCache(highland_scale);
            var highland_y_scale = new CImplicitScaleDomain(source: highland_cache, y: 0);
            var highland_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: highland_y_scale, tz: 0.0);

            var mountain_shape_fractal = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 8, freq: 1);
            var mountain_autocorrect = new CImplicitAutoCorrect(source: mountain_shape_fractal, low: -1, high: 1);
            var mountain_scale = new CImplicitScaleOffset(source: mountain_autocorrect, scale: 0.45, offset: 0.15);
            var mountain_y_scale = new CImplicitScaleDomain(source: mountain_scale, y: 0.25);
            var mountain_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: mountain_y_scale, tz: 0.0);

            var terrain_type_fractal = new CImplicitFractal(type: EFractalTypes.FBM, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 3, freq: 0.125);
            var terrain_autocorrect = new CImplicitAutoCorrect(source: terrain_type_fractal, low: 0, high: 1);
            var terrain_type_y_scale = new CImplicitScaleDomain(source: terrain_autocorrect, y: 0);
            var terrain_type_cache = new CImplicitCache(terrain_type_y_scale);
            var highland_mountain_select = new CImplicitSelect(low: highland_terrain, high: mountain_terrain, control: terrain_type_cache, threshold: 0.55, falloff: 0.2);
            var highland_lowland_select = new CImplicitSelect(low: lowland_terrain, high: highland_mountain_select, control: terrain_type_cache, threshold: 0.25, falloff: 0.15);
            var highland_lowland_select_cache = new CImplicitCache(highland_lowland_select);

            var coastline_shape_fractal = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 8, freq: 1);
            var coastline_autocorrect = new CImplicitAutoCorrect(source: coastline_shape_fractal, low: 0, high: 1);
            var coastline_seamless = new CImplicitSeamlessMapping(source: coastline_autocorrect, seamlessmode: CImplicitSeamlessMapping.EMappingModes.SEAMLESS_X);
            var coastline_cache = new CImplicitCache(coastline_seamless);
            var coastline_y_scale = new CImplicitScaleDomain(source: coastline_cache, y: 0);
            var coastline_scale = new CImplicitScaleOffset(source: coastline_y_scale, scale: 0.5, offset: -1);
            var coastline_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: coastline_scale, tz: 0.0);
            var coastline_radial_mapping = new CImplicitTranslateRadial(source: coastline_terrain, xCentre: globalMapSize/FRACTAL_SIZE/2, zCentre: globalMapSize/FRACTAL_SIZE/2);

            var coastline_highland_lowland_select = new CImplicitTranslateDomain(source: highland_lowland_select_cache, tx: 0.0, ty: coastline_radial_mapping, tz: 0.0);

            var ground_select = new CImplicitSelect(low: 0, high: 1, threshold: 0.5, control: coastline_highland_lowland_select);

            //    var cave_attenuate_bias = new CImplicitMath(op: EMathOperation.BIAS, source: highland_lowland_select_cache, p: 0.45);
            //    var cave_shape1 = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 1, freq: 4);
            //    var cave_shape2 = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 1, freq: 4);
            //    var cave_shape_attenuate = new CImplicitCombiner(type: ECombinerTypes.MULT, source0: cave_shape1, source1: cave_attenuate_bias, source2: cave_shape2);
            //    var cave_perturb_fractal = new CImplicitFractal(type: EFractalTypes.FBM, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 6, freq: 3);
            //    var cave_perturb_scale = new CImplicitScaleOffset(source: cave_perturb_fractal, scale: 0.5, offset: 0);
            //    var cave_perturb = new CImplicitTranslateDomain(source: cave_shape_attenuate, tx: cave_perturb_scale, ty: 0, tz: 0);
            //    var cave_select = new CImplicitSelect(low: 1, high: 0, control: cave_perturb, threshold: 0.48, falloff: 0);

            //    var ground_cave_multiply = new CImplicitCombiner(type: ECombinerTypes.MULT, source0: cave_select, source1: ground_select);

            noiseGenerator = coastline_highland_lowland_select;
        }

        public Array<int> GenerateGlobalMap()
        {
            Debug.WriteLine("Generating global map");
            var worldSize = new ArraySize()
            {
                minZ = 0,
                maxZ = globalMapSize,
                minX = 0,
                maxX = globalMapSize,
                minY = minNoiseHeight,
                maxY = maxNoiseHeight,
                scale = 8 //Chunk.CHUNK_SIZE,
            };

            var heightMap = new Array<int>(worldSize);
            for (int z = worldSize.minZ; z < worldSize.maxZ; z += worldSize.scale)
            {
                for (int x = worldSize.minX; x < worldSize.maxX; x += worldSize.scale)
                {
                    int d = worldSize.maxY / 2;
                    int y = d;
                    while (d > 1)
                    {
                        d /= 2;
                        if (noiseGenerator.get ((double)x / FRACTAL_SIZE, (double)y / maxNoiseHeight, (double)z / FRACTAL_SIZE) < 0.5)
                            y += d;
                        else
                            y -= d;
                    }
                    heightMap[x, z] = y;
                }
            }
            return heightMap;
        }

        public void Generate(Chunk chunk)
		{
            Debug.WriteLine("Generating new chunk: " + chunk.ChunkCoords);
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

            for (int z = worldSize.minZ; z < worldSize.maxZ; z += worldSize.scale)
            {
                for (int x = worldSize.minX; x < worldSize.maxX; x += worldSize.scale)
                {
                    for (int y = 0; y < maxNoiseHeight; y++)
                    {
                        //double p = perlinNoise.OctavePerlin(worldSize, x, y, z, octaveCount, persistence);
                        double p = noiseGenerator.get((double)x/FRACTAL_SIZE, (double)y/maxNoiseHeight, (double)z/FRACTAL_SIZE);
                        var blockType = p < 0.5 ? Block.BlockType.Rock : Block.BlockType.Air;
                        chunk.Blocks[x % Chunk.CHUNK_SIZE, y, z % Chunk.CHUNK_SIZE] = new Block(blockType);
                    }
                }
            }

            chunk.BuildHeightMap();
            //GenerateChunk(chunk);

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
					}
				}
			}
		}
	}
}