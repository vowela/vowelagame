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
    void Start() 
    {
        /*if (ConnectionManager.IsConnected)
        {
            // Download authoritative place
            var data = Protocol.SerializeData((byte)PacketId.ObjectChangesRequest, 0);
            NetController.SendData(data);
            IsSceneLoaded = true;
        }*/
    }
}
