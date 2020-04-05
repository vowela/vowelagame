using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ENet;
using Server;
using UnityEngine;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Data.Multiplayer;

public class StartupManager : MonoBehaviour
{
    private SceneManager sceneManager;
    private bool isSceneLoaded;
    void Start()
    {
        ConnectionManager.ClientEventHandler += NetEventPoll_ServerEventHandler;
    }

    void Update() 
    {
        if (!isSceneLoaded && AuthController.Authorized)
        {
            // Start attempt to connect to server
            // Download a scene file
            var data = Protocol.SerializeData((byte)PacketId.SceneDataRequest, 0);
            NetController.SendData(data);
            isSceneLoaded = true;
        }
    }

    private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId){
        var netEvent = (ENet.Event) sender;
        if (packetId == PacketId.SceneDataResponse) {
            var protocol = new Protocol();
            
            var readBuffer = new byte[netEvent.Packet.Length];
            netEvent.Packet.CopyTo(readBuffer);

            protocol.Deserialize(readBuffer, out var code, out var sceneFile);

            // Parse scene file
            sceneManager = GetComponentInChildren<SceneManager>();
            var sceneData = JsonUtility.FromJson<SceneData>(sceneFile);
            sceneManager.CompileScene(sceneData);
            isSceneLoaded = true;
        }
    }
}
