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
        private static readonly Dictionary<uint, Peer> ConnectedPeers = new Dictionary<uint, Peer>();

        public static void AddPeer(Peer peer) => ConnectedPeers[peer.ID] = peer;
        public static void RemovePeer(uint peerId) => ConnectedPeers.Remove(peerId);
        
        public void Tick()
        {
            if (RPCPoll.RPCQueue.TryDequeue(out var rpcData))
            {
                if (rpcData.peerId == -1)           
                    SendData(rpcData.data);
                else if (ConnectedPeers.TryGetValue((uint)rpcData.peerId, out var peer)) 
                    SendData(rpcData.data, ref peer);
            }
        }
        
        public static void SendData(byte[] buffer)
        {
            var packet = default(Packet);
            packet.Create(buffer);
            Server.HostInstance.Broadcast(0, ref packet);
        }

        public static void SendData(byte[] buffer, ref Peer peer)
        {
            var packet = default(Packet);
            packet.Create(buffer);
            peer.Send(0, ref packet);
        }
    }
}