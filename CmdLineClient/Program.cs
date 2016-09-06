using System;

namespace CmdLineClient
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine ("Hello World!");
            if (args.Length > 0)
            {
                if (args[0] == "/client")
                {
                    Console.WriteLine("Start client");
                    SyncSocketClient.StartClient();
                }
                else if (args[0] == "/server")
                {
                    Console.WriteLine("Start listening");
                    SyncSocketListener.StartListening();
                }
            }
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

    }

}
