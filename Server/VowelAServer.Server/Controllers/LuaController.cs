using System;
using ENet;
using VowelAServer.Server.Models;
using VowelAServer.Server.Net;
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
                var data = Protocol.SerializeData((byte)PacketId.SceneDataResponse, "Hey beba");
                NetController.SendData(data, ref senderEvent);
            }
        }
    }
}
