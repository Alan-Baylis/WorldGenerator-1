using System;

namespace CmdLineClient
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine ("Hello World!");

            var worldClient = new WorldClient("http://localhost:8080", 1);
            Location cursor = new Location (100,0,100);
            int stepSize = 5;
            while (true)
            {
                worldClient.LookingAt (cursor);
                Console.Clear ();
                //var chunk = WorldData.WorldMap.Chunk (cursor);
                //chunk.Render (y);
                var key = Console.ReadKey ();
                switch (key.KeyChar)
                {
                case 'q': cursor.Z -= stepSize; break;
                case 'a': cursor.Z += stepSize; break;
                case 'p': cursor.X += stepSize; break;
                case 'o': cursor.X -= stepSize; break;
                case '>': cursor.Y += stepSize; break;
                case '<': cursor.Y -= stepSize; break;
                }
            }

            //WriteStatusCodeResult(result);

        }

        static void WriteStatusCodeResult(System.Net.HttpStatusCode statusCode)
        {
            if(statusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Opreation Succeeded - status code {0}", statusCode);
            }
            else
            {
                Console.WriteLine("Opreation Failed - status code {0}", statusCode);
            }
            Console.WriteLine("");
        }
    }

}
