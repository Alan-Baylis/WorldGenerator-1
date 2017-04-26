﻿using Sean.Shared;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Sean.WorldGenerator
{
    public class River
    {
        private IWorld worldInstance;
        public HashSet<Position> Coords { get; set; }
        public Position Source { get; set; }
        public bool Growing { get; private set; }

        private HashSet<Position> _emptyCoords;
        private Dictionary<Position, float> _heights;
        private float _minScore;
        private Position _minPos;
        private int _maxLength;

        private const int WATER = 2; // TODO - stick these somewhere
        private const int GRASS = 4;
        private const int RIVER = 31;

        public River(IWorld world, Position source)
        {
            worldInstance = world;
            Growing = true;
            _maxLength = 128;
            _minPos = new Position(0, Global.CHUNK_HEIGHT, 0);
            _minScore = Global.CHUNK_HEIGHT;
            Coords = new HashSet<Position>();
            _emptyCoords = new HashSet<Position>();
            _heights = new Dictionary<Position, float>();
            Source = source;
            Add(source, _minScore);
            CalcScore(source);
        }
        public void Grow()
        {
            Add(_minPos, _minScore);
        }
        public void Add(Position pos, float score)
        {
            if (pos.X == 0 && pos.Z == 0) return;

            Coords.Add(pos);
            _emptyCoords.Remove(pos);
            _heights.Remove(pos);

            var block = worldInstance.GetBlock (pos.X, pos.Y, pos.Z);
            switch (block.Type)
            {
                case BlockType.Water1: block = new Block(BlockType.Water2); break;
                case BlockType.Water2: block = new Block(BlockType.Water3); break;
                case BlockType.Water3: block = new Block(BlockType.Water4); break;
                case BlockType.Water4: block = new Block(BlockType.Water5); break;
                case BlockType.Water5: block = new Block(BlockType.Water6); break;
                case BlockType.Water6: block = new Block(BlockType.Water7); break;
                case BlockType.Water7: block = new Block(BlockType.Water); break;
                default: block = new Block(BlockType.Water1); break;
            }
            worldInstance.SetBlock(pos.X, pos.Y, pos.Z, block);
            if (block.Type != BlockType.Water)
            {
                _emptyCoords.Add(pos);
                CalcScore(pos); // re-adds to _heights
            }

            var airBlock = new Block(BlockType.Air);
            var above = worldInstance.GetBlock (pos.X, pos.Y + 1, pos.Z);
            if (above.Type == BlockType.Dirt || above.Type == BlockType.Grass)
            {
                worldInstance.SetBlock(pos.X, pos.Y+1, pos.Z, airBlock);
            }
            above = worldInstance.GetBlock (pos.X, pos.Y + 2, pos.Z);
            if (above.Type == BlockType.Dirt || above.Type == BlockType.Grass) {
                worldInstance.SetBlock(pos.X, pos.Y+2, pos.Z, airBlock);
            }
            worldInstance.GlobalMapTerrain.Set(pos.X, pos.Z, RIVER);
            //Log.WriteInfo($"[River.Add] Adding {pos}");

            AddIfEmpty(pos.X, pos.Y - 1, pos.Z);
            AddIfEmpty(pos.X+1, pos.Y, pos.Z);
            AddIfEmpty(pos.X-1, pos.Y, pos.Z);
            AddIfEmpty(pos.X, pos.Y, pos.Z+1);
            AddIfEmpty(pos.X, pos.Y, pos.Z-1);
            AddIfEmpty(pos.X, pos.Y + 1, pos.Z);

            if (_minPos == pos)
                FindNextLowest();
        }

        private void AddIfEmpty(int x,int y,int z)
        {
            if (Source.GetDistanceExact (new Position (x, y, z)) > _maxLength) {
                // Temporary code to limit length of river
                Growing = false;
                return;
            }

            if (!worldInstance.IsValidBlockLocation (x, y, z)) {
                // Have reached edge of map
                Growing = false;
                return;
            }
                
            var block = worldInstance.GetBlock (x, y, z);
            if (block.IsWater) {
                if (!Coords.Contains (new Position (x, y, z))) {
                    // Have reached another river or the ocean
                    Growing = false;
                }
                return;
            }
            //if (!block.IsSolid)
            if (block.Type == BlockType.Air || block.Type == BlockType.Dirt || block.Type == BlockType.Grass)
            {
                var pos = new Position(x,y,z);
                _emptyCoords.Add(pos);
                CalcScore(pos);

//                // Add solid block down form lower riverbed
//                var blockBelow = worldInstance.GetBlock (x, y-1, z);
//                if (blockBelow.IsSolid) {
//                    if (blockBelow.IsWater) {
//                        if (!Coords.Contains (new Position (x, y-1, z))) {
//                            // Have reached another river or the ocean
//                            Growing = false;
//                        }
//                        return;
//                    }
//                    pos = new Position(x,y-1,z);
//                    _emptyCoords.Add(pos);
//                    CalcScore(pos);
//                }
            }
        }
        private void CalcScore(Position pos)
        {
            var chunk = new ChunkCoords(pos);
            //var loc = chunk.NormLocOnChunk(pos);
            float score;
            const int comp = 5; // compare range
            try
            {
                //var a = worldInstance.GlobalMap[pos.X+Global.CHUNK_SIZE,pos.Z] * (1-loc.Item1);
                //var b = worldInstance.GlobalMap[pos.X-Global.CHUNK_SIZE,pos.Z] * loc.Item1;
                //var c = worldInstance.GlobalMap[pos.X,pos.Z+Global.CHUNK_SIZE] * (1-loc.Item2);
                //var d = worldInstance.GlobalMap[pos.X,pos.Z-Global.CHUNK_SIZE] * loc.Item2;

                //var a = worldInstance.GetBlockHeight(pos.X+comp,pos.Z);
                //var b = worldInstance.GetBlockHeight(pos.X-comp,pos.Z);
                //var c = worldInstance.GetBlockHeight(pos.X,pos.Z+comp);
                //var d = worldInstance.GetBlockHeight(pos.X,pos.Z-comp);

                //score = pos.Y + ((a+b+c+d) / 4) / Global.CHUNK_HEIGHT;
                var block = worldInstance.GetBlock (pos.X, pos.Y, pos.Z);
                var currentWaterHeight = block.WaterHeight;
                score = ((float)pos.Y + ((float)currentWaterHeight / 16)) / Global.CHUNK_HEIGHT;

                var blockBelow = worldInstance.GetBlock (pos.X, pos.Y-1, pos.Z);
                //if (!blockBelow.IsSolid)
                //{
                //    score -= 0.5f;
                //}
            }
            catch (Exception) { // TODO Handle out of array bounds errors better
                score = pos.Y;
            }
            if (!_heights.ContainsKey(pos))
                _heights.Add(pos, score);
            if (score < _minScore)
            {
                _minScore = score;
                _minPos = pos;
            }
        }
        private void FindNextLowest()
        {
            _minScore = Global.CHUNK_HEIGHT;
            foreach(var pos in _emptyCoords)
            {
                var score = _heights[pos];
                if (score < _minScore)
                {
                    _minScore = score;
                    _minPos = pos;
                }
            }
        }
    }

    public class Water
    {
        public Water (IWorld world)
        {
            worldInstance = world;
            Rivers = new List<River>();
            Run();
            //GenerateRivers();

            //ProcessWater(); // TODO - run from own thread
        }

        private IWorld worldInstance;
        private List<River> Rivers;
        private Thread thread;
        private const int WATER = 2; // TODO - stick these somewhere
        private const int GRASS = 4;
        private const int RIVER = 5;

        public void Run()
        {
            thread = new Thread(new ThreadStart(StartThread));
            thread.Start();
        }

        private void CreateRiver()
        {
            for (var i = 0; i < Settings.RiverCount; i++)
            {
                var pos = worldInstance.GetRandomLocationOnLoadedChunk();
                bool isWater = true;
                while (isWater)
                {
                    pos = worldInstance.GetRandomLocationOnLoadedChunk();
                    isWater = (worldInstance.GlobalMapTerrain[pos.X, pos.Z] == WATER);
                }

                var y = 255;
                var b = worldInstance.GetBlock(pos.X, y, pos.Z);
                while (b.IsTransparent)
                {
                    y--;
                    b = worldInstance.GetBlock(pos.X, y, pos.Z);
                }
                var river = new River(worldInstance, new Position(pos.X, y , pos.Z));
                var chunkCoords = new ChunkCoords (pos);
                Log.WriteInfo ($"Creating river from chunk {chunkCoords}");
                Rivers.Add(river);
            }
        }

        private void StartThread()
        {
            try
            {
                while (worldInstance.LoadedChunkCount == 0)
                {
                    System.Threading.Thread.Sleep(5000);
                }
                CreateRiver();
                bool growing = true;
                while (growing)
                {
                    growing = false;
                    foreach (var river in Rivers)
                    {
                        river.Grow();
                        growing |= river.Growing;
                    }
                    //System.Threading.Thread.Sleep(300);
                }
            }
            catch (Exception ex) {
                Log.WriteError ($"Water thread crashed - {ex.Message}");
            }
        }
    }
}

