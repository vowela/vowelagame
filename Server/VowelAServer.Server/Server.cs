using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ENet;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Controllers;
using VowelAServer.Server.Models;
using VowelAServer.Server.Net;
using VowelAServer.Shared.Gameplay;
using VowelAServer.Shared.Networking;
using VowelAServer.Shared.Utils;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Server
{
    class Server
    {
        public static List<ITickable> Tickables;
        public static readonly Host HostInstance = new Host();

        private static void InitTickables()
        {
            Tickables = new List<ITickable>
            {
                new WorldSimulation()
            };
        }

        static void Main(string[] args)
        {
            InitTickables();
            RPCManager.GetOrBuildLookup();

            const ushort port = 6005;
            const int maxClients = 100;
            Library.Initialize();

            var address = new Address { Port = port };
            HostInstance.Create(address, maxClients);

            Logger.WriteSuccess($"Circle ENet Server started on {port}");

            var continueThread = true;
            while (continueThread)
            {
                // Ticking gameplay logic
                foreach (var tickable in Tickables)
                {
                    tickable.Tick();
                }

                NetEventPoll.CheckPoll();
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape) continueThread = false;
                }
            }
            HostInstance.Flush();

            Library.Deinitialize();
        }
    }
}