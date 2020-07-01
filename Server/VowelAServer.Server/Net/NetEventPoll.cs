using System;
using System.Collections.Generic;
using System.IO;
using ENet;
using VowelAServer.Server.Controllers;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Networking;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Server.Net
{
    public static class NetEventPoll
    {
        public static readonly Dictionary<int, NetController> NetControllers = new Dictionary<int, NetController>();
        
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
                        var reader     = new BinaryReader(readStream);

                        readStream.Position = 0;
                        eNetEvent.Packet.CopyTo(readBuffer);
                        networkEvent = (NetworkEvent)reader.ReadByte();

                        if (networkEvent == NetworkEvent.RPC) CallRpcMethod(reader);
                        break;
                }

                if (eNetEvent.Type == EventType.Receive)
                    eNetEvent.Packet.Dispose();
            }
        }

        private static void CallRpcMethod(BinaryReader reader)
        {
            // Find needed controller and method
            var netId = reader.ReadInt32();
            if (!NetControllers.TryGetValue(netId, out var netController)) return;
            
            var methodName = reader.ReadString();
            if (netController.RPCMethods.TryGetValue(methodName, out var method))
                method.Invoke();
        }
        
        public static void SendData(byte[] buffer)
        {
            var packet = default(Packet);
            packet.Create(buffer);
            Server.HostInstance.Broadcast(0, ref packet);
        }

        public static void SendData(byte[] buffer, ref Event netEvent)
        {
            var packet = default(Packet);
            packet.Create(buffer);
            netEvent.Peer.Send(0, ref packet);
        }
    }
}
