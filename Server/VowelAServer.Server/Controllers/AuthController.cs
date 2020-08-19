using System.Collections.Generic;
using ENet;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Models.Dtos;
using VowelAServer.Db.Services;
using VowelAServer.Server.Utils;
using VowelAServer.Shared.Data.Enums;
using VowelAServer.Shared.Models;
using VowelAServer.Shared.Networking;
using VowelAServer.Utilities.Logging;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Server.Controllers
{
    public class AuthController : StaticNetworkObject
    {
        private static bool TryLogin(UserDto dto)
        {
            if (dto == null) return false;

            var user = UserService.GetUserByLogin(dto.Login);
            return user != null && PasswordHasher.VerifyPassword(dto.Password, user.HashedPassword, user.Salt);
        }

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

        [RPC]
        public static void Register(Player player, UserDto user)
        {
            RPC(player.NetworkID, "AuthController", "OnAuthorized", TryRegister(user));
        }

        [RPC]
        public static void Login(Player player, UserDto user)
        {
            RPC(player.NetworkID, "AuthController", "OnAuthorized", TryLogin(user));
        }
    }
}
