﻿using System;
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
            _array.Add (new ArrayItem(Block.BlockType.Air, chunkHeight));
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
            int height = 0;
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
                var blockType = (Block.BlockType)(memoryStream.ReadByte() << 8 + memoryStream.ReadByte());
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

        public byte[] Serialize()
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                for (var x = 0; x < CHUNK_SIZE; x++)
                {
                    for (var z = 0; z < CHUNK_SIZE; z++)
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
                for (var x = 0; x < CHUNK_SIZE; x++)
                {
                    for (var z = 0; z < CHUNK_SIZE; z++)
                    {
                        _array[x, z].Deserialize(memoryStream);
                    }
                }
            }
        }
    }
}

