using System;
using System.Threading;
using WebSocketSharp;

namespace Sean.WorldServer
{
    public class WebSocketListener
    {
        public WebSocketListener ()
        {
        }
            
        public static void Run() 
        {
            Thread thread = new Thread (new ThreadStart (StartListening));
            thread.Start ();
        }
        private static void StartListening() 
        {
            try 
            {
                using (var ws = new WebSocket ("ws://127.0.0.1/")) 
                {
                    ws.OnMessage = e => {
                        Console.WriteLine ("Ws says: " + e.Data);
                    };

                    ws.Connect ();
                    ws.Send ("ws pong");
            
                }
        
                Console.WriteLine("Ending Listening Server");
            }
            catch (Exception e) 
            {
                Console.WriteLine("Exception caught in WebSocketListener - {0}", e.ToString());
            }
        }

    }
}

