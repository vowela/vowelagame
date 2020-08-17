using System.Collections;
using System.Collections.Generic;
using ENet;
using UnityEngine;
using VowelAServer.Shared.Networking;

public class StaticNetworkComponent : MonoBehaviour
{
    private StaticNetObject netObject;
    
    /// <summary> Sending a static RPC call for an object and method in it </summary>
    public static void RPC(string objectName, string methodName, params object[] args) => NetworkHelper.SendData(SerializationHelper.Serialize((byte)NetworkEvent.RPCStatic, methodName, objectName, -1, args));
}
