using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private PerlinNoise perlinNoise;
        private const int octaves = 1;
        private const double persistence = 0.4;
        private int chunkMidpoint = Global.CHUNK_SIZE / 2;

        public Generator(IWorld world, int seed)
        {
            worldInstance = world;
            perlinNoise = new PerlinNoise(seed, 100);

            terrainGenerator = CreateTerrainGenerator();
            biosphereGenerator = CreateBiosphereGenerator();
        }

        private CImplicitModuleBase CreateTerrainGenerator()
        { 
            var ground_gradient = new CImplicitGradient(x1: 0, x2: 0, y1: 0, y2: 1);
            
            var lowland_shape_fractal = new CImplicitFractal(type: EFractalTypes.BILLOW, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 2, freq: 1.25);
            var lowland_autocorrect = new CImplicitAutoCorrect(source: lowland_shape_fractal, low: -1, high: 1);
            var lowland_scale = new CImplicitScaleOffset(source: lowland_autocorrect, scale: 0.10, offset: 0.0);
            var lowland_cache = new CImplicitCache(lowland_scale);
            var lowland_y_scale = new CImplicitScaleDomain(source: lowland_cache, y: 0);
            var lowland_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: lowland_y_scale, tz: 0.0);
            //var lowland_terrain = new CImplicitGradient(x1:0, x2:0, y1:0, y2:1);

            //var highland_shape_fractal = new CImplicitFractal(type: EFractalTypes.FBM, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 4, freq: 1);
            //var highland_autocorrect = new CImplicitAutoCorrect(source: highland_shape_fractal, low: -1, high: 1);
            //var highland_scale = new CImplicitScaleOffset(source: highland_autocorrect, scale: 0.15, offset: 0);
            //var highland_cache = new CImplicitCache(highland_scale);
            //var highland_y_scale = new CImplicitScaleDomain(source: highland_cache, y: 0);
            //var highland_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: highland_y_scale, tz: 0.0);

            var mountain_shape_fractal = new CImplicitFractal(type: EFractalTypes.RIDGEDMULTI, basistype: CImplicitBasisFunction.EBasisTypes.GRADIENT, interptype: CImplicitBasisFunction.EInterpTypes.QUINTIC, octaves: 4, freq: 3);
            var mountain_autocorrect = new CImplicitAutoCorrect(source: mountain_shape_fractal, low: -1, high: 1);
            var mountain_scale = new CImplicitScaleOffset(source: mountain_autocorrect, scale: 0.4, offset: 0.0);
            var mountain_cache = new CImplicitCache(mountain_scale);
            var mountain_y_scale = new CImplicitScaleDomain(source: mountain_cache, y: 0.25);
            var mountain_terrain = new CImplicitTranslateDomain(source: ground_gradient, tx: 0.0, ty: mountain_y_scale, tz: 0.0);
            //var mountain_terrain = new CImplicitGradient(x1:0, x2:0, y1:0, y2:1);

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
            var island_offset = new CImplicitScaleOffset(source: island_translate, scale: 0.75, offset: -0.40);
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

        public Array<int> GenerateIslandMap(uint octaves = 8, double freq = 0.42, double xOffset = 0, double zOffset = 0, double scale = 1.0)
        {
            islandGenerator = CreateIslandTerrainGenerator(octaves, freq, xOffset, zOffset, scale);
            Debug.WriteLine("Generating island map");
            var worldSize = new ArraySize()
            {
                minZ = 0,
                maxZ = Settings.globalMapSize,
                minX = 0,
                maxX = Settings.globalMapSize,
                minY = Settings.minNoiseHeight,
                maxY = Settings.maxNoiseHeight,
                scale = Global.CHUNK_SIZE,
            };

            var heightMap = new Array<int>(worldSize);
            for (int z = worldSize.minZ; z < worldSize.maxZ; z += worldSize.scale)
            {
                for (int x = worldSize.minX; x < worldSize.maxX; x += worldSize.scale)
                {
                    int d = worldSize.maxY - worldSize.minY;
                    int y = (worldSize.maxY + worldSize.minY) / 2;
                    while (d > 1)
                    {
                        d /= 2;
                        var p = islandGenerator.get(
                            //(double)(x + xOffset + chunkMidpoint) / Settings.FRACTAL_SIZE, 
                            (double)(x + chunkMidpoint) / Settings.FRACTAL_SIZE, 
                            (double)y / Settings.maxNoiseHeight, 
                            //(double)(z + zOffset + chunkMidpoint) / Settings.FRACTAL_SIZE);
                            (double)(z + chunkMidpoint) / Settings.FRACTAL_SIZE);
                        if (p < 0.5)
                            y += d;
                        else
                            y -= d;
                    }
                    if (y < worldSize.minY) y = worldSize.minY;
                    if (y > worldSize.maxY) y = worldSize.maxY;
                    heightMap[x, z] = worldSize.maxY - y;
                }
            }
            return heightMap;
        }

        public Array<int> GenerateGlobalMap()
        {
            Debug.WriteLine("Generating global map");
            var worldSize = new ArraySize()
            {
                minZ = 0,
                maxZ = Settings.globalMapSize,
                minX = 0,
                maxX = Settings.globalMapSize,
                minY = Settings.minNoiseHeight,
                maxY = Settings.maxNoiseHeight,
                scale = Global.CHUNK_SIZE,
            };

            var heightMap = new Array<int>(worldSize);
            for (int z = worldSize.minZ; z < worldSize.maxZ; z += worldSize.scale)
            {
                for (int x = worldSize.minX; x < worldSize.maxX; x += worldSize.scale)
                {
                    int d = worldSize.maxY - worldSize.minY;
                    int y = (worldSize.maxY + worldSize.minY)/2;
                    while (d > 1)
                    {
                        d /= 2;
                        var p = terrainGenerator.get ((double)(x+chunkMidpoint) / Settings.FRACTAL_SIZE, (double)y / Settings.maxNoiseHeight, (double)(z+chunkMidpoint) / Settings.FRACTAL_SIZE);
                        if (p < 0.5)
                            y += d;
                        else
                            y -= d;
                    }
                    if (y < worldSize.minY) y = worldSize.minY;
                    if (y > worldSize.maxY) y = worldSize.maxY;
                    heightMap[x, z] = worldSize.maxY-y;
                }
            }
            return heightMap;
        }

        public double CalcGlobalBiosphere(int x, int z)
        {
            return biosphereGenerator.get((double)(x+chunkMidpoint) / Settings.FRACTAL_SIZE, (double)(z+chunkMidpoint) / Settings.FRACTAL_SIZE);
        }
        public double CalcBiosphere(int x, int z)
        {
            return biosphereGenerator.get((double)x, (double)z);
        }

        public void Generate(Chunk chunk)
		{
            Debug.WriteLine("Generating new chunk: " + chunk.ChunkCoords);
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

            _generateQueue = new UniqueQueue<Position>();
            _generateQueue.Enqueue(new Position(
                chunk.ChunkCoords.WorldCoordsX + Global.CHUNK_SIZE/2, 
                Settings.maxNoiseHeight, 
                chunk.ChunkCoords.WorldCoordsZ + Global.CHUNK_SIZE/2));

            if (worldInstance.IsChunkLoaded(new ChunkCoords(chunk.ChunkCoords.X - 1, chunk.ChunkCoords.Z))) {
                for (var z = chunk.MinPosition.Z; z <= chunk.MaxPosition.Z; z++) {
                    for (var y = chunk.MinPosition.Y; y < chunk.MaxPosition.Y; y++)
                    {
                        var block = worldInstance.GetBlock(chunk.MinPosition.X - 1, y, z);
                        if (block.IsTransparent) _generateQueue.Enqueue(new Position(chunk.MinPosition.X, y, z));
                    }
                }
            }
            if (worldInstance.IsChunkLoaded (new ChunkCoords (chunk.ChunkCoords.X + 1, chunk.ChunkCoords.Z))) {
                for (var z = chunk.MinPosition.Z; z <= chunk.MaxPosition.Z; z++) {
                    for (var y = chunk.MinPosition.Y; y < chunk.MaxPosition.Y; y++)
                    {
                        var block = worldInstance.GetBlock(chunk.MaxPosition.X + 1, y, z);
                        if (block.IsTransparent) _generateQueue.Enqueue(new Position(chunk.MaxPosition.X, y, z));
                    }
                }
            }
            if (worldInstance.IsChunkLoaded (new ChunkCoords (chunk.ChunkCoords.X, chunk.ChunkCoords.Z - 1))) {
                for (var x = chunk.MinPosition.X; x <= chunk.MaxPosition.X; x++) {
                    for (var y = chunk.MinPosition.Y; y < chunk.MaxPosition.Y; y++) {
                        var block = worldInstance.GetBlock(x, y, chunk.MinPosition.Z - 1);
                        if (block.IsTransparent) _generateQueue.Enqueue(new Position(x, y, chunk.MinPosition.Z));
                    }
                }
            }
            if (worldInstance.IsChunkLoaded (new ChunkCoords (chunk.ChunkCoords.X, chunk.ChunkCoords.Z + 1))) {
                for (var x = chunk.MinPosition.X; x <= chunk.MaxPosition.X; x++) {
                    for (var y = chunk.MinPosition.Y; y < chunk.MaxPosition.Y; y++) {
                        var block = worldInstance.GetBlock(x, y, chunk.MaxPosition.Z + 1);
                        if (block.IsTransparent) _generateQueue.Enqueue(new Position(x, y, chunk.MaxPosition.Z));
                    }
                }
            }
                
            GenerateChunkCells(chunk);

            chunk.BuildHeightMap();

            TreeGenerator.Generate(worldInstance, chunk);
            chunk.FinishedGeneration = true;
        }

        private UniqueQueue<Position> _generateQueue;
        private void GenerateChunkCells(Chunk chunk)
        {
            while (_generateQueue.Count > 0)
            {
                var pos = _generateQueue.Dequeue();
                if (pos == null)
                {
                    Console.WriteLine("Null position in queue?");
                    continue;
                }
                var x = pos.X;
                var y = pos.Y;
                var z = pos.Z;
                  
                if (worldInstance.GetBlock(x,y,z).Type == Block.BlockType.Unknown)
                {
                    var block = GenerateCell(x, y, z);
                    if (block.Type == Block.BlockType.Unknown) Console.WriteLine ("Unknown block type generated?");
                    worldInstance.SetBlock (x, y, z, block);
                    if (worldInstance.GetBlock (x, y, z).Type == Block.BlockType.Unknown) Console.WriteLine ("Block not set?");
                    if (block.IsTransparent)
                    {
                        ExpandSearchCheckBlock (x - 1, y, z);
                        ExpandSearchCheckBlock (x + 1, y, z);
                        if (y-1 >= Settings.minNoiseHeight)
                            ExpandSearchCheckBlock (x, y - 1, z);
                        if (y+1 < Settings.maxNoiseHeight)
                            ExpandSearchCheckBlock (x, y + 1, z);
                        ExpandSearchCheckBlock (x, y, z - 1);
                        ExpandSearchCheckBlock (x, y, z + 1);
                    }
                }
            }
        }
        private void ExpandSearchCheckBlock(int x,int y,int z)
        {
            if (worldInstance.IsValidBlockLocation(x, y, z) && worldInstance.GetBlock(x,y,z).Type == Block.BlockType.Unknown)
                _generateQueue.Enqueue (new Position (x, y, z));
        }
        private Block GenerateCell(int x, int y, int z)
        {
            //double p = perlinNoise.OctavePerlin(worldSize, x, y, z, octaveCount, persistence);
            double p = terrainGenerator.get((double)x / Settings.FRACTAL_SIZE, (double)(Settings.maxNoiseHeight - y) / Settings.maxNoiseHeight, (double)z / Settings.FRACTAL_SIZE);
            var blockType = p > 0.5 ? Block.BlockType.Dirt : Block.BlockType.Air;
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