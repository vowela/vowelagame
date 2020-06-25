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
    public class WorldAreaController
    {
        public static bool HasArea(string id) =>
            WorldSimulation.Instance.SceneController.Scene.ContainerAreas.Contains(new ContainerArea() { Id = id });
        public static bool HasAreaName(string areaName) =>
            WorldSimulation.Instance.SceneController.Scene.ContainerAreaNames.ContainsKey(areaName);
        
        public static ContainerArea GetAreaByName(string areaName) =>
            GetAreaById(WorldSimulation.Instance.SceneController.Scene.ContainerAreaNames[areaName]);
        public static ContainerArea GetAreaById(string areaId) =>
            WorldSimulation.Instance.SceneController.Scene.ContainerAreas.TryGetValue(new ContainerArea {Id = areaId}, out var outArea) ? outArea : null;
        
        public static void CreateArea(ContainerArea area)
        {
            if (!string.IsNullOrEmpty(area.Id) &&
                HasArea(area.Id))
            {
                // Object exists
            }
            else
            {
                // Create new area
                if (string.IsNullOrEmpty(area.Id)) area.Id = Guid.NewGuid().ToString();
                WorldSimulation.Instance.SceneController.Scene.ContainerAreas.Add(area);
            }
        }

        public static void SetUniqueName(string uniqueName, string areaId)
        {
            // Apply unique naming if it doesn't exist
            if (!string.IsNullOrEmpty(uniqueName))
                if (!HasAreaName(uniqueName))
                    WorldSimulation.Instance.SceneController.Scene.ContainerAreaNames.Add(uniqueName, areaId);
        }
    }
}
