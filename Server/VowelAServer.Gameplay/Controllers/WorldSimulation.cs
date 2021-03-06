﻿using VowelAServer.Shared.Gameplay;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Gameplay.Controllers
{
    public class WorldSimulation : ITickable
    {
        private float lastSavedTime;
        private float timeToSave = 300000f; // 5 mins
        
        public WorldSimulation()
        {
            //WorldTime.Instance().Start(WorldTime.GetSavedTime());
        }

        public void Tick()
        {
            if (WorldTime.Instance().GetWorldTime() > lastSavedTime + timeToSave)
            {
                Logger.Write("Save the server data..");

                WorldTime.Instance().SaveTime();
                lastSavedTime = WorldTime.Instance().GetWorldTime();
            }
        }
    }
}
