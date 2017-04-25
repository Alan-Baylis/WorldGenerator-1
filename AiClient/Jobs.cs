using System.Collections.Generic;
using Sean.Shared;

namespace AiClient
{
    public class IJob
    {
        
    }

    class Build : IJob
    {
        Position roughPosition;
        Vector3 dimensions;
        Dictionary<Item, int> requiredItems;

        void Process()
        {
            FindLocation (roughPosition, dimensions);
        }

        void FindLocation(Position position, Vector3 dimensions)
        {
            
        }
    }

    class Gather : IJob
    {
        Item itemToGather;
    }

    class AddToStore : IJob
    {
        //Store storeLocation;
    }

    class Find : IJob
    {
    }
}
