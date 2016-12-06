using System;
using Sean.WorldGenerator;
using Sean.Shared;

namespace Sean.WorldServer
{
    static class MainClass
	{
        static public World WorldInstance = new World();

		static public void Main(String[] args)
		{
            Console.WriteLine("World Server starting...");

            var gui = new WorldServer.Gui();
            /*
            // Start OWIN host 
            string baseAddress = "http://localhost:9000/"; 
            using (WebApp.Start<Startup>(url: baseAddress)) 
            { 
                HttpClient client = new HttpClient(); 
                var response = client.GetAsync(baseAddress + "api/Test").Result; // Test call
                Log.WriteInfo(response); 
                Log.WriteInfo(response.Content.ReadAsStringAsync().Result); 
            }
            Log.WriteInfo("Press any key to continue");
            Console.ReadKey();
            */

            /*
            const int MIN_SURFACE_HEIGHT = Chunk.CHUNK_HEIGHT / 2 - 40; //max amount below half
            const int MAX_SURFACE_HEIGHT = Chunk.CHUNK_HEIGHT / 2 + 8;  //max amount above half
            var worldSize = new ArraySize()
            {
                minZ = 100,
                maxZ = 100 + Chunk.CHUNK_SIZE,
                minX = 100,
                maxX = 100 + Chunk.CHUNK_SIZE,
                minY = 0,
                maxY = 10,
                scale = 1,
                minHeight = MIN_SURFACE_HEIGHT,
                maxHeight = MAX_SURFACE_HEIGHT,
            };
            var data = PerlinNoise.GetIntMap(worldSize, 8);
            data.Render ();
            Log.WriteInfo ();
            */

            //var d = new Array<int> (new ArraySize(){minX=50, maxX=100, minZ=50, maxZ=100, scale=5});
            //foreach (var a in d.GetLines()) {
            //
            //}

            //var chunk = World.GetChunk(new ChunkCoords(100, 100), 1);
            //World.RenderMap();
            //var otpServer = new OtpServer();
            //otpServer.Start();

            ServerSocketListener.Run();
            WebSocketListener.Run ();
            WebServerListener.Run();
            GameThread.Run();
            //ClientSocket.SendMessage ();

            gui.Run();

            // Rest web server
            //string baseUri = "http://localhost:8085";
            //Log.WriteInfo("Starting web Server...");
            //WebApp.Start<WebHostStartup>(baseUri);
            //Log.WriteInfo("Server running at {0}", baseUri);

            // TODO - shutdown event
            while (gui.IsRunning)
            {
                System.Threading.Thread.Sleep(5000);
            }
            Log.WriteInfo("Shutting down");
            ServerSocketListener.Stop();
            WebSocketListener.Stop();
            WebServerListener.Stop();
		}
	}
}
