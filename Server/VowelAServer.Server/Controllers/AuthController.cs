using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ENet;
using VowelAServer.Server.Net;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Data.Multiplayer;
using Newtonsoft.Json;
using VowelAServer.Shared.Models.Dtos;
using VowelAServer.Db.Services;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Controllers;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Data.Math;
using VowelAServer.Server.Utils;
using VowelAServer.Shared.Data.Enums;

namespace VowelAServer.Server.Authorization
{
    public class AuthController
    {
        private const string AuthAreaName = "AuthorizationArea";
        private HashSet<Player> players = new HashSet<Player>();

        public AuthController()
        {
            NetEventPoll.ServerEventHandler += NetEventPoll_ServerEventHandler;

            CreateBaseAuthAreaIfNotExists();
        }

        private void CreateBaseAuthAreaIfNotExists()
        {
            if (!WorldAreaController.HasAreaName(AuthAreaName))
            {
                // Firstly create new area (The world center)
                var newArea = new ContainerArea(new Vector(0, -63700));
                WorldAreaController.CreateArea(newArea);
                WorldAreaController.SetUniqueName(AuthAreaName, newArea.Id);
                
                // Then create a new auth object and place it in a new area
                var authObject = new ContainerData
                {
                    ContainerName = "AuthObject",
                    AreaId = newArea.Id,
                    Position = new Point(0, -63700)
                };

                WorldObjectController.CreateObject(authObject);
            }
        }

        public bool Login(UserDto dto)
        {
            if (dto == null) return false;

            var user = UserService.GetUserByLogin(dto.Login);
            if (user == null) return false;

            return PasswordHasher.VerifyPassword(dto.Password, user.HashedPassword, user.Salt);
        }

        public bool Register(UserDto user)
        {
            byte[] hashedPassword, salt;

            PasswordHasher.CreateHash(user.Password, out hashedPassword, out salt);

            var userToAdd = new User()
            {
                Login = user.Login,
                HashedPassword = hashedPassword,
                Salt = salt,
                Roles = Roles.User,
            };

            return UserService.CreateUser(userToAdd);
        }

        private void NetEventPoll_ServerEventHandler(object sender, PacketId packetId)
        {
            var netEvent = (Event)sender;
            var playerId = netEvent.Peer.ID;

            if (netEvent.Type == EventType.Connect)
            {
                var authArea = WorldAreaController.GetAreaByName(AuthAreaName);
                if (authArea != null)
                {
                    var objectsNumerator = WorldSimulation.Instance.SceneController.Scene.SceneData.Where(x=>x.AreaId == authArea.Id);
                    var sceneDataUpdate = new SceneData {Added = objectsNumerator.ToHashSet()};
                    var authAreaJson = JsonConvert.SerializeObject(sceneDataUpdate);
                    var data = Protocol.SerializeData((byte) PacketId.AreaResponse, authAreaJson);
                    NetController.SendData(data, ref netEvent);
                }
            }
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
