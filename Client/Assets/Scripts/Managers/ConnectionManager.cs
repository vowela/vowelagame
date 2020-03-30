using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ENet;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data.Multiplayer;

public class ConnectionManager : MonoBehaviour
{
    public static event EventHandler<PacketId> ClientEventHandler;
    public static Peer CurrentPeer;
    public static Host Client;
    public string Ip = "127.0.0.1";
    public ushort Port = 6005;
    private int skipFrame = 0;

    void Awake ()
    {
        Application.runInBackground = true;
        InitENet();
    }

    void FixedUpdate()
    {
        UpdateENet();

        if (++skipFrame < 3)
            return;

        skipFrame = 0;
    }

    void OnDestroy()
    {
        Client.Dispose();
        ENet.Library.Deinitialize();
    }

    private void InitENet()
    {
        ENet.Library.Initialize();
        Client = new Host();
        Address address = new Address();

        address.SetHost(Ip);
        address.Port = Port;
        Client.Create();
        
        Debug.Log("Connecting");
        CurrentPeer = Client.Connect(address);
    }

    private void UpdateENet()
    {
        ENet.Event netEvent;

        if (Client.CheckEvents(out netEvent) <= 0)
        {
            if (Client.Service(15, out netEvent) <= 0)
                return;
        }

        var packetId = PacketId.None;
        switch (netEvent.Type)
        {
            case ENet.EventType.None:
                break;

            case ENet.EventType.Connect:
                Debug.Log("Client connected to server - ID: " + CurrentPeer.ID);
                Debug.Log("SendLogin");
                var data = Protocol.SerializeData((byte)PacketId.LoginRequest, 0);
                NetController.SendData(data);
                
                AuthController.Authorized = true;
                break;

            case ENet.EventType.Disconnect:
                Debug.Log("Client disconnected from server");
                break;

            case ENet.EventType.Timeout:
                Debug.Log("Client connection timeout");
                break;

            case ENet.EventType.Receive:
                Debug.Log("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                
                var readBuffer = new byte[netEvent.Packet.Length];
                var readStream = new MemoryStream(readBuffer);
                var reader = new BinaryReader(readStream);

                readStream.Position = 0;
                netEvent.Packet.CopyTo(readBuffer);
                packetId = (PacketId)reader.ReadByte();

                Debug.Log("ParsePacket received: " + packetId);
                break;
        }

        if (packetId != PacketId.None)
            ClientEventHandler?.Invoke(netEvent, packetId);

        if (netEvent.Type == ENet.EventType.Receive)
            netEvent.Packet.Dispose();
    }
}
