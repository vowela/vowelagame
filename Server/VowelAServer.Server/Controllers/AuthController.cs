using System;
using System.Collections.Generic;
using ENet;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Models.Dtos;
using VowelAServer.Db.Services;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Utils;
using VowelAServer.Shared.Data.Enums;
using VowelAServer.Shared.Models;
using VowelAServer.Shared.Networking;
using VowelAServer.Utilities.Logging;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Server.Controllers
{
    /// <summary>
    /// AuthController contains authentication methods and corresponds for session ID's updating/renewing
    /// </summary>
    public class AuthController : StaticNetworkObject
    {
        /// <summary> Tries to login with login data, creates new Player or returns existing if user exists or empty if not </summary>
        private static User TryLogin(UserDto dto)
        {
            if (dto == null) return null;

            var user = UserService.GetUserByLogin(dto.Login);
            if (user != null && PasswordHasher.VerifyPassword(dto.Password, user.HashedPassword, user.Salt))
                return user;
            return null;
        }

        /// <summary> Hashes password, saves to database and returns result of registering </summary>
        private static bool TryRegister(UserDto user)
        {
            PasswordHasher.CreateHash(user.Password, out var hashedPassword, out var salt);

            var userToAdd = new User()
            {
                Login          = user.Login,
                HashedPassword = hashedPassword,
                Salt           = salt,
                Roles          = Roles.User,
            };

            return UserService.CreateUser(userToAdd);
        }

        [RPC] public static void Register(Player player, UserDto user)
        {
            var registerResult = TryRegister(user);
            RPC(player.NetPeer, "AuthController", "OnRegistered", registerResult);
        }

        /// <summary> Remove user's sid from database, clear player registration, unauthorize user </summary>
        [RPC] public static void Logout(Player player)
        {
            var playerSId  = player.GetSId();

            ChatManager.SendToAll(player, "Пока сучары!");
            
            var user       = UserService.GetUserBySID(playerSId);
            user.SessionID = Guid.Empty;
            UserService.UpdateUserData(user);
            player.Unregister(playerSId);
            RPC(player.NetPeer, "AuthController", "OnAuthorized", (AuthResult.Unauthorized, Guid.Empty));
        }

        [RPC] public static void Login(Player player, UserDto user)
        {
            var userData = TryLogin(user);
            // Check session, update id if it's expired/not exists
            if (userData != null && userData.SessionID == Guid.Empty) RenewSID(userData);
            if (userData != null && userData.SessionID != Guid.Empty)
            {
                player.Register(userData.SessionID);
                
                ChatManager.SendToAll(player, "Вечер в хату!");
                RPC(player.NetPeer, "AuthController", "OnAuthorized", (AuthResult.Authorized, userData.SessionID));
            }
            else
            {
                RPC(player.NetPeer, "AuthController", "OnAuthorized", (AuthResult.Unauthorized, Guid.Empty));
            }
        }

        [RPC] public static void LoginSession(Player player, Guid sessionId)
        {
            var userData = UserService.GetUserBySID(sessionId);
            if (userData != null)
            {
                if (userData.SessionID == Guid.Empty) RenewSID(userData);
                player.Register(userData.SessionID);
                
                ChatManager.SendToAll(player, "Вечер в хату!");
            }
            RPC(player.NetPeer, "AuthController", "OnAuthorized",
                userData != null ? (AuthResult.Authorized, userData.SessionID) : (AuthResult.Unauthorized, Guid.Empty));
        }

        private static void RenewSID(User user)
        {
            user.SessionID = Guid.NewGuid();
            UserService.UpdateUserData(user);
        }
    }
}
