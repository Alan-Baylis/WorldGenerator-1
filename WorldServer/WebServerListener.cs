using System;
using System.Threading;
using System.Text;
using System.Net;
using WebSocketSharp.Server; // TODO - switch to standard httpServer ?
using WebSocketSharp;
using Sean.Shared;

namespace Sean.WorldServer
{
    public class WebServerListener
    {
        private static Thread thread;
        public WebServerListener()
        {
        }
            
        public static void Run() 
        {
            thread = new Thread (new ThreadStart (StartListening));
            thread.Start ();
        }
        public static void Stop()
        {
            thread.Abort();
        }
        private static void StartListening() 
        {
            try 
            {
                var ServerListeningPort = 8080;
                var httpsv = new HttpServer (ServerListeningPort);
                Log.WriteInfo($"Webserver waiting for a connection on port {ServerListeningPort}...");
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
                    //Log.WriteInfo ("Listening on port {0}", httpsv.Port);
                    foreach (var path in httpsv.WebSocketServices.Paths)
                        Log.WriteInfo($"- {path}");
                }
                
                while (true)
                {}

                //httpsv.Stop ();
                //Log.WriteInfo("Ending Listening Server");

            }
            catch (Exception e) 
            {
                Log.WriteInfo($"WebServerListener thread crashed - {e.Message}");
            }
        }
    }
}

