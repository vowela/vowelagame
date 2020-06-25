﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ENet;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data.Multiplayer;

public class ConnectionManager : MonoBehaviour
{
    public static bool IsConnected;
    public static event EventHandler<PacketId> ClientEventHandler;
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

        var packetId = PacketId.None;
        switch (netEvent.Type)
        {
            case ENet.EventType.None:
                break;
            case ENet.EventType.Connect:
                Debug.Log("Client connected to server - ID: " + CurrentPeer.ID);
                IsConnected = true;
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
                var reader = new BinaryReader(readStream);

                readStream.Position = 0;
                netEvent.Packet.CopyTo(readBuffer);
                packetId = (PacketId)reader.ReadByte();

                Debug.Log("ParsePacket received: " + packetId);
                break;
        }

        if (packetId != PacketId.None) ClientEventHandler?.Invoke(netEvent, packetId);

        if (netEvent.Type == ENet.EventType.Receive) netEvent.Packet.Dispose();
    }

    private void Connect() {
        if (!tryConnect) return;

        CurrentPeer = Client.Connect(Address);
    }
}
