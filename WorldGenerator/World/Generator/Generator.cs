﻿using System;
using Sean.Shared;
using NoiseLibrary;

namespace Sean.WorldGenerator
{
	public class Generator
	{
        private CImplicitModuleBase islandGenerator;
        private CImplicitModuleBase terrainGenerator;
        private CImplicitModuleBase biosphereGenerator;
        private IWorld worldInstance;
        //private PerlinNoise perlinNoise;
        private const int octaves = 1;
        private const double persistence = 0.4;

        public Generator(IWorld world, int seed)
        {
            Log.WriteInfo($"Creating Terrain Generator...");
            worldInstance = world;
            //perlinNoise = new PerlinNoise(seed, 100);

            terrainGenerator = CreateTerrainGenerator();
            biosphereGenerator = CreateBiosphereGenerator();
        }

        private CImplicitModuleBase CreateTerrainGenerator()
        {
            var ground_gradient = new CImplicitGradient(x1: 0, x2: 0, y1: 0, y2: 1);

            var lowland_gradient = new CImplicitGradient(x1: 0, x2: 0, y1: 0.8, y2: 1);
            var lowland_shape_fractal = new CImplicitFractal(type: EFractalTypes.BILLOW, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 2, freq: 1.25);
            var lowland_autocorrect = new CImplicitAutoCorrect(source: lowland_shape_fractal, low: -1, high: 1);
            var lowland_scale = new CImplicitScaleOffset(source: lowland_autocorrect, scale: 0.05, offset: 0.0);
            var lowland_cache = new CImplicitCache(lowland_scale);
            var lowland_y_scale = new CImplicitScaleDomain(source: lowland_cache, y: 0);
            var lowland_terrain = new CImplicitTranslateDomain(source: lowland_gradient, tx: 0.0, ty: lowland_y_scale, tz: 0.0);

            //var highland_gradient = new CImplicitGradient(x1: 0, x2: 0, y1: 0.7, y2: 1);
            //var highland_shape_fractal = new CImplicitFractal(type: EFractalTypes.FBM, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 4, freq: 1);
            //var highland_autocorrect = new CImplicitAutoCorrect(source: highland_shape_fractal, low: -1, high: 1);
            //var highland_scale = new CImplicitScaleOffset(source: highland_autocorrect, scale: 0.25, offset: 0);
            //var highland_cache = new CImplicitCache(highland_scale);
            //var highland_y_scale = new CImplicitScaleDomain(source: highland_cache, y: 0);
            //var highland_terrain = new CImplicitTranslateDomain(source: highland_gradient, tx: 0.0, ty: highland_y_scale, tz: 0.0);

            var mountain_gradient = new CImplicitGradient(x1: 0, x2: 0, y1: 0.7, y2: 1);
            var mountain_shape_fractal = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 4, freq: 3);
            var mountain_autocorrect = new CImplicitAutoCorrect(source: mountain_shape_fractal, low: -1, high: 1);
            var mountain_scale = new CImplicitScaleOffset(source: mountain_autocorrect, scale: 0.23, offset: 0.15);
            var mountain_cache = new CImplicitCache(mountain_scale);
            var mountain_y_scale = new CImplicitScaleDomain(source: mountain_cache, y: 0.25);
            var mountain_terrain = new CImplicitTranslateDomain(source: mountain_gradient, tx: 0.0, ty: mountain_y_scale, tz: 0.0);

            var terrain_type_fractal = new CImplicitFractal(type: EFractalTypes.FBM, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 3, freq: 0.5);
            var terrain_autocorrect = new CImplicitAutoCorrect(source: terrain_type_fractal, low: 0, high: 1);
            var terrain_type_y_scale = new CImplicitScaleDomain(source: terrain_autocorrect, y: 0);
            var terrain_type_cache = new CImplicitCache(terrain_type_y_scale);
            //var highland_mountain_select = new CImplicitSelect(low: highland_terrain, high: mountain_terrain, control: terrain_type_cache, threshold: 0.55, falloff: 0.2);
            var mountain_lowland_select = new CImplicitSelect(low: lowland_terrain, high: mountain_terrain, control: terrain_type_cache, threshold: 0.5, falloff: 0.2);
            var mountain_lowland_select_cache = new CImplicitCache(mountain_lowland_select);
            

