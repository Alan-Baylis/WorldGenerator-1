using Sean.Shared;
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

        public River(IWorld world)
        {
            worldInstance = world;
            Growing = true;
            _maxLength = int.MaxValue; //128;
            Coords = new HashSet<Position>();
            _emptyCoords = new HashSet<Position>();
            _heights = new Dictionary<Position, float>();
            _minScore = int.MaxValue;
            Source = FindGoodSourceSpot ();
            Add(Source, _minScore);
            var score = CalcScore(Source);
            //_heights.Add(Source, score);
        }
        public Position FindGoodSourceSpot()
        {
            //var best = worldInstance.GetRandomLocationOnLoadedChunk ();
            var bestScore = 0.0;
            var best = new Position(0,0,0);
            for (int i = 0; i < 30; i++)
            {
                int x = Settings.Random.Next(worldInstance.GlobalMap.Size.minX, worldInstance.GlobalMap.Size.maxX);
                int z = Settings.Random.Next(worldInstance.GlobalMap.Size.minZ, worldInstance.GlobalMap.Size.maxZ);

                var riverScore = PotentialRiver(x, z);
                if (riverScore > bestScore)
                {
                    bestScore = riverScore;
                    best = new Position(x, 0, z);
                }
            }
            var y = worldInstance.GetBlockHeight (best.X, best.Z);
            return new Position(best.X, y, best.Z);
        }
        private float PotentialRiver(int x, int z)
        {
            var heightScore = (Global.CHUNK_HEIGHT - (double)(worldInstance.GlobalMap[x, z] - Global.waterLevel)) / Global.CHUNK_HEIGHT;
            var midX = (worldInstance.GlobalMap.Size.maxX / 2);
            var midZ = (worldInstance.GlobalMap.Size.maxZ / 2);
            var max = Math.Max(midX, midZ);
            var posScore = (max - (Math.Sqrt((midX - x) * (midX - x) + (midZ - z) * (midZ - z)))) / max;
            return (float)(heightScore + posScore);
        }

        public void Grow()
        {
            Add(_minPos, _minScore);
        }
        public void Add(Position pos, float score)
        {
            if (pos.X == 0 && pos.Z == 0) return;

            if (!Coords.Contains(pos))
                Coords.Add(pos);
            _emptyCoords.Remove(pos);
            _heights.Remove(pos);

            var newWaterBlock = false;
            var block = worldInstance.GetBlock (pos.X, pos.Y, pos.Z);
            switch (block.Type)
            {
                case BlockType.Water1: block = new Block(BlockType.Water4); break;
                case BlockType.Water2: block = new Block(BlockType.Water4); break;
                case BlockType.Water3: block = new Block(BlockType.Water4); break;
                case BlockType.Water4: block = new Block(BlockType.Water7); break;
                case BlockType.Water5: block = new Block(BlockType.Water7); break;
                case BlockType.Water6: block = new Block(BlockType.Water7); break;
                case BlockType.Water7: block = new Block(BlockType.Water); break;
                default:
                    block = new Block(BlockType.Water1);
                    newWaterBlock = true;
                    break;
            }
            //Log.WriteInfo ($"[Water.Add] Adding {pos}");
            worldInstance.SetBlock(pos.X, pos.Y, pos.Z, block);
            if (block.Type != BlockType.Water)
            {
                if (!_emptyCoords.Contains(pos))
                    _emptyCoords.Add(pos);

                var newScore = CalcScore(pos);
                if (!_heights.ContainsKey(pos))
                    _heights.Add(pos, newScore);
                if (newScore < _minScore)
                {
                    _minScore = newScore;
                    _minPos = pos;
                }
            }

            if (newWaterBlock)
            {
                var below = worldInstance.GetBlock (pos.X, pos.Y-1, pos.Z);
                if (below.Type == BlockType.Water)
                {
                    // Mark so we don't render it
                    // TODO - check surrounding blocks
                    worldInstance.SetBlock(pos.X, pos.Y-1, pos.Z, new Block(BlockType.UnderWater));
                }
                var above = worldInstance.GetBlock (pos.X, pos.Y+1, pos.Z);
                if (above.IsWater) {
                    worldInstance.SetBlock (pos.X, pos.Y, pos.Z, new Block (BlockType.Water));
                    _heights.Remove(pos);
                    _emptyCoords.Remove(pos);
                }

//                if (worldInstance.GetBlock(pos.X-1, pos.Y - 1, pos.Z).IsWater
//                    && worldInstance.GetBlock(pos.X+1, pos.Y - 1, pos.Z).IsWater
//                    && worldInstance.GetBlock(pos.X, pos.Y - 1, pos.Z-1).IsWater
//                    && worldInstance.GetBlock(pos.X, pos.Y - 1, pos.Z+1).IsWater)
//                {
//                    // Lake detected
//                }
            }

            ClearBlockAboveWater(pos.X, pos.Y+1, pos.Z);
            ClearBlockAboveWater(pos.X, pos.Y+2, pos.Z);
            ClearBlockAboveWater(pos.X+1, pos.Y+1, pos.Z);
            ClearBlockAboveWater(pos.X-1, pos.Y+1, pos.Z);
            ClearBlockAboveWater(pos.X, pos.Y+1, pos.Z+1);
            ClearBlockAboveWater(pos.X, pos.Y+1, pos.Z-1);
            ClearBlockAboveWater(pos.X+1, pos.Y+1, pos.Z+1);
            ClearBlockAboveWater(pos.X+1, pos.Y+1, pos.Z-1);
            ClearBlockAboveWater(pos.X-1, pos.Y+1, pos.Z+1);
            ClearBlockAboveWater(pos.X-1, pos.Y+1, pos.Z-1);

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
        private void ClearBlockAboveWater(int x,int y,int z)
        {
            var airBlock = new Block(BlockType.Air);
            var above = worldInstance.GetBlock (x, y, z);
            if (above.Type == BlockType.Dirt || above.Type == BlockType.Grass) {
                worldInstance.SetBlock (x, y, z, airBlock);
            }
        }

        private void AddIfEmpty(int x,int y,int z)
        {
            if (Source.GetDistanceExact (new Position (x, y, z)) > _maxLength) {
                // Temporary code to limit length of river
                Log.WriteInfo ($"[Water.AddIfEmpty] River limit reached");
                Growing = false;
                return;
            }

            if (!worldInstance.IsValidBlockLocation (x, y, z)) {
                // Have reached edge of map
                Log.WriteInfo ($"[Water.AddIfEmpty] Reached edge of map");
                Growing = false;
                return;
            }
                
            var block = worldInstance.GetBlock (x, y, z);
            if (block.IsWater){// || y < Global.waterLevel) {
                if (!Coords.Contains (new Position (x, y, z))) {
                    // Have reached another river or the ocean
                    Log.WriteInfo ($"[Water.AddIfEmpty] Met another river or ocean");
                    Growing = false;
                }
                return;
            }
            //if (!block.IsSolid)
            if (CanPlaceWater(block))
            {
                var pos = new Position(x,y,z);
                if (!_emptyCoords.Contains(pos))
                    _emptyCoords.Add(pos);

                var newScore = CalcScore(pos);
                if (!_heights.ContainsKey(pos))
                    _heights.Add(pos, newScore);
                if (newScore < _minScore)
                {
                    _minScore = newScore;
                    _minPos = pos;
                }

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
        private bool CanPlaceWater(Block block)
        {
            return block.Type == BlockType.Air;// || block.Type == BlockType.Dirt || block.Type == BlockType.Grass;
        }
        private float CalcScore(Position pos)
        {
            float score;
            try
            {
                float neighbours = 0;
                var h =  worldInstance.GetBlockHeight(pos.X,pos.Z);
                for (var i=1;i<8;i++)
                {
                    neighbours += TestCalcScore(h, i, pos.X+i,pos.Z);
                    neighbours += TestCalcScore(h, i, pos.X-i,pos.Z);
                    neighbours += TestCalcScore(h, i, pos.X,pos.Z+i);
                    neighbours += TestCalcScore(h, i, pos.X,pos.Z-i);
                    neighbours += TestCalcScore(h, i, pos.X+i,pos.Z+i);
                    neighbours += TestCalcScore(h, i, pos.X+i,pos.Z-i);
                    neighbours += TestCalcScore(h, i, pos.X-i,pos.Z+i);
                    neighbours += TestCalcScore(h, i, pos.X-i,pos.Z-i);
                }

                var block = worldInstance.GetBlock (pos.X, pos.Y, pos.Z);
                float current = pos.Y;
                if (block.IsWater)
                {
                    current = current + ((float)block.WaterHeight / 8);
                }
                var below = worldInstance.GetBlock (pos.X, pos.Y-1, pos.Z);
                if (!below.IsSolid)
                    current -= 0.2f;

                score = (current + (neighbours / 64) ) / Global.CHUNK_HEIGHT;
                //Log.WriteInfo($"[Water.CalcScore] Position {pos} = {score}");
            }
            catch (Exception) { // TODO Handle out of array bounds errors better
                score = pos.Y / Global.CHUNK_HEIGHT;
            }
            return score;
        }
        private float TestCalcScore(int h, int i, int x,int z)
        {
            int y = worldInstance.GetBlockHeight(x,z);
            var block = worldInstance.GetBlock(x, y, z);
            while (CanPlaceWater(block) && y>0)
            {
                y--;
                block = worldInstance.GetBlock(x, y, z);
            }
            y++; // location to place water
            float waterHeight = 0;
            if (block.IsWater)
            {
                y--;
                waterHeight = (float)block.WaterHeight / 8;
            }
            //if (y >= h)
            //    return 0.0f;
            return ((float)(y - h) + waterHeight) / (float)(Math.Pow(2,i));
        }
        private void FindNextLowest()
        {
            _minScore = Global.CHUNK_HEIGHT;
            foreach (var pos in _emptyCoords)
            {
                var score = _heights[pos];
                if (score < _minScore)
                {
                    _minScore = score;
                    _minPos = pos;
                }
            }
        }
        /*
        // Find next lowest, which is also next to current min
        private void FindNextLowest()
        {
            var potentials = new List<Position>();
            _minScore = Global.CHUNK_HEIGHT;
            foreach(var pos in _emptyCoords)
            {
                var score = _heights[pos];
                if (score < _minScore)
                {
                    potentials.Clear();
                    potentials.Add(pos);
                    _minScore = score;
                }
                else if (score == _minScore)
                {
                    potentials.Add(pos);
                }
            }
            if (potentials.Count == 0)
                return;
            foreach(var pos in potentials)
            {
                if (Math.Abs(pos.X - _minPos.X) == 1
                    || Math.Abs(pos.Y - _minPos.Y) == 1
                    || Math.Abs(pos.Z - _minPos.Z) == 1)
                {
                    _minPos = pos;
                    return;
                }
            }
            _minPos = potentials[0];
        }
        */
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
                var river = new River(worldInstance);
                var chunkCoords = new ChunkCoords (river.Source);
                Log.WriteInfo ($"Creating river from chunk {chunkCoords}");
                Rivers.Add(river);
            }
        }

        private void StartThread()
        {
            try
            {
                Thread.Sleep(7000);
                CreateRiver();
                bool growing = true;
                while (growing)
                {
                    if (Rivers.Count != 0)
                    {
                        growing = false;
                        foreach (var river in Rivers)
                        {
                            river.Grow();
                            growing |= river.Growing;
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex) {
                Log.WriteError ($"Water thread crashed - {ex.Message}");
            }
        }
    }
}

