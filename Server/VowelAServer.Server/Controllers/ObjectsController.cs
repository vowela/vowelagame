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
    public class ObjectsController
    {
        public ObjectsController()
        { 
            NetEventPoll.ServerEventHandler += NetEventPoll_ServerEventHandler;
        }

        private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId)
        {
            var senderEvent = (Event)sender;
            if (packetId == PacketId.ObjectChangesRequest)
            {
                var readBuffer = new byte[senderEvent.Packet.Length];
                senderEvent.Packet.CopyTo(readBuffer);

                var protocol = new Protocol();
                protocol.Deserialize(readBuffer, out var code, out var containerJson);

                var containerObject = JsonConvert.DeserializeObject<ContainerData>(containerJson);
                if (containerObject != null)
                {
                    WorldObjectController.CreateObject(containerObject);

                    //var data = Protocol.SerializeData((byte)PacketId.MenuResponse, json);
                    //NetController.SendData(data);
                }
            }
        }
    }
}
