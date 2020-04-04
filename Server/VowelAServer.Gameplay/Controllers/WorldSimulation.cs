using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Interfaces;

namespace VowelAServer.Gameplay.Controllers
{
    public class WorldSimulation : ITickable
    {
        public static WorldSimulation Instance;

        public List<ContainerData> Scene;

        private float lastSavedTime;
        private float timeToSave = 15000f; // 5 mins ( 300000 )
        private string sceneDataPath;

        public WorldSimulation()
        {
            Instance = this;
            WorldTime.Instance().GetWorldTime();
            InitScene();
        }

        public void Tick()
        {
            if (WorldTime.Instance().GetWorldTime() > lastSavedTime + timeToSave)
            {
                Console.WriteLine("Save the server data..");
                SaveSceneData();
                lastSavedTime = WorldTime.Instance().GetWorldTime();
            }
        }

        private void InitScene()
        {
            Scene = LoadSceneData();
        }

        private string GetSceneDataPath()
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

        private List<ContainerData> LoadSceneData()
        {
            if (!File.Exists(GetSceneDataPath())) return new List<ContainerData>();
            var openFile = JsonConvert.DeserializeObject<List<ContainerData>>(File.ReadAllText(GetSceneDataPath()));
            return openFile;

            // read JSON directly from a file
            /*using (var file = File.OpenText(@"..\scene_file.json"))
            using (var reader = new JsonTextReader(file))
            {f
                var o2 = (JObject)JToken.ReadFrom(reader);

                return o2.ToObject<List<ContainerData>>();
            }*/
        }

        private void SaveSceneData()
        {
            File.WriteAllText(GetSceneDataPath(), JsonConvert.SerializeObject(Scene));

            // write JSON directly to a file
            /*using (StreamWriter file = File.CreateText(@"c:\videogames.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                sceneData.WriteTo(writer);
            }*/
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
