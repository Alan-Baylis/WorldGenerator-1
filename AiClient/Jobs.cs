using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiClient
{
    public class IJob
    {
    }

    class Gather : IJob
    {
        Item itemToGather;
    }

    class AddToStore : IJob
    {
        Store storeLocation;
    }
}
