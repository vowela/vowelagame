using ENet;
using VowelAServer.Shared.Gameplay;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Server.Controllers
{
    public class RPCPoller : ITickable
    {
        public void Tick()
        {
            if (RPCPoll.RPCQueue.TryDequeue(out var rpcData))
            {
                if (rpcData.peer.IsSet) SendData(rpcData.data);
                else                    SendData(rpcData.data, ref rpcData.peer);
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