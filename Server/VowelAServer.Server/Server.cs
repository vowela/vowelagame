using System;
using System.Collections.Generic;
using ENet;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Gameplay.Debugging;
using VowelAServer.Server.Controllers;
using VowelAServer.Server.Managers;
using VowelAServer.Server.Net;
using VowelAServer.Shared.Gameplay;
using VowelAServer.Utilities.Helpers;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Server
{
    class Server
    {
        public static readonly List<ITickable> Tickables = new List<ITickable>();
        public static readonly Host HostInstance = new Host();

        private static void InitTickables()
        {
            var tickables = typeof(ITickable).DerivedTypes();
            foreach (var tickable in tickables) Tickables.Add((ITickable)Activator.CreateInstance(tickable));
        }

        static void Main(string[] args)
        {
            // Make sure that Gameplay has been started
            var runner = new Runner();
            
            InitTickables();
            RPCManager.GetOrBuildLookup();

            const ushort port = 6005;
            const int maxClients = 100;
            Library.Initialize();

            var address = new Address { Port = port };
            HostInstance.Create(address, maxClients);

            Logger.WriteSuccess($"VowelA Game Server started on port: {port}");

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