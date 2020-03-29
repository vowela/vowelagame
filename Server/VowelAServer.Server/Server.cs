using System;
using System.Collections.Generic;
using System.IO;
using ENet;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Authorization;
using VowelAServer.Server.Controllers;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Data.Multiplayer;
using VowelAServer.Shared.Interfaces;

namespace VowelAServer.Server
{
    class Server
    {
        public static List<ITickable> Tickables;
        public static readonly Host HostInstance = new Host();
        public static AuthController AuthController;
        public static LuaController LuaController;

        private static void InitTickables()
        {
            Tickables = new List<ITickable>
            {
                new WorldSimulation()
            };
        }

        private static void InitControllers()
        {
            AuthController = new AuthController();
            LuaController = new LuaController();
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

            while (!Console.KeyAvailable)
            {
                foreach (var tickable in Tickables)
                {
                    tickable.Tick();
                }
                NetEventPoll.CheckPoll();
            }
            HostInstance.Flush();

            Library.Deinitialize();
        }
    }
}