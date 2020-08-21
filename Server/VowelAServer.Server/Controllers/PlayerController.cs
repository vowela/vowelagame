using VowelAServer.Db.Services;
using VowelAServer.Server.Models;
using VowelAServer.Server.Utils;
using VowelAServer.Shared.Models;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Server.Controllers
{
    public class PlayerController : StaticNetworkObject
    {
        [RPC]
        public static void GetPlayerData(Player player)
        {
            if (!player.IsRegistered) return;
            
            var playerProfile = UserService.GetPlayerProfileBySID(player.GetSId());
            RPC("PlayerController", "SetPlayerData", playerProfile);
        }

        [RPC]
        public static void SetPlayerData(Player player, PlayerProfile playerProfile)
        {
            if (!player.IsRegistered) return;

            UserService.UpdatePlayerProfileData(playerProfile);
            RPC("PlayerController", "SetPlayerData", playerProfile);
        }
    }
}