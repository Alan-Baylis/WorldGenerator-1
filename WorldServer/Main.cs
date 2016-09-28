using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Hosting;
using Sean.WorldGenerator;

namespace Sean.WorldServer
{
    static class MainClass
	{
		static public void Main(String[] args)
		{
			Console.WriteLine("World Server...");

            /*
            // Start OWIN host 
            string baseAddress = "http://localhost:9000/"; 
            using (WebApp.Start<Startup>(url: baseAddress)) 
            { 
                HttpClient client = new HttpClient(); 
                var response = client.GetAsync(baseAddress + "api/Test").Result; // Test call
                Console.WriteLine(response); 
                Console.WriteLine(response.Content.ReadAsStringAsync().Result); 
            }
            Console.WriteLine("Press any key to continue");
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
            Console.WriteLine ();
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
            //ClientSocket.SendMessage ();

            // Rest web server
            //string baseUri = "http://localhost:8085";
            //Console.WriteLine("Starting web Server...");
            //WebApp.Start<WebHostStartup>(baseUri);
            //Console.WriteLine("Server running at {0}", baseUri);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            Console.WriteLine("Shutting down");
            ServerSocketListener.Stop();
            WebSocketListener.Stop();
            WebServerListener.Stop();
		}
	}
}
