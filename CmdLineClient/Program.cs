using System;

namespace CmdLineClient
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine ("Hello World!");



            int x = WorldData.WorldMap.MaxXPosition / 2;
            int y = 0;
            int z = WorldData.WorldMap.MaxZPosition / 2;

            var cursor = new Position (x, y, z);
            while (true)
            {
                Console.Clear ();
                var chunk = WorldData.WorldMap.Chunk (cursor);
                chunk.Render (y);
                var key = Console.ReadKey ();
                switch (key.KeyChar)
                {
                case 'q': cursor.Z -= Chunk.CHUNK_SIZE; break;
                case 'a': cursor.Z += Chunk.CHUNK_SIZE; break;
                case 'p': cursor.X += Chunk.CHUNK_SIZE; break;
                case 'o': cursor.X -= Chunk.CHUNK_SIZE; break;
                case '>': cursor.Y += Chunk.CHUNK_SIZE; break;
                case '<': cursor.Y -= Chunk.CHUNK_SIZE; break;
                }
            }




            Console.WriteLine("Read all the companies...");
            var companyClient = new CompanyClient("http://localhost:8080");

            int nextId  = (from c in companies select c.Id).Max() + 1;

            Console.WriteLine("Add a new company...");
            var result = companyClient.AddCompany(
                new Company 
                { 
                    Id = nextId, 
                    Name = string.Format("New Company #{0}", nextId) 
                });
            WriteStatusCodeResult(result);

        }

        static void WriteCompaniesList(IEnumerable<Company> companies)
        {
            foreach(var company in companies)
            {
                Console.WriteLine("Id: {0} Name: {1}", company.Id, company.Name);
            }
            Console.WriteLine("");
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
