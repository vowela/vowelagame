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
            if (packetId == PacketId.SceneDataRequest)
            {
                Console.WriteLine("Requested scene data");
                if (WorldSimulation.Instance != null) {
                    var sceneChanges = new SceneData()
                    {
                        Added = WorldSimulation.Instance.Scene
                    };
                    var json = JsonConvert.SerializeObject(sceneChanges);
                    var data = Protocol.SerializeData((byte)PacketId.SceneDataResponse, json);
                    NetController.SendData(data, ref senderEvent);
                }
            }
        }
    }
}
