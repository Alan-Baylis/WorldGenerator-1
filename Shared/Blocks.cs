using System;
using Sean.Shared;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace Sean.Shared
{
    public class BlocksColumn
    {
        private ArrayList _array;
        private int _chunkHeight;

        private class ArrayItem
        {
            public ArrayItem(Block.BlockType blockType, int count)
            {
                this.BlockType = blockType;
                this.Count = count;
            }
            public Block.BlockType BlockType;
            public int Count; // TODO - fix to byte size
        }
        public BlocksColumn(int chunkHeight)
        {
            _chunkHeight = chunkHeight;
            _array = new ArrayList();
            _array.Add (new ArrayItem(Block.BlockType.Unknown, chunkHeight));
        }
        public Block this[int y]
        {
            get { return GetBlock(y); }
            set { SetBlock(y, value); }
        }
        private ArrayItem GetArrayItem(int index)
        {
            return (ArrayItem)_array[index];
        }
        private Block GetBlock(int y)
        {
            int height = -1;
            foreach (ArrayItem item in _array) {
                height += item.Count;
                if (height >= y) {
                    return new Block (item.BlockType);
                }
            }
            throw new ApplicationException ($"[BlocksColumn.GetBlock] {y} out of bounds");
        }
        private void SetBlock(int y, Block block)
        {
            int height = -1;
            for (int index = 0; index < _array.Count; index++) {
                var item = GetArrayItem(index);
                if (height + item.Count >= y) {
                    if (item.Count == 1)
                    {
                        // Replace block
                        item.BlockType = block.Type;
                    }
                    else if (height+1 == y)
                    {
                        // At start of sequence
                        item.Count--;
                        item = new ArrayItem(block.Type, 1);
                        _array.Insert(index, item);
                    }
                    else if (height + item.Count == y)
                    {
                        // At end of sequence
                        item.Count--;
                        item = new ArrayItem(block.Type, 1);
                        index++;
                        _array.Insert(index, item);
                    }
                    else
                    {
                        // mid sequence
                        var h = item.Count;
                        item.Count = y - height;
                        var lowerItem = new ArrayItem(item.BlockType, y - height - 1);
                        _array.Insert(index, lowerItem);
                        var upperItem = item;
                        upperItem.Count = height + h - y;
                        item = new ArrayItem(block.Type, 1);
                        index++;
                        _array.Insert(index, new ArrayItem(block.Type, 1));
                    }

                    if (index + 1 < _array.Count)
                    {
                        var nextItem = GetArrayItem(index + 1);
                        if (nextItem.BlockType == item.BlockType) // Combine with above
                        {
                            item.Count += nextItem.Count;
                            _array.Remove(nextItem);
                        }
                    }
                    if (index - 1 >= 0)
                    {
                        var prevItem = GetArrayItem(index - 1);
                        if (prevItem.BlockType == item.BlockType) // Combine with below
                        {
                            prevItem.Count += item.Count;
                            _array.Remove(item);
                        }
                    }
                    break;
                }
                height += item.Count;
            }
        }

        public IEnumerable<Tuple<int, Block.BlockType>> GetVisibleIterator()
        {
            int h = 0;
            int height = 0;
            foreach (ArrayItem item in _array)
            {
                height += item.Count;
                if (item.BlockType == Block.BlockType.Unknown || item.BlockType == Block.BlockType.Air)
                    h = height;
                while (h < height)
                {
                    yield return new Tuple<int, Block.BlockType> (h, item.BlockType);
                    h++;
                }
            }
        }
        public byte[] Serialize()
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                foreach (ArrayItem item in _array)
                {
                    memoryStream.WriteByte((byte)(((ushort)item.BlockType) / 256));
                    memoryStream.WriteByte((byte)(((ushort)item.BlockType) % 256));
                    memoryStream.WriteByte((byte)item.Count);
                }
                return memoryStream.ToArray();
            }
        }

        public void Deserialize(MemoryStream memoryStream)
        {
            _array = new ArrayList();
            int height = 0;
            while (height < _chunkHeight)
            {
                //var blockType = (Block.BlockType)(memoryStream.ReadByte() << 8 + memoryStream.ReadByte());
                var a = memoryStream.ReadByte();
                var b = memoryStream.ReadByte();
                var blockType = (Block.BlockType)((a << 8) + b);
                var count = memoryStream.ReadByte();
                _array.Add (new ArrayItem(blockType, count));
                height += count;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder ();
            foreach (ArrayItem item in _array)
            {
                str.Append($"{(ushort)(item.BlockType)}x{item.Count},");
            }
            return str.ToString ();
        }
        public string Render()
        {
            var chars = new char[5] { '_', '~', '#', '+', '@' };
            StringBuilder str = new StringBuilder();
            int h = 0;
            int height = 0;
            foreach (ArrayItem item in _array)
            {
                height += item.Count;
                while (h < height)
                {
                    str.Append(chars[(int)item.BlockType % 5]);
                    h++;
                }
            }
            return str.ToString();
        }
    }

    public class Blocks 
    {
        public readonly BlocksColumn[,] _array;

        public Blocks()
        {
            _array = new BlocksColumn[Global.CHUNK_SIZE, Global.CHUNK_SIZE];
            for (int x = 0; x < Global.CHUNK_SIZE; x++) {
                for (int z = 0; z < Global.CHUNK_SIZE; z++) {
                    _array [x, z] = new BlocksColumn (Global.CHUNK_HEIGHT);
                }
            }
        }

        public Block this[Coords coords]
        {
            get { return _array[coords.Xblock % Global.CHUNK_SIZE, coords.Zblock % Global.CHUNK_SIZE][coords.Yblock]; }
            set { _array[coords.Xblock % Global.CHUNK_SIZE, coords.Zblock % Global.CHUNK_SIZE][coords.Yblock] = value; }
        }

        public Block this[Position position]
        {
            get { return _array[position.X % Global.CHUNK_SIZE, position.Z % Global.CHUNK_SIZE][position.Y]; }
            set { _array[position.X % Global.CHUNK_SIZE, position.Z % Global.CHUNK_SIZE][position.Y] = value; }
        }

        private IEnumerable<Tuple<Position, Block.BlockType>> GetVisibleIterator(int x, int z)
        {
            foreach (var item in _array[x,z].GetVisibleIterator()) {
                var y = item.Item1;
                var block = item.Item2;
                yield return new Tuple<Position, Block.BlockType> (
                    new Position (x, y, z), block);
            }
        }
        public IEnumerable<Tuple<Position, Block.BlockType>> GetVisibleIterator(Facing direction)
        {
            switch (direction) {
            case Facing.North:
                for (var x = 0; x < Global.CHUNK_SIZE; x++) {
                    for (var z = 0; z < Global.CHUNK_SIZE; z++) {
                        foreach (var item in GetVisibleIterator (x, z)) {
                            yield return item;
                        }
                    }
                }
                break;
            case Facing.South:
                for (var x = Global.CHUNK_SIZE - 1; x >= 0; x--) {
                    for (var z = Global.CHUNK_SIZE - 1; z >= 0; z--) {
                        foreach (var item in GetVisibleIterator (x, z)) {
                            yield return item;
                        }
                    }
                }
                break;
            case Facing.East:
                for (var x = Global.CHUNK_SIZE - 1; x >= 0; x--) {
                    for (var z = 0; z < Global.CHUNK_SIZE; z++) {
                        foreach (var item in GetVisibleIterator (x, z)) {
                            yield return item;
                        }
                    }
                }
                break;
            case Facing.West:
                for (var x = 0; x < Global.CHUNK_SIZE; x++) {
                    for (var z = Global.CHUNK_SIZE - 1; z >= 0; z--) {
                        foreach (var item in GetVisibleIterator (x, z)) {
                            yield return item;
                        }
                    }
                }
                break;
            }
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

        public byte[] Serialize()
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                for (var x = 0; x < Global.CHUNK_SIZE; x++)
                {
                    for (var z = 0; z < Global.CHUNK_SIZE; z++)
                    {
                        var column = _array[x, z].Serialize();
                        memoryStream.Write(column,0,column.Length);
                    }
                }
                return memoryStream.ToArray();
            }
        }

        public void Deserialize(byte[] data)
        {
            using (var memoryStream = new System.IO.MemoryStream(data))
            {
                for (var x = 0; x < Global.CHUNK_SIZE; x++)
                {
                    for (var z = 0; z < Global.CHUNK_SIZE; z++)
                    {
                        _array[x, z].Deserialize(memoryStream);
                    }
                }
            }
        }
    }
}

