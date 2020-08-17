using System;
using ENet;
using UnityEngine;
using VowelAServer.Shared.Networking;

public class NetworkComponent : MonoBehaviour
{
    private NetObject netObject;

    public void InitComponent(int netId) => netObject = new NetObject(netId);

    public void Start()
    {
        if (netObject == null) Debug.LogError(gameObject.name + ": I'm not initialized");
    }

    /// <summary> Sending an RPC call for this object and method in it </summary>
    public void RPC(string methodName, params object[] args) => NetworkHelper.SendData(SerializationHelper.Serialize((byte)NetworkEvent.RPC, methodName, string.Empty, netObject.NetId, args));
}
