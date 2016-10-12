using System;
using Sean.Shared;
using System.Collections.Generic;
using C5;
using System.Text;

namespace Sean.Shared
{
    public class BlocksColumn
    {
        private ArrayList<Tuple<ushort,int>> _array;
        private int _chunkHeight;

        public BlocksColumn(int chunkHeight)
        {
            _chunkHeight = chunkHeight;
            _array = new ArrayList<Tuple<ushort,int>>();
            _array.Add (new Tuple<ushort,int> ((ushort)Block.BlockType.Air, chunkHeight));
        }
        public Block this[int y]
        {
            get { return GetBlock(y); }
            set { SetBlock(y, value); }
        }
        private Block GetBlock(int y)
        {
            int height = 0;
            ushort blockType = 0;
            foreach (var tuple in _array) {
                height += tuple.Item2;
                if (height >= y) {
                    return new Block (tuple.Item1);
                }
            }
            throw new ApplicationException ($"[BlocksColumn.GetBlock] {y} out of bounds");
        }
        private void SetBlock(int y, Block block)
        {
            int height = 0;
            for (int index = 0; index < _array.Count; index++) {
                if (height + _array[index].Item2 >= y) {
                    if (_array [index].Item2 == 1) {
                        // Replace block
//                        _array [index].Item1 = block.BlockData;
                        break;
                    } else if (height + 1 == y) {
//                        _array[index].Item2 --;
                        _array.Insert (index - 1, new Tuple<ushort, int> (block.BlockData, 1));
                        break;
                    } else if (height + _array [index].Item2 == y) {
//                        _array[index].Item2 --;
                        _array.Insert (index, new Tuple<ushort, int> (block.BlockData, 1));
                        break;
                    } else {
                        var h = _array [index].Item2;
//                        _array[index].Item2 = y - height;
                        _array.Insert (index, new Tuple<ushort, int> (block.BlockData, 1));
//                        _array.Insert (index, new Tuple<ushort, byte> (block.BlockData, y - (h + height)));
                        break;
                    }
                }
                height += _array[index].Item2;
            }
        }
        public override string ToString()
        {
            var chars = new char[5]{'_','~','#','@','%'};
            StringBuilder str = new StringBuilder ();
            int y = 0;
            int height = 0;
            foreach (var tuple in _array) {
                height += tuple.Item2;
                while (y++ < height) {
                    str.Append (chars[tuple.Item1 % 5]);
                }
            }
            return str.ToString ();
        }
    }

    /// <summary>
    /// Note that the data is stored as [Y,X,Z]. When we Buffer.BlockCopy, we want the data accessed Y-by-Y. This improves compression and results in a ~10% smaller world
    /// </summary>
    public class Blocks // TODO - merge with shared
    {
        private const int CHUNK_SIZE = 32; // TODO - move
        private const int CHUNK_HEIGHT = 128; // TODO - move
        public readonly BlocksColumn[,] _array;

        public Blocks(int chunkSizeX, int chunkHeight, int chunkSizeZ)
        {
            _array = new BlocksColumn[chunkSizeX, chunkSizeZ];
            for (int x = 0; x < chunkSizeX; x++) {
                for (int z = 0; x < chunkSizeZ; z++) {
                    _array [x, z] = new BlocksColumn (chunkHeight);
                }
            }
        }

        public Block this[Coords coords]
        {
            get { return _array[coords.Xblock % CHUNK_SIZE, coords.Zblock % CHUNK_SIZE][coords.Yblock]; }
            set { _array[coords.Xblock % CHUNK_SIZE, coords.Zblock % CHUNK_SIZE][coords.Yblock] = value; }
        }

        public Block this[Position position]
        {
            get { return _array[position.X % CHUNK_SIZE, position.Z % CHUNK_SIZE][position.Y]; }
            set { _array[position.X % CHUNK_SIZE, position.Z % CHUNK_SIZE][position.Y] = value; }
        }

        /// <summary>Get a block from the array.</summary>
        /// <param name="x">Chunk relative X.</param>
        /// <param name="y">Chunk relative Y.</param>
        /// <param name="z">Chunk relative Z.</param>
        public Block this[int x, int y, int z]
        {
            get { return _array[x,z][y]; }
            set { _array[x,z][y] = value; }
        }
    }
}

