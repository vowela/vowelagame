using System;
using System.Collections.Generic;
using System.IO;
using ENet;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Controllers;
using VowelAServer.Server.Managers;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Networking;
using VowelAServer.Utilities.Helpers;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Server.Net
{
    public static class NetEventPoll
    {
        public static void CheckPoll()
        {
            var polled = false;

            while (!polled)
            {
                if (Server.HostInstance.CheckEvents(out var eNetEvent) <= 0)
                {
                    if (Server.HostInstance.Service(15, out eNetEvent) <= 0)
                        break;

                    polled = true;
                }

                NetworkEvent networkEvent;
                switch (eNetEvent.Type)
                {
                    case EventType.Connect:
                        Logger.Write("Client connected - ID: " + eNetEvent.Peer.ID + ", IP: " + eNetEvent.Peer.IP);
                        eNetEvent.Peer.Timeout(32, 1000, 4000);
                        break;
                    case EventType.Timeout:
                        Logger.Write("Client timeout - ID: " + eNetEvent.Peer.ID + ", IP: " + eNetEvent.Peer.IP);
                        networkEvent = NetworkEvent.DisconnectReason;
                        break;
                    case EventType.Disconnect:
                        Logger.Write("Client disconnected - ID: " + eNetEvent.Peer.ID + ", IP: " + eNetEvent.Peer.IP);
                        networkEvent = NetworkEvent.DisconnectReason;
                        break;
                    case EventType.Receive:
                        //Logger.Write("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length
                        var readBuffer = new byte[eNetEvent.Packet.Length];
                        var readStream = new MemoryStream(readBuffer);
                        var reader = new BinaryReader(readStream);

                        readStream.Position = 0;
                        eNetEvent.Packet.CopyTo(readBuffer);

                        var player = (Guid.TryParse(reader.ReadString(), out var sid)
                            ? Player.GetPlayerBySID(sid)
                            : null) ?? Player.Undefined(eNetEvent.Peer);
                        
                        networkEvent = (NetworkEvent)reader.ReadByte();
                        
                        if (networkEvent == NetworkEvent.RPCStatic) CallRpcMethod(player, reader);
                        else if (networkEvent == NetworkEvent.RPC)  CallObjectRpcMethod(player, reader);

                        break;
                }

                if (eNetEvent.Type == EventType.Receive)
                    eNetEvent.Packet.Dispose();
            }
        }

        // TODO: Make these calls in shared project somehow
        private static void CallObjectRpcMethod(Player player, BinaryReader reader)
        {
            // Determine target name for class using object id
            var netObjectId = reader.ReadInt32();
            var netObject   = NetObject.FindNetObjectById(netObjectId);
            if (netObject != null) CallRpcMethod(player, reader, netObject.GetType().Name, netObject);
        }

        private static void CallRpcMethod(Player player, BinaryReader reader, string targetName = "", object caller = null)
        {
            // Find needed controller and method
            targetName     = string.IsNullOrEmpty(targetName) ? reader.ReadString() : targetName;
            var methodName = reader.ReadString();
            // Parse arguments data
            var argsCount  = reader.ReadInt32();
            var arguments  = new List<object>();
            for (var i = 0; i < argsCount; i++)
            {
                var argLength = reader.ReadInt32();
                var argData   = reader.ReadBytes(argLength);
                arguments.Add(SerializationHelper.DeserializeFromBytes(argData));
            }

            if (RPCManager.RPCMethods.TryGetValue((targetName, methodName), out var method))
            {
                // If arguments count not exact, pass player at the first argument
                if (method.GetParameters().Length == arguments.Count)
                    method.Invoke(caller, arguments.ToArray());
                else
                {
                    arguments.Insert(0, player);
                    method.Invoke(caller, arguments.ToArray());
                }
            }
        }
    }
}
