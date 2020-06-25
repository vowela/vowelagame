using System;
using System.Collections.Generic;
using ENet;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Authorization;
using VowelAServer.Server.Controllers;
using VowelAServer.Server.Net;
using VowelAServer.Shared.Interfaces;

namespace VowelAServer.Server
{
    class Server
    {
        public static List<ITickable> Tickables;
        public static readonly Host HostInstance = new Host();
        public static AuthController AuthController;
        public static LuaController LuaController;
        public static MenuController MenuController;
        public static ObjectsController ObjectsController;

        private static void InitTickables()
        {
            Tickables = new List<ITickable>
            {
                new WorldSimulation()
            };
        }

        private static void InitControllers()
        {
            AuthController    = new AuthController();
            LuaController     = new LuaController();
            MenuController    = new MenuController();
            ObjectsController = new ObjectsController();
        }

        static void Main(string[] args)
        {
            InitTickables();
            InitControllers();

            const ushort port = 6005;
            const int maxClients = 100;
            Library.Initialize();

            var address = new Address
            {
                Port = port
            };
            HostInstance.Create(address, maxClients);

            Console.WriteLine($"Circle ENet Server started on {port}");

            var continueThread = true;
            while (continueThread)
            {
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