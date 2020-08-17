using System;
using System.Collections.Generic;

namespace VowelAServer.Shared.Networking
{
    public class NetObject
    {
        private static readonly Dictionary<int, NetObject> NetworkObjects = new Dictionary<int, NetObject>();
        public int NetId { get; private set; }

        public NetObject(int netId)
        {
            NetId = netId;
            NetworkObjects[NetId] = this;
        }

        public static NetObject FindNetObjectById(int netId) => NetworkObjects[netId];
    }

    public class StaticNetObject
    {
        
    }
}