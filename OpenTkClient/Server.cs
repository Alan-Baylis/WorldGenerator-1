using System;
using System.Threading;
using Sean.Shared;

namespace OpenTkClient
{
    public interface IServer
    {
        void Start ();
        void GetWorldMap();
        void GetMap(int x, int z);
    }

    public class Server
    {
        private static Thread thread;
        private IServer server;

        public static void Run(IServer server)
        {
            this.server = server;
            thread = new Thread(new ThreadStart(Start));
            thread.Start();
        }

        private static void Start()
        {
            Console.WriteLine ("Server thread starting");

            try
            {
                server.Start();

		        Thread.Sleep (2000);

                server.GetWorldMap();
				while(true)
				{
                    int x = Global.LookingAt.X / Global.CHUNK_SIZE;
					int z = Global.LookingAt.Z / Global.CHUNK_SIZE;
                    if (Global.Scale < 4)
                    {
                        server.GetMap(x, z);
                        server.GetMap(x + 1, z);
                        server.GetMap(x - 1, z);
                        server.GetMap(x, z + 1);
                        server.GetMap(x, z - 1);
                        server.GetMap(x + 1, z + 1);
                        server.GetMap(x + 1, z - 1);
                        server.GetMap(x - 1, z + 1);
                        server.GetMap(x - 1, z - 1);
                    }
					Thread.Sleep (2000);
				}
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught in Server thread - {0}", e.ToString());
            }
        }
    }
}
