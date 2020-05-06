using System;
using System.Collections.Generic;
using ENet;
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

        public static SceneData CreateObject(ContainerData container)
        {
            if (!string.IsNullOrEmpty(container.Id) &&
                HasObject(container.Id))
            {
                // Object exists
            }
            else
            {
                // Check if requested area for an object is available
                if (string.IsNullOrEmpty(container.AreaId) || !WorldAreaController.HasArea(container.AreaId)) return new SceneData();
                // Create new object
                if (string.IsNullOrEmpty(container.Id)) container.Id = Guid.NewGuid().ToString();
                container.ClientLuaCode = @"function OnStart() CreatePrimitive(0, true) end";
                WorldSimulation.Instance.SceneController.Scene.SceneData.Add(container);

                var sceneChanges = new SceneData()
                {
                    Added = new HashSet<ContainerData> { container }
                };
                return sceneChanges;
            }
            return new SceneData();
        }

        public static SceneData ChangeObject(ContainerData container)
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
                    return sceneChanges;
                }
            }
            else
            {
                // Nothing found
            }
            return new SceneData();
        }
    }
}
