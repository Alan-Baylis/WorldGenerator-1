using System;
using System.Threading;
using System.Text;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Net;

namespace Sean.WorldServer
{
    // See http://sta.github.io/websocket-sharp/

    public class Laputa : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = e.Data == "BALUS"
                      ? "I've been balused already..."
                      : "I'm not available now.";

            Send(msg);
        }
    }

    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
        }
    }
    public class Chat : WebSocketBehavior
    {
        private string _suffix;

        public Chat()
          : this(null)
        {
        }

        public Chat(string suffix)
        {
            _suffix = suffix ?? String.Empty;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast(e.Data + _suffix);
        }
    }


    
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
                var wssv = new WebSocketServer("ws://localhost:8083");
                wssv.AddWebSocketService<Laputa>("/Laputa");
                wssv.AddWebSocketService<Echo>("/Echo");
                wssv.AddWebSocketService<Echo>("/WebSocket");
                wssv.AddWebSocketService<Chat>("/Chat");
                wssv.AddWebSocketService<Chat>("/ChatWithNyan", () => new Chat(" Nyan!"));
                wssv.Start();
               
     

                var ServerListeningPort = 8080;
                var httpsv = new HttpServer (ServerListeningPort);
                Console.WriteLine($"Webserver waiting for a connection on port {ServerListeningPort}...");
                //var httpsv = new HttpServer (5963, true);
                //var httpsv = new HttpServer (System.Net.IPAddress.Parse ("127.0.0.1"), 4649);
                //var httpsv = new HttpServer (System.Net.IPAddress.Parse ("127.0.0.1"), 5963, true);
                //var httpsv = new HttpServer ("http://localhost:4649");
                //var httpsv = new HttpServer ("https://localhost:5963");
                //#if DEBUG
                //// To change the logging level.
                //httpsv.Log.Level = LogLevel.Trace;

                //// To change the wait time for the response to the WebSocket Ping or Close.
                //httpsv.WaitTime = TimeSpan.FromSeconds (2);
                //#endif

                // To set the document root path.
                if (System.IO.Directory.Exists("../../../../Html5Client/"))
                    httpsv.RootPath = "../../../../Html5Client/"; 
                else
                    httpsv.RootPath = "../../Public/";

                // To set the HTTP GET method event.
                httpsv.OnGet += (sender, e) => {
                    var req = e.Request;
                    var res = e.Response;

                    var path = req.RawUrl;
                    if (path == "/")
                        path += "index.html";

                    var content = httpsv.GetFile (path);
                    if (content == null) {
                        res.StatusCode = (int) HttpStatusCode.NotFound;
                        return;
                    }

                    if (path.EndsWith (".html")) {
                        res.ContentType = "text/html";
                        res.ContentEncoding = Encoding.UTF8;
                    }

                    res.WriteContent (content);
                };

                // Not to remove the inactive WebSocket sessions periodically.
                //httpsv.KeepClean = false;

                // To resolve to wait for socket in TIME_WAIT state.
                //httpsv.ReuseAddress = true;

                // Add the WebSocket services.
                //httpsv.AddWebSocketService<Echo> ("/Echo");
                //httpsv.AddWebSocketService<WebSocketSession> ("/WebSocket");

                httpsv.Start ();
                if (httpsv.IsListening) {
                    Console.WriteLine ("Listening on port {0}", httpsv.Port);
                    foreach (var path in httpsv.WebSocketServices.Paths)
                        Console.WriteLine ("- {0}", path);
                }
                
                while (true)
                {}

                //httpsv.Stop ();
                //Console.WriteLine("Ending Listening Server");

            }
            catch (Exception e) 
            {
                Console.WriteLine("Exception caught in WebSocketListener - {0}", e.ToString());
            }
        }
    }

    /*
    public class WebSocketSession : WebSocketBehavior
    {
        protected override void OnMessage (MessageEventArgs e)
        {
            Console.WriteLine("[OnMessage]");
            var name = Context.QueryString["name"];
            var msg = !name.IsNullOrEmpty () ? String.Format ("'{0}' to {1}", e.Data, 
                name) : e.Data;
            Send (msg);
        }
    }
    */
    
}

