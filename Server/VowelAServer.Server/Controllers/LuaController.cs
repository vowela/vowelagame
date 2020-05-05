using System;
using ENet;
using Newtonsoft.Json;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Models;
using VowelAServer.Server.Net;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Data.Multiplayer;

namespace VowelAServer.Server.Controllers
{
    public class LuaController
    {
        public LuaController()
        {
            NetEventPoll.ServerEventHandler += NetEventPoll_ServerEventHandler;
        }

        private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId)
        {
            var senderEvent = (Event)sender;
           // RequestFullSceneData();
        }

        private void RequestFullSceneData()
        {
            if (WorldSimulation.Instance != null) {
                var sceneChanges = new SceneData()
                {
                    Added = WorldSimulation.Instance.SceneController.Scene.SceneData
                };
                var json = JsonConvert.SerializeObject(sceneChanges);
                var data = Protocol.SerializeData((byte)PacketId.ObjectChangesEvent, json);
                //NetController.SendData(data, ref senderEvent);
            }
        }
    }
}
