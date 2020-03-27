using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VowelAServer.Shared.Data;

public class StartupManager : MonoBehaviour
{
    private SceneManager sceneManager;

    void Start()
    {
        // Start attempt to connect to server
        // Download a scene file
        var sceneFile = TestGenerateJson();

        // Parse scene file
        Debug.Log(sceneFile);
        sceneManager = GetComponentInChildren<SceneManager>();
        var sceneData = JsonUtility.FromJson<SceneData>(sceneFile);
        sceneManager.CompileScene(sceneData);
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
