using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VowelAServer.Gameplay.Models;
using VowelAServer.Shared.Data;

namespace VowelAServer.Gameplay.Controllers
{
    public class SceneController
    {
        public SceneModel Scene;

        private string sceneDataPath;
        public string SceneDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(sceneDataPath))
                {
                    var lastParent = Directory.GetParent(Path.Combine(Directory.GetCurrentDirectory()));
                    var done = false;
                    for (var i = 0; i < 5; i++)
                    {
                        foreach (var dir in lastParent.GetDirectories())
                        {
                            if (dir.Name == "Storage")
                            {
                                done = true;
                                sceneDataPath = dir.FullName.ToString();
                                break;
                            }
                        }
                        if (done) break;
                        lastParent = lastParent.Parent;
                        sceneDataPath = lastParent.ToString();
                    }
                    sceneDataPath += @"/scene_file.json";
                }
                return sceneDataPath;
            }
        }

        public void LoadSceneData()
        {
            if (!File.Exists(SceneDataPath))
            {
                Scene = new SceneModel { SceneData = new HashSet<ContainerData>() };
                return;
            }
            var openFile = JsonConvert.DeserializeObject<SceneModel>(File.ReadAllText(SceneDataPath));
            Scene = openFile;
        }

        public void SaveSceneData()
        {
            File.WriteAllText(SceneDataPath, JsonConvert.SerializeObject(Scene));
        }

        private List<ContainerData> CreateTestScene()
        {
            var child0 = Guid.NewGuid().ToString();
            var child1 = Guid.NewGuid().ToString();
            var child2 = Guid.NewGuid().ToString();
            var child3 = Guid.NewGuid().ToString();
            return new List<ContainerData>()
            {
                new ContainerData
                {
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
            };
        }
    }
}
