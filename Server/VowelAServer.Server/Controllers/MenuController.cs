using System;
using System.Collections.Generic;
using ENet;
using Newtonsoft.Json;
using VowelAServer.Server.Models;
using VowelAServer.Server.Net;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Data.Multiplayer;

namespace VowelAServer.Server.Controllers
{
    public class MenuController
    {
        public MenuData ContextMenu;

        public MenuController()
        {
            NetEventPoll.ServerEventHandler += NetEventPoll_ServerEventHandler;

            InitContextMenu();
        }

        private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId)
        {
            var senderEvent = (Event)sender;
            if (packetId == PacketId.MenuRequest)
            {
                var readBuffer = new byte[senderEvent.Packet.Length];
                senderEvent.Packet.CopyTo(readBuffer);

                var protocol = new Protocol();
                protocol.Deserialize(readBuffer, out var code, out var type);

                if (type == "context")
                {
                    var json = JsonConvert.SerializeObject(ContextMenu);
                    var data = Protocol.SerializeData((byte)PacketId.MenuResponse, json);
                    NetController.SendData(data);
                }
            }
        }

        private void InitContextMenu()
        {
            ContextMenu = new MenuData()
            {
                ButtonData = new List<MenuButtonData>() {
                    new MenuButtonData {
                        ButtonText = "Новый объект",
                        LuaCode = "function OnClick() CreateGameObject('New Object Init') end"
                    },
                    new MenuButtonData {
                        ButtonText = "Привет",
                        LuaCode = "function OnClick() CreateGameObject('New Object Init') end"
                    }
                }
            };
        }
    }
}
