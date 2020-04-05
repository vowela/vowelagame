using System;
using System.IO;
using ENet;
using VowelAServer.Shared.Data.Multiplayer;

namespace VowelAServer.Server.Net
{
    public class NetEventPoll
    {
        public static event EventHandler<PacketId> ServerEventHandler;

        public static void CheckPoll()
        {
            bool polled = false;

            while (!polled)
            {
                if (Server.HostInstance.CheckEvents(out Event netEvent) <= 0)
                {
                    if (Server.HostInstance.Service(15, out netEvent) <= 0)
                        break;

                    polled = true;
                }

                var packetId = PacketId.None;
                switch (netEvent.Type)
                {
                    case EventType.Connect:
                        Console.WriteLine("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        netEvent.Peer.Timeout(32, 1000, 4000);
                        break;
                    case EventType.Timeout:
                        Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        packetId = PacketId.LogoutEvent;
                        break;
                    case EventType.Disconnect:
                        Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        packetId = PacketId.LogoutEvent;
                        break;
                    case EventType.Receive:
                        //Console.WriteLine("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length
                        var readBuffer = new byte[netEvent.Packet.Length];
                        var readStream = new MemoryStream(readBuffer);
                        var reader = new BinaryReader(readStream);

                        readStream.Position = 0;
                        netEvent.Packet.CopyTo(readBuffer);
                        packetId = (PacketId)reader.ReadByte();
                        break;
                }

                ServerEventHandler?.Invoke(netEvent, packetId);

                if (netEvent.Type == EventType.Receive)
                    netEvent.Packet.Dispose();
            }
        }
    }
}
