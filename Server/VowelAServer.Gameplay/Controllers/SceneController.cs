using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using VowelAServer.Gameplay.Models;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Utils;

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
                if (string.IsNullOrEmpty(sceneDataPath)) sceneDataPath = Utils.GetDirPath("Storage") + @"/scene_file.json";

                return sceneDataPath;
            }
        }

        public void LoadSceneData()
        {
            if (!File.Exists(SceneDataPath))
            {
                Scene = new SceneModel {
                    SceneData          = new HashSet<ContainerData>(), 
                    ContainerAreas     = new HashSet<ContainerArea>(),
                    ContainerAreaNames = new Dictionary<string, string>()
                };
                return;
            }
            var openFile = JsonConvert.DeserializeObject<SceneModel>(File.ReadAllText(SceneDataPath));
            Scene = openFile;
        }

        public void SaveSceneData()
        {
            File.WriteAllText(SceneDataPath, JsonConvert.SerializeObject(Scene));
        }
    }
}
