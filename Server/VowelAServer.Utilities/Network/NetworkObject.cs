using System.Reflection;
using System.Runtime.CompilerServices;
using ENet;
using VowelAServer.Shared.Networking;
using VowelAServer.Utilities.Helpers;

namespace VowelAServer.Utilities.Network
{
    public class NetworkObject : NetObject
    {
        /// <summary> Sending an RPC call for this object and method in it </summary>
        public void RPC(Peer peer, string methodName, params object[] args)   => RPCPoll.RPCQueue.Enqueue((peer, SerializationHelper.Serialize((byte)NetworkEvent.RPC, methodName, string.Empty, NetId, args)));
        public void RPC(string methodName, params object[] args)              => RPCPoll.RPCQueue.Enqueue((new Peer(), SerializationHelper.Serialize((byte)NetworkEvent.RPC, methodName, string.Empty, NetId, args)));
        
        public NetworkObject(int netId) : base(netId) { }
    }
    
    public class StaticNetworkObject : StaticNetObject
    {
        /// <summary> Sending a static RPC call for an object and method in it </summary>
        public static void RPC(Peer peer, string objectName, string methodName, params object[] args) => RPCPoll.RPCQueue.Enqueue((peer, SerializationHelper.Serialize((byte)NetworkEvent.RPCStatic, methodName, objectName, -1, args)));
        public static void RPC(string objectName, string methodName, params object[] args)            => RPCPoll.RPCQueue.Enqueue((new Peer(), SerializationHelper.Serialize((byte)NetworkEvent.RPCStatic, methodName, objectName, -1, args)));
    }
}