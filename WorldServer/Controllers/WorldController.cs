﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;

namespace Sean.WorldServer
{
    public class WorldController : ApiController
    {
        [HttpPost]
        [Route("Player/{playerId}/LookingAt?x={x}&y={y}")]
        public async Task<bool> LookingAt(int playerId, int x, int z)
        {
            Console.WriteLine ("Player {0} at {1},{2}", playerId, x,z);
            return true;
        }

        [HttpGet]
        [Route("Player/{playerId}/ObjectId/{objectId}")]
        public async Task<string> QueryObject(int playerId, int objectId)
        {
            return "hello";
        }

        [HttpPost]
        [Route("Player/{playerId}/ObjectId?Action={action}")]
        public async Task<bool> DoAction(int playerId, int objectId, Action action)
        {
            return true;
        }
    }
}