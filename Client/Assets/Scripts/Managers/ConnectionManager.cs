using System.Collections;
using System.Collections.Generic;
using System.IO;
using ENet;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data.Multiplayer;

public class ConnectionManager : MonoBehaviour
{
    private int skipFrame = 0;
    private Host client;
    private Peer peer;
    const int channelID = 0;

    void Start ()
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
        client.Dispose();
        ENet.Library.Deinitialize();
    }

    private void InitENet()
    {
        const string ip = "127.0.0.1";
        const ushort port = 6005;
        ENet.Library.Initialize();
        client = new Host();
        Address address = new Address();

        address.SetHost(ip);
        address.Port = port;
        client.Create();
        Debug.Log("Connecting");
        peer = client.Connect(address);
    }

    private void UpdateENet()
    {
        ENet.Event netEvent;

        if (client.CheckEvents(out netEvent) <= 0)
        {
            if (client.Service(15, out netEvent) <= 0)
                return;
        }

        switch (netEvent.Type)
        {
            case ENet.EventType.None:
                break;

            case ENet.EventType.Connect:
                Debug.Log("Client connected to server - ID: " + peer.ID);
                SendLogin();
                break;

            case ENet.EventType.Disconnect:
                Debug.Log("Client disconnected from server");
                break;

            case ENet.EventType.Timeout:
                Debug.Log("Client connection timeout");
                break;

            case ENet.EventType.Receive:
                Debug.Log("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                ParsePacket(ref netEvent);
                netEvent.Packet.Dispose();
                break;
        }
    }


    private void SendLogin()
    {
        Debug.Log("SendLogin");
        var protocol = new Protocol();
        var buffer = protocol.Serialize((byte)PacketId.LoginRequest, 0);
        var packet = default(Packet);
        packet.Create(buffer);
        peer.Send(channelID, ref packet);
    }

    private void ParsePacket(ref ENet.Event netEvent)
    {
        var readBuffer = new byte[1024];
        var readStream = new MemoryStream(readBuffer);
        var reader = new BinaryReader(readStream);

        readStream.Position = 0;
        netEvent.Packet.CopyTo(readBuffer);
        var packetId = (PacketId)reader.ReadByte();

        Debug.Log("ParsePacket received: " + packetId);
    }
}
