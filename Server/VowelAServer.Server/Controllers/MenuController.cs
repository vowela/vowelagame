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
        public MenuData ObjectMenu;

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

                var json = "";
                if (type == "context")
                    json = JsonConvert.SerializeObject(ContextMenu);
                else if (type == "object")
                    json = JsonConvert.SerializeObject(ObjectMenu);

                var data = Protocol.SerializeData((byte)PacketId.MenuResponse, json);
                NetController.SendData(data);
            }
        }

        private void InitContextMenu()
        {
            ContextMenu = new MenuData()
            {
                ButtonData = new List<MenuButtonData>() {
                    new MenuButtonData {
                        ButtonText = "New Object",
                        LuaCode = @"function OnClick() CreateObject(Player.GetPosition()) end"
                    }
                }
            };
            ObjectMenu = new MenuData()
            {
                ButtonData = new List<MenuButtonData>()
                {
                    new MenuButtonData {
                        ButtonText = "Delete Object",
                        LuaCode = "function OnClick() EditLogic() end"
                    },
                    new MenuButtonData {
                        ButtonText = "Child Object",
                        LuaCode = "function OnClick() CreateObject(Player.GetPosition()) end"
                    },
                    new MenuButtonData {
                        ButtonText = "Edit Client Logic",
                        LuaCode = "function OnClick() EditClientLogic() end"
                    },
                    new MenuButtonData {
                        ButtonText = "Edit Server Logic",
                        LuaCode = "function OnClick() EditServerLogic() end"
                    }
                }
            };
        }
    }
}
