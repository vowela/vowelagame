using System;
using System.Collections.Generic;
using System.IO;
using ENet;
using VowelAServer.Server.Net;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Data.Multiplayer;
using Newtonsoft.Json;
using VowelAServer.Shared.Models.Dtos;
using VowelAServer.Db.Services;

namespace VowelAServer.Server.Authorization
{
    public class AuthController
    {
        private HashSet<Player> players = new HashSet<Player>();

        public AuthController()
        {
            NetEventPoll.ServerEventHandler += NetEventPoll_ServerEventHandler;
        }

        public bool Login(UserDto user)
        {
            if (user == null) return false;

            // TODO: добавить потом проверку пароля
            return UserService.GetUserByLogin(user.Login) != null;
        }

        private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId)
        {
            var netEvent = (Event)sender;
            var playerId = netEvent.Peer.ID;
            
            /*if (packetId == PacketId.ConnectionRequest)
            {
                var readBuffer = new byte[netEvent.Packet.Length];
                netEvent.Packet.CopyTo(readBuffer);

                var protocol = new Protocol();
                protocol.Deserialize(readBuffer, out var code, out var userStr);

                var user = JsonConvert.DeserializeObject<UserDto>(userStr);

                var isLogedIn = Login(user);

                //var data = Protocol.SerializeData((byte) PacketId.LoginResponse, Convert.ToUInt32(isLogedIn));
                //NetController.SendData(data, ref netEvent);

                if (isLogedIn) players.Add(new Player(playerId));
            }
            else if (packetId == PacketId.PlayerDisconnectEvent)
            {
                var player = new Player(playerId);
                if (!players.Contains(player)) return;

                players.Remove(player);
                var data = Protocol.SerializeData((byte)PacketId.PlayerDisconnectEvent, playerId);
                NetController.SendData(data);
            }*/
        }
    }
}
