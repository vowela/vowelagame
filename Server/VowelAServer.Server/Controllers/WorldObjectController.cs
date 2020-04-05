using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Models;
using VowelAServer.Server.Net;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Data.Multiplayer;

namespace VowelAServer.Server.Controllers
{
    public class WorldObjectController
    {
        public static bool HasObject(string id) =>
            WorldSimulation.Instance.SceneController.Scene.SceneData.Contains(new ContainerData { Id = id });

        public static void CreateObject(ContainerData container)
        {
            if (!string.IsNullOrEmpty(container.Id) &&
                HasObject(container.Id))
            {
                // Object exists
            }
            else
            {
                // Create new object
                if (string.IsNullOrEmpty(container.Id)) container.Id = Guid.NewGuid().ToString();
                WorldSimulation.Instance.SceneController.Scene.SceneData.Add(container);

                var sceneChanges = new SceneData()
                {
                    Added = new HashSet<ContainerData> { container }
                };
                SendSceneChanges(sceneChanges);
            }
        }

        public static void ChangeObject(ContainerData container)
        {
            if (!string.IsNullOrEmpty(container.Id) &&
                HasObject(container.Id))
            {
                // Object exists
                if (WorldSimulation.Instance.SceneController.Scene.SceneData.TryGetValue(container, out var outContainer))
                {
                    var sceneChanges = new SceneData()
                    {
                        Changed = new HashSet<ContainerData> { container }
                    };
                    SendSceneChanges(sceneChanges);
                }
            }
            else
            {
                // Nothing found
            }
        }

        private static void SendSceneChanges(SceneData sceneChanges)
        {
            var json = JsonConvert.SerializeObject(sceneChanges);
            var data = Protocol.SerializeData((byte)PacketId.ObjectChangesEvent, json);
            NetController.SendData(data);
        }
    }
}