            /*
            var coastline_shape_fractal = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 2, freq: 1);
            var coastline_autocorrect = new CImplicitAutoCorrect(source: coastline_shape_fractal, low: 0, high: 1);
            var coastline_seamless = new CImplicitSeamlessMapping(source: coastline_autocorrect, seamlessmode: CImplicitSeamlessMapping.EMappingModes.SEAMLESS_X);
            var coastline_cache = new CImplicitCache(coastline_seamless);
            var coastline_y_scale = new CImplicitScaleDomain(source: coastline_cache, y: 0);
            var coastline_scale = new CImplicitScaleOffset(source: coastline_y_scale, scale: 0.5, offset: -1.2);
            var coastline_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: coastline_scale, tz: 0.0);
            var coastline_radial_mapping = new CImplicitTranslateRadial(source: coastline_terrain, xCentre: Settings.globalMapSize / Settings.FRACTAL_SIZE / 2, zCentre: Settings.globalMapSize / Settings.FRACTAL_SIZE / 2);
            //var coastline_radial_mapping = new CImplicitGradient(x1:0, x2:0, y1:0, y2:1);
            */
            uint octaves = 8;
            double freq = 1.2;
            double xOffset = 5.65;
            double zOffset = 2.52;
            double scale = 0.22;
            var island_shape_fractal = new CImplicitFractal(type: EFractalTypes.MULTI,
                basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: octaves, freq: freq);
            var island_autocorrect = new CImplicitAutoCorrect(source: island_shape_fractal, low: 0, high: 1);
            var island_translate = new CImplicitTranslateDomain(source: island_autocorrect, tx: (double)xOffset, ty: 1.0, tz: (double)zOffset);
            var island_offset = new CImplicitScaleOffset(source: island_translate, scale: 0.70, offset: -0.40);
            var island_scale = new CImplicitScaleDomain(source: island_offset, x: scale, y: 0, z: scale);
            var island_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: island_scale, tz: 0.0);

            var coastline_highland_lowland_select = new CImplicitTranslateDomain(source: mountain_lowland_select_cache, tx: 0.0, ty: island_terrain, tz: 0.0);
            //var coastline_highland_lowland_select = mountain_lowland_select_cache;

            //var ground_select = new CImplicitSelect(low: 0, high: 1, threshold: 0.5, control: coastline_highland_lowland_select);

            //    var cave_attenuate_bias = new CImplicitMath(op: EMathOperation.BIAS, source: highland_lowland_select_cache, p: 0.45);
            //    var cave_shape1 = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 1, freq: 4);
            //    var cave_shape2 = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 1, freq: 4);
            //    var cave_shape_attenuate = new CImplicitCombiner(type: ECombinerTypes.MULT, source0: cave_shape1, source1: cave_attenuate_bias, source2: cave_shape2);
            //    var cave_perturb_fractal = new CImplicitFractal(type: EFractalTypes.FBM, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 6, freq: 3);
            //    var cave_perturb_scale = new CImplicitScaleOffset(source: cave_perturb_fractal, scale: 0.5, offset: 0);
            //    var cave_perturb = new CImplicitTranslateDomain(source: cave_shape_attenuate, tx: cave_perturb_scale, ty: 0, tz: 0);
            //    var cave_select = new CImplicitSelect(low: 1, high: 0, control: cave_perturb, threshold: 0.48, falloff: 0);

            //    var ground_cave_multiply = new CImplicitCombiner(type: ECombinerTypes.MULT, source0: cave_select, source1: ground_select);

