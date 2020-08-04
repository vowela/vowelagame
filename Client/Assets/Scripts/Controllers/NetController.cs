using ENet;
using UnityEngine;
using VowelAServer.Shared.Networking;

public class NetController : MonoBehaviour
{
    public static void SendData(byte[] buffer)
    {
        var packet = default(Packet);
        packet.Create(buffer);
        ConnectionManager.CurrentPeer.Send(0, ref packet);
    }
    
    public static void RPC(string objectName, string methodName, params object[] args) => SendData(SerializationHelper.Serialize((byte)NetworkEvent.RPC, objectName, methodName, args));
}
