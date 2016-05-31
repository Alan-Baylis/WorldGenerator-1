using System;
using System.Collections.Generic;

namespace Sean.World
{

    public interface IWorldGenerator
    {
        void Generate();

        ChunkCoords GetChunkCoords(Position position);
        int GetChunkSize();

        Chunk GetChunk(ChunkCoords chunkCoords, int id);
        void ChunkIgnore(ChunkCoords chunkCoords, int id);

        void PutBlock (Position position, Block.BlockType blockType);
        //void PutItem (Coords location, GameItem item);
        //GameItem GetItem (Coords location, GameItem item);
    }
        
    public static class WorldsManager
    {
        private static IWorldGenerator world; // TODO just the 1 world for now
        static WorldsManager()
        {
            world = new WorldGenerator ();
        }
        public static IWorldGenerator GetInstance(int id)
        {
            return world;
        }
    }


    public class WorldEventArgs : EventArgs
    {
        Position blockLocation;
        ItemAction action;
        Block block;
    }     

    public enum ChunkEventTarget
    {
        Block,
        Item,
        Character,
        Lighting,
        Projectile,
        Sound,
        Message
    }
    public enum ItemAction
    {
        Add,
        Remove,
        Update       
    }
        
    public class WorldGenerator : IWorldGenerator
    {
        public delegate void WorldEventHandler(WorldEventArgs e);
        public event WorldEventHandler WorldEvents;

        private Dictionary<ChunkCoords, List<int> > registrations;

        public WorldGenerator ()
        {
            registrations = new Dictionary<ChunkCoords, List<int> > ();
        }

        public void Generate()
        {
            WorldData.WorldMap.Generate ();
        }

        public int GetChunkSize()
        {
            return Chunk.CHUNK_SIZE;
        }
        public ChunkCoords GetChunkCoords(Position position)
        {
            return new ChunkCoords (position.X / Chunk.CHUNK_SIZE, position.Z / Chunk.CHUNK_SIZE); 
        }
        public Chunk GetChunk(ChunkCoords chunkCoords, int id)
        {
            // Ensure chunk loaded or generated
            var chunk = WorldData.WorldMap.Chunk (chunkCoords.X, chunkCoords.Z);
            if (!registrations.ContainsKey (chunkCoords)) {
                registrations [chunkCoords] = new List<int> ();
            }
            registrations[chunkCoords].Add(id);

            return WorldData.WorldMap.Chunk(chunkCoords);
        }
        public void ChunkIgnore(ChunkCoords chunkCoords, int id)
        {
            var reg = registrations [chunkCoords];
            if (reg != null)
            {
                reg.Remove (id);
                if (reg.Count == 0) {
                    registrations.Remove (chunkCoords);
                }
            }
        }
            
        public void PutBlock(Position position, Block.BlockType blockType)
        {
            WorldData.PlaceBlock(position, blockType);
        }



    }
}

