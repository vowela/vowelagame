using System.Collections.Generic;
using ENet;

namespace VowelAServer.Utilities.Network
{
    public class RPCPoll
    {
        public static Queue<(int peerId, byte[] data)> RPCQueue = new Queue<(int peerId, byte[] data)>();
    }
}