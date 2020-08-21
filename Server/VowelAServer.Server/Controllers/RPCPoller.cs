using System.Collections.Generic;
using ENet;
using VowelAServer.Shared.Gameplay;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Server.Controllers
{
    /// <summary>
    /// This class corresponds to managing new peers (connections) over the network session
    /// Also as it's a tick, it processes all rpc commands in poll
    /// </summary>
    public class RPCPoller : ITickable
    {
        public void Tick()
        {
            if (RPCPoll.RPCQueue.TryDequeue(out var rpcData))
            {
                if (rpcData.peer.IsSet) SendData(rpcData.data, ref rpcData.peer);
                else                    SendData(rpcData.data);
            }
        }
        
        // Sends data to all peers via broadcast
        public static void SendData(byte[] buffer)
        {
            var packet = default(Packet);
            packet.Create(buffer);
            Server.HostInstance.Broadcast(0, ref packet);
        }

        // Sends data to concrete peer
        public static void SendData(byte[] buffer, ref Peer peer)
        {
            var packet = default(Packet);
            packet.Create(buffer);
            peer.Send(0, ref packet);
        }
    }
}