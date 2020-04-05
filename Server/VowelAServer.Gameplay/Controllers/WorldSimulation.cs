using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VowelAServer.Gameplay.Models;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Interfaces;

namespace VowelAServer.Gameplay.Controllers
{
    public class WorldSimulation : ITickable
    {
        public static WorldSimulation Instance;

        public SceneController SceneController = new SceneController();

        private float lastSavedTime;
        private float timeToSave = 5000f; // 5 mins ( 300000 )

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
                SceneController.SaveSceneData();
                lastSavedTime = WorldTime.Instance().GetWorldTime();
            }
        }

        private void InitScene()
        {
            SceneController.LoadSceneData();
        }
    }
}
