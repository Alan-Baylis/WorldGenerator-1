using System;
using System.Threading;
using Sean.Shared;

namespace OpenTkClient
{
    public interface IServer
    {
        void Start ();
        void GetWorldMap();
        void ClearExistingChunks();
        void GetMap(int x, int z);
    }

    public class Server
    {
        private static Thread thread;
        private static Thread updateThread;
        private static IServer serverInstance;

        public static void Run(IServer server)
        {
            serverInstance = server;
            thread = new Thread(new ThreadStart(Start));
            thread.Start();
            updateThread = new Thread(new ThreadStart(UpdateStart));
            updateThread.Start();
        }

        private static void UpdateStart()
        {
            Console.WriteLine ("Server update thread starting");
            while (true) {
                serverInstance.GetWorldMap ();
                Thread.Sleep (60000);
            }
        }
        private static void Start()
        {
            Console.WriteLine ("Server thread starting");

            try
            {
                serverInstance.Start();

		        Thread.Sleep (2000);
                if (Global.ClearExistingChunks)
                    serverInstance.ClearExistingChunks();
				while(true)
				{
                    int x = Global.LookingAt.X / Global.CHUNK_SIZE;
					int z = Global.LookingAt.Z / Global.CHUNK_SIZE;
                    if (Global.Scale < 4)
                    {
                        serverInstance.GetMap(x, z);
                        serverInstance.GetMap(x + 1, z);
                        serverInstance.GetMap(x - 1, z);
                        serverInstance.GetMap(x, z + 1);
                        serverInstance.GetMap(x, z - 1);
                        serverInstance.GetMap(x + 1, z + 1);
                        serverInstance.GetMap(x + 1, z - 1);
                        serverInstance.GetMap(x - 1, z + 1);
                        serverInstance.GetMap(x - 1, z - 1);
                    }
					Thread.Sleep (2000);
				}
            }
            catch (Exception e)
            {
               Console.WriteLine($"Server thread crashed - {e.Message}");
            }
        }
    }
}
