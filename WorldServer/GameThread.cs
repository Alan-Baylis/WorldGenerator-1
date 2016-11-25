﻿using System;
using System.Threading;
using System.Text;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Net;
using Sean.Shared.Comms;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldServer
{
    public class GameThread
    {
        private static Thread thread;

        public GameThread()
        {
        }

        public static void Run()
        {
            thread = new Thread(new ThreadStart(StartThread));
            thread.Start();
        }
        public static void Stop()
        {
            thread.Abort();
        }
       
        private static void StartThread()
        {
            try
            {
                while (true)
                {
                    CharacterManager.UpdateJobs ();
                    CharacterManager.MoveCharacters();
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError($"GameThread crashed - {ex.Message}");
            }
        }
       
    }

}

