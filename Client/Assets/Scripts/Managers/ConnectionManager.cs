using System.Collections.Generic;
using System.IO;
using ENet;
using UnityEngine;
using VowelAServer.Shared.Networking;
using VowelAServer.Shared.Utils;

public class ConnectionManager : MonoBehaviour
{
    public static bool IsConnected;
    public static Peer CurrentPeer;
    public static Host Client;
    public string Ip = "127.0.0.1";
    public ushort Port = 6005;
    public Address Address;
    
    private int skipFrame = 0;
    private bool tryConnect = true;

    void Awake ()
    {
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
        InitENet();
    }

    void FixedUpdate()
    {
        UpdateENet();

        if (++skipFrame < 3) return;

        skipFrame = 0;
    }

    void OnDestroy()
    {
        Client.Dispose();
        Library.Deinitialize();
    }

    private void InitENet()
    {
        Library.Initialize();
        Client = new Host();
        Address = new Address();

        Address.SetHost(Ip);
        Address.Port = Port;

        Client.Create();
        
        Debug.Log("Connecting");
        Connect();
    }

    private void UpdateENet()
    {
        ENet.Event netEvent;

        if (Client.CheckEvents(out netEvent) <= 0)
            if (Client.Service(0, out netEvent) <= 0) return;

        NetworkEvent networkEvent;
        switch (netEvent.Type)
        {
            case ENet.EventType.None:
                break;
            case ENet.EventType.Connect:
                Debug.Log("Client connected to server - ID: " + CurrentPeer.ID);
                IsConnected = true;
                
                // Static RPC: UnityNetController.RPC("DevConsole", "GetCommand", "Hey");
                // Object RPC: Player.Obj.RPC("GetCommand", "Hey!");
                break;
            case ENet.EventType.Disconnect:
                Debug.Log("Client disconnected from server");
                Connect();
                break;
            case ENet.EventType.Timeout:
                Debug.Log("Client connection timeout");
                Connect();
                break;
            case ENet.EventType.Receive:
                Debug.Log("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                
                var readBuffer = new byte[netEvent.Packet.Length];
                var readStream = new MemoryStream(readBuffer);
                var reader     = new BinaryReader(readStream);

                readStream.Position = 0;
                netEvent.Packet.CopyTo(readBuffer);
                networkEvent = (NetworkEvent)reader.ReadByte();

                if (networkEvent == NetworkEvent.RPCStatic) CallStaticRpcMethod(reader);
                else if (networkEvent == NetworkEvent.RPC)  CallObjectRpcMethod(reader);
                break;
        }

        if (netEvent.Type == ENet.EventType.Receive) netEvent.Packet.Dispose();
    }

    private static void CallObjectRpcMethod(BinaryReader reader)
    {
        // Determine target name for class using object id
        var netObjectId = reader.ReadInt32();
        var netObject   = NetObject.FindNetObjectById(netObjectId);
        if (netObject != null) CallStaticRpcMethod(reader, netObject.GetType().Name, netObject);
    }

    private static void CallStaticRpcMethod(BinaryReader reader, string targetName = "", object caller = null)
    {
        // Find needed controller and method
        targetName     = string.IsNullOrEmpty(targetName) ? reader.ReadString() : targetName;
        var methodName = reader.ReadString();
        // Parse arguments data
        var argsCount  = reader.ReadInt32();
        var arguments  = new List<object>();
        for (var i = 0; i < argsCount; i++)
        {
            var argLength = reader.ReadInt32();
            var argData   = reader.ReadBytes(argLength);
            arguments.Add(SerializationHelper.DeserializeFromBytes(argData));
        }
        if (RPCManager.RPCMethods.TryGetValue((targetName, methodName), out var method))
            method.Invoke(caller, arguments.ToArray());
    }

    private void Connect() {
        if (!tryConnect) return;

        CurrentPeer = Client.Connect(Address);
    }
}
