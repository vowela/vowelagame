using System;
using System.Collections.Generic;
using System.IO;
using ENet;
using VowelAServer.Server.Net;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Data.Multiplayer;

namespace VowelAServer.Server.Authorization
{
    public class AuthController
    {
        private HashSet<Player> players = new HashSet<Player>();

        public AuthController()
        {
            NetEventPoll.ServerEventHandler += NetEventPoll_ServerEventHandler;
        }

        private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId)
        {
            var netEvent = (Event)sender;
            var playerId = netEvent.Peer.ID;

            if (packetId == PacketId.LoginRequest)
            {
                var data = Protocol.SerializeData((byte) PacketId.LoginResponse, playerId);
                NetController.SendData(data, ref netEvent);

                data = Protocol.SerializeData((byte)PacketId.LoginEvent, playerId);
                NetController.SendData(data);
                /* TODO: IS IT NECESSARY ??
                foreach (var player in players)
                {
                    data = Protocol.SerializeData((byte)PacketId.LoginEvent, playerId);
                    NetController.SendData(data, ref netEvent);
                }
                */
                players.Add(new Player(playerId));
            }
            else if (packetId == PacketId.LogoutEvent)
            {
                var player = new Player(playerId);
                if (!players.Contains(player))
                    return;

                players.Remove(player);
                var data = Protocol.SerializeData((byte)PacketId.LogoutEvent, playerId);
                NetController.SendData(data);
            }
        }
    }
}
