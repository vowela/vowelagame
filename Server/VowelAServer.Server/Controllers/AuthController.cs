using System;
using System.Collections.Generic;
using System.IO;
using ENet;
using VowelAServer.Server.Controllers;
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
            if (packetId == PacketId.LoginRequest)
            {
                var playerId = netEvent.Peer.ID;
                SendLoginResponse(ref netEvent, playerId);
                BroadcastLoginEvent(playerId);
                foreach (var player in players)
                    SendLoginEvent(ref netEvent, player.Id);
                players.Add(new Player(playerId));
            }
            else if (packetId == PacketId.LogoutEvent)
            {
                var playerId = netEvent.Peer.ID;
                HandleLogout(playerId);
            }
        }

        private void HandleLogout(uint playerId)
        {
            var player = new Player(playerId);
            if (!players.Contains(player))
                return;

            players.Remove(player);
            BroadcastLogoutEvent(playerId);
        }

        private void BroadcastLogoutEvent(uint playerId)
        {
            var protocol = new Protocol();
            var buffer = protocol.Serialize((byte)PacketId.LogoutEvent, playerId);
            var packet = default(Packet);
            packet.Create(buffer);
            Server.HostInstance.Broadcast(0, ref packet);
        }

        private void SendLoginResponse(ref Event netEvent, uint playerId)
        {
            var protocol = new Protocol();
            var buffer = protocol.Serialize((byte)PacketId.LoginResponse, playerId);
            var packet = default(Packet);
            packet.Create(buffer);
            netEvent.Peer.Send(0, ref packet);
        }

        private void SendLoginEvent(ref Event netEvent, uint playerId)
        {
            var protocol = new Protocol();
            var buffer = protocol.Serialize((byte)PacketId.LoginEvent, playerId);
            var packet = default(Packet);
            packet.Create(buffer);
            netEvent.Peer.Send(0, ref packet);
        }

        private void BroadcastLoginEvent(uint playerId)
        {
            var protocol = new Protocol();
            var buffer = protocol.Serialize((byte)PacketId.LoginEvent, playerId);
            var packet = default(Packet);
            packet.Create(buffer);
            Server.HostInstance.Broadcast(0, ref packet);
        }
    }
}
