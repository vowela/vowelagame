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
    public static bool IsSceneLoaded;
    

    void Update() 
    {
        if (!IsSceneLoaded && ConnectionManager.IsConnected)
        {
            // Start attempt to connect to server
            // Download a scene file
            var data = Protocol.SerializeData((byte)PacketId.SceneDataRequest, 0);
            NetController.SendData(data);
            IsSceneLoaded = true;
        }
    }
}
