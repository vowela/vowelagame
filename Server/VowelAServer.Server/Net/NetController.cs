using System;
using ENet;
using VowelAServer.Server.Models;

namespace VowelAServer.Server.Net
{
    public class NetController
    {
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