            return coastline_highland_lowland_select;
        }

        private CImplicitModuleBase CreateIslandTerrainGenerator(uint octaves, double freq, double xOffset, double zOffset, double scale)
        {
            var ground_gradient = new CImplicitGradient(x1: 0, x2: 0, y1: 0, y2: 1);
            var island_shape_fractal = new CImplicitFractal(type: EFractalTypes.MULTI, 
                basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: octaves, freq: freq);
            var island_autocorrect = new CImplicitAutoCorrect(source: island_shape_fractal, low: 0, high: 1);
            var island_translate = new CImplicitTranslateDomain(source: island_autocorrect, tx: (double)xOffset, ty: 1.0, tz: (double)zOffset);
            var island_offset = new CImplicitScaleOffset(source: island_translate, scale: 0.70, offset: -0.40);
            var island_scale = new CImplicitScaleDomain(source: island_offset, x: scale, y: 0, z: scale);
            var island_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: island_scale, tz: 0.0);

            return island_terrain;
        }

        private CImplicitModuleBase CreateBiosphereGenerator()
        {
            var bio_type_fractal = new CImplicitFractal(type: EFractalTypes.FBM, 
                basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 3, freq: 0.5);
            var bio_autocorrect = new CImplicitAutoCorrect(source: bio_type_fractal, low: 0, high: 1);

            return bio_autocorrect;
        }

        public Array<byte> GenerateGlobalMap()
        {
            Log.WriteInfo("Generating global map");
            var worldSize = new ArraySize()
            {
                minZ = 0,
                maxZ = Settings.globalMapSize + Global.CHUNK_SIZE,
                minX = 0,
                maxX = Settings.globalMapSize + Global.CHUNK_SIZE,
                minY = Settings.minNoiseHeight,
                maxY = Settings.maxNoiseHeight,
                scale = Global.CHUNK_SIZE,
            };

            var halfscale = worldSize.scale / 2;
            var heightMap = new Array<byte>(worldSize);
            for (int z = worldSize.minZ; z < worldSize.maxZ-worldSize.scale; z += worldSize.scale)
            {
                for (int x = worldSize.minX; x < worldSize.maxX-worldSize.scale; x += worldSize.scale)
                {
                    int d = worldSize.maxY;
                    int y = d/2;
                    while (d > 1)
                    {
                        d /= 2;
                        //var p = terrainGenerator.get ((double)x / Settings.FRACTAL_SIZE, (double)y / Settings.maxNoiseHeight, (double)z / Settings.FRACTAL_SIZE);
                        var block = GenerateCell(x+halfscale, y, z+halfscale);
                        if (block.IsTransparent)
                            y -= d;
                        else
                            y += d;
                    }
                    if (y < worldSize.minY) y = worldSize.minY;
                    if (y > worldSize.maxY) y = worldSize.maxY;
                    heightMap[x, z] = (byte)y;
                }
            }
            return heightMap;
        }

        public double CalcGlobalBiosphere(int x, int z)
        {
            return biosphereGenerator.get((double)x / Settings.FRACTAL_SIZE, (double)z / Settings.FRACTAL_SIZE);
        }
        public double CalcBiosphere(int x, int z)
        {
            return biosphereGenerator.get((double)x, (double)z);
        }

        public void Generate(Chunk chunk)
		{
            Log.WriteInfo("Generating new chunk: " + chunk.ChunkCoords);
            chunk.FinishedGeneration = false;
            var worldSize = new ArraySize()
            {
                minZ = chunk.ChunkCoords.WorldCoordsZ,
                maxZ = chunk.ChunkCoords.WorldCoordsZ + Global.CHUNK_SIZE,
                minX = chunk.ChunkCoords.WorldCoordsX,
                maxX = chunk.ChunkCoords.WorldCoordsX + Global.CHUNK_SIZE,
                minY = Settings.minNoiseHeight,
                maxY = Settings.maxNoiseHeight,
                scale = 1,
            };

            var generateQueue = new UniqueQueue<Position>();
            generateQueue.Enqueue(new Position(
                chunk.ChunkCoords.WorldCoordsX + Global.CHUNK_SIZE/2, 
                Settings.maxNoiseHeight, 
                chunk.ChunkCoords.WorldCoordsZ + Global.CHUNK_SIZE/2));

            if (worldInstance.IsChunkLoaded(new ChunkCoords(chunk.ChunkCoords.X - 1, chunk.ChunkCoords.Z))) {
                for (var z = chunk.MinPosition.Z; z <= chunk.MaxPosition.Z; z++) {
                    for (var y = chunk.MinPosition.Y; y < chunk.MaxPosition.Y; y++)
                    {
                        var block = worldInstance.GetBlock(chunk.MinPosition.X - 1, y, z);
                        if (block.IsTransparent) generateQueue.Enqueue(new Position(chunk.MinPosition.X, y, z));
                    }
                }
            }
            if (worldInstance.IsChunkLoaded (new ChunkCoords (chunk.ChunkCoords.X + 1, chunk.ChunkCoords.Z))) {
                for (var z = chunk.MinPosition.Z; z <= chunk.MaxPosition.Z; z++) {
                    for (var y = chunk.MinPosition.Y; y < chunk.MaxPosition.Y; y++)
                    {
                        var block = worldInstance.GetBlock(chunk.MaxPosition.X + 1, y, z);
                        if (block.IsTransparent) generateQueue.Enqueue(new Position(chunk.MaxPosition.X, y, z));
                    }
                }
            }
            if (worldInstance.IsChunkLoaded (new ChunkCoords (chunk.ChunkCoords.X, chunk.ChunkCoords.Z - 1))) {
                for (var x = chunk.MinPosition.X; x <= chunk.MaxPosition.X; x++) {
                    for (var y = chunk.MinPosition.Y; y < chunk.MaxPosition.Y; y++) {
                        var block = worldInstance.GetBlock(x, y, chunk.MinPosition.Z - 1);
                        if (block.IsTransparent) generateQueue.Enqueue(new Position(x, y, chunk.MinPosition.Z));
                    }
                }
            }
            if (worldInstance.IsChunkLoaded (new ChunkCoords (chunk.ChunkCoords.X, chunk.ChunkCoords.Z + 1))) {
                for (var x = chunk.MinPosition.X; x <= chunk.MaxPosition.X; x++) {
                    for (var y = chunk.MinPosition.Y; y < chunk.MaxPosition.Y; y++) {
                        var block = worldInstance.GetBlock(x, y, chunk.MaxPosition.Z + 1);
                        if (block.IsTransparent) generateQueue.Enqueue(new Position(x, y, chunk.MaxPosition.Z));
                    }
                }
            }
                
            GenerateChunkCells(generateQueue);

            chunk.BuildHeightMap();

            TreeGenerator.Generate(worldInstance, chunk);
            chunk.FinishedGeneration = true;
        }

        private void GenerateChunkCells(UniqueQueue<Position> generateQueue)
        {
            while (generateQueue.Count > 0)
            {
                var pos = generateQueue.Dequeue();
                if (pos == null)
                {
                    Log.WriteError("Null position in queue?");
                    continue;
                }
                var x = pos.X;
                var y = pos.Y;
                var z = pos.Z;
                  
                if (worldInstance.GetBlock(x,y,z).Type == BlockType.Unknown)
                {
                    var block = GenerateCell(x, y, z);
                    if (block.Type == BlockType.Unknown) Log.WriteError("Unknown block type generated?");
                    worldInstance.SetBlock (x, y, z, block);
                    if (worldInstance.GetBlock (x, y, z).Type == BlockType.Unknown) Log.WriteError("Block not set?");
                    if (block.IsTransparent)
                    {
                        ExpandSearchCheckBlock (x - 1, y, z, generateQueue);
                        ExpandSearchCheckBlock (x + 1, y, z, generateQueue);
                        if (y-1 >= Settings.minNoiseHeight)
                            ExpandSearchCheckBlock (x, y - 1, z, generateQueue);
                        if (y+1 < Settings.maxNoiseHeight)
                            ExpandSearchCheckBlock (x, y + 1, z, generateQueue);
                        ExpandSearchCheckBlock (x, y, z - 1, generateQueue);
                        ExpandSearchCheckBlock (x, y, z + 1, generateQueue);
                    }
                }
            }
        }
        public void GenerateSurroundingCells(int x, int y, int z)
        {
            var generateQueue = new UniqueQueue<Position>();
            generateQueue.Enqueue (new Position (x+1, y, z));
            generateQueue.Enqueue (new Position (x-1, y, z));
            generateQueue.Enqueue (new Position (x, y+1, z));
            generateQueue.Enqueue (new Position (x, y-1, z));
            generateQueue.Enqueue (new Position (x, y, z+1));
            generateQueue.Enqueue (new Position (x, y, z-1));
            GenerateChunkCells (generateQueue);
        }
        private void ExpandSearchCheckBlock(int x,int y,int z, UniqueQueue<Position> generateQueue)
        {
            if (worldInstance.IsLoadedBlockLocation(x, y, z) && worldInstance.GetBlock(x,y,z).Type == BlockType.Unknown)
                generateQueue.Enqueue (new Position (x, y, z));
        }
        private Block GenerateCell(int x, int y, int z)
        {
            //double p = perlinNoise.OctavePerlin(worldSize, x, y, z, octaveCount, persistence);
            double p = terrainGenerator.get((double)x / Settings.FRACTAL_SIZE, (double)(Settings.maxNoiseHeight - y) / Settings.maxNoiseHeight, (double)z / Settings.FRACTAL_SIZE);
            var blockType = BlockType.Air;
            if (p > 0.54)
                blockType = BlockType.Rock;
            else if (p > 0.5)
                blockType = BlockType.Dirt;
            return new Block(blockType);
        }

        /*
		private void GenerateChunk(Chunk chunk)
		{
            for (var x = chunk.ChunkCoords.WorldCoordsX; x < chunk.ChunkCoords.WorldCoordsX + Global.CHUNK_SIZE; x++)
			{
                for (var z = chunk.ChunkCoords.WorldCoordsZ; z < chunk.ChunkCoords.WorldCoordsZ + Global.CHUNK_SIZE; z++)
				{
                    for (var y = 0; y <= Math.Max(chunk.HeightMap[x,z], Settings.waterLevel); y++)
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
                            if (y > Settings.waterLevel)
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
                                switch (Settings.waterLevel - y)
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
                        else if (y > chunk.HeightMap[x, z] && y <= Settings.waterLevel)
						{
                            blockType = (World.WorldType == WorldType.Winter && y == Settings.waterLevel && y - chunk.HeightMap[x, z] <= 3) ? Block.BlockType.Ice : Block.BlockType.Water;
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
                        chunk.Blocks[x % Global.CHUNK_SIZE, y, z % Global.CHUNK_SIZE] = new Block(blockType);
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
		}*/
	}
}