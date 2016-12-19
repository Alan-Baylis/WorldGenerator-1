using Sean.Shared;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Sean.WorldGenerator
{
    public class River
    {
        private IWorld worldInstance;
        public List<Shared.Position> Coords { get; set; }
        public Position Source { get; set; }

        private List<Shared.Position> _emptyCoords;
        private Dictionary<Shared.Position, float> _heights;
        private float _minScore;
        private Position _minPos;

        private const int WATER = 2; // TODO - stick these somewhere
        private const int GRASS = 4;
        private const int RIVER = 5;

        public River(IWorld world, Position source)
        {
            worldInstance = world;
            var _minPos = new Position(0, Global.CHUNK_HEIGHT, 0);
            _minScore = Global.CHUNK_HEIGHT;
            Coords = new List<Position>();
            _emptyCoords = new List<Position>();
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
            if (_emptyCoords.Contains(pos))
                _emptyCoords.Remove(pos);
            if (_heights.ContainsKey(pos))
                _heights.Remove(pos);

            var block = new Block(Block.BlockType.Water1);
            worldInstance.SetBlock(pos.X, pos.Y, pos.Z, block);
            worldInstance.GlobalMap.Set(pos.X, pos.Z, RIVER);

            AddIfEmpty(pos.X, pos.Y - 1, pos.Z);
            AddIfEmpty(pos.X+1, pos.Y, pos.Z);
            AddIfEmpty(pos.X-1, pos.Y, pos.Z);
            AddIfEmpty(pos.X, pos.Y, pos.Z+1);
            AddIfEmpty(pos.X, pos.Y, pos.Z-1);
            AddIfEmpty(pos.X, pos.Y + 1, pos.Z);

            if (_minScore >= score)
                FindNextLowest();
        }
        
        private void AddIfEmpty(int x,int y,int z)
        {
            if (!worldInstance.GetBlock(x, y, z).IsSolid)
            {
                var pos = new Position(x,y,z);
                _emptyCoords.Add(pos);
                CalcScore(pos);
            }
        }
        private void CalcScore(Position pos)
        {
            var chunk = new ChunkCoords(pos);
            var loc = chunk.NormLocOnChunk(pos);
            var a = worldInstance.GlobalMap[chunk.X+1,chunk.Z] * (1-loc.Item1);
            var b = worldInstance.GlobalMap[chunk.X-1,chunk.Z] * loc.Item1;
            var c = worldInstance.GlobalMap[chunk.X,chunk.Z+1] * (1-loc.Item2);
            var d = worldInstance.GlobalMap[chunk.X,chunk.Z-1] * loc.Item2;
            float score = (a+b+c+d) / 4;
            _heights.Add(pos, pos.Y + (score / Global.CHUNK_HEIGHT));
            if (score < _minScore)
            {
                _minScore = score;
                _minPos = pos;
            }
        }
        private void FindNextLowest()
        {
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

                pos.Y = 255;
                var b = worldInstance.GetBlock(pos);
                while (b.IsTransparent)
                {
                    pos.Y--;
                    b = worldInstance.GetBlock(pos);
                }
                var river = new River(worldInstance, pos);
                Rivers.Add(river);
            }
        }

        private void StartThread()
        {
            while (worldInstance.LoadedChunkCount == 0)
            {
                System.Threading.Thread.Sleep(5000);
            }
            CreateRiver();
            while (true)
            {
                foreach (var river in Rivers)
                {
                    river.Grow();
                }
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}

