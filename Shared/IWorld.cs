namespace Sean.Shared
{
    public interface IWorld
    {
        bool IsValidBlockLocation (int x, int y, int z);
        bool IsLoadedBlockLocation(Position position);
        bool IsLoadedBlockLocation(int x, int y, int z);

        bool IsLocationSolid(Position position);
        bool IsLocationTransparent(Position position);

        Block GetBlock(Position position);
        Block GetBlock(int x, int y, int z);
        int GetBlockHeight(int x, int z);
        int GetHeightMapLevel(int x, int z);

        void SetBlock(Position position, Block block);
        void SetBlock(int x, int y, int z, Block block);

        bool IsChunkLoaded(ChunkCoords coords);
        Chunk GetChunk(ChunkCoords coords);

        Array<byte> GlobalMap { get; }
        Array<byte> GlobalMapTerrain { get; }

        Position GetRandomLocationOnLoadedChunk();

        int LoadedChunkCount { get; }
    }
}
