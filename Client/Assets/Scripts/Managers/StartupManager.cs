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

    }

    void Update() 
    {
        if (!isSceneLoaded && AuthController.Authorized)
        {
            ConnectionManager.ClientEventHandler += NetEventPoll_ServerEventHandler;
            // Start attempt to connect to server
            // Download a scene file
            Debug.Log("DownloadScene");
            var data = Protocol.SerializeData((byte)PacketId.SceneDataRequest, 0);
            NetController.SendData(data);
            /*
            var sceneFile = TestGenerateJson();

            // Parse scene file
            Debug.Log(sceneFile);
            sceneManager = GetComponentInChildren<SceneManager>();
            var sceneData = JsonUtility.FromJson<SceneData>(sceneFile);
            sceneManager.CompileScene(sceneData);*/
            isSceneLoaded = true;
        }
    }

    private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId){
        var netEvent = (ENet.Event) sender;
        if (packetId == PacketId.SceneDataResponse) {
            var protocol = new Protocol();
            
            var readBuffer = new byte[1024];
            var readStream = new MemoryStream(readBuffer);
            var reader = new BinaryReader(readStream);

            readStream.Position = 0;
            netEvent.Packet.CopyTo(readBuffer);

            protocol.Deserialize(readBuffer, out var code, out var value);
            Debug.Log(value);
        }
    }

    private string TestGenerateJson() {
        // Generate test data
        var child0 = Guid.NewGuid().ToString();
        var child1 = Guid.NewGuid().ToString();
        var child2 = Guid.NewGuid().ToString();
        var child3 = Guid.NewGuid().ToString();
        var testScene = new SceneData() {
            Added = new List<ContainerData>() {
                new ContainerData() {
                    ContainerName = "Test Name 1",
                    LuaCode = @"--This is test lua code
                    function Update() return 'Ya siel deda' end",
                    Id = child0
                },
                new ContainerData() {
                    ContainerName = "Test Name 2",
                    LuaCode = "--This is test lua code",
                    Id = child1
                },
                new ContainerData() {
                    ContainerName = "Test Name 3",
                    LuaCode = "--This is test lua code",
                    Id = child2,
                    ParentId = child0
                },
                new ContainerData() {
                    ContainerName = "Test Name 4",
                    LuaCode = "--This is test lua code",
                    Id = child3,
                    ParentId = child2
                }
            }
        };

        return JsonUtility.ToJson(testScene);
    }
}
