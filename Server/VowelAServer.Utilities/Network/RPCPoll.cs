using System.Collections.Generic;
using ENet;

namespace VowelAServer.Utilities.Network
{
    public class RPCPoll
    {
        public static Queue<(Peer peer, byte[] data)> RPCQueue = new Queue<(Peer peer, byte[] data)>();
    }
}