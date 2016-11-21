namespace Sean.Shared
{
    public interface IWorld
    {
        bool IsValidBlockLocation(Position position);
        bool IsValidBlockLocation(int x, int y, int z);

        Block GetBlock(Position position);
        Block GetBlock(int x, int y, int z);

        void SetBlock(Position position, Block block);
        void SetBlock(int x, int y, int z, Block block);

        bool IsChunkLoaded(ChunkCoords coords);
        Chunk GetChunk(ChunkCoords coords);

        Array<byte> GlobalMap { get; }
        Array<byte> GlobalMapTerrain { get; }

    }
}
