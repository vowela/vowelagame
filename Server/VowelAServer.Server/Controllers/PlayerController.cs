using VowelAServer.Db.Services;
using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Models;
using VowelAServer.Server.Utils;
using VowelAServer.Shared.Models;
using VowelAServer.Shared.Models.Multiplayer;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Server.Controllers
{
    public class PlayerController : StaticNetworkObject
    {
        [RPC] public static void GetPlayerData(Player player)
        {
            if (!player.IsRegistered) return;
            
            var playerProfile = UserService.GetPlayerProfileBySID(player.GetSId());
            if (playerProfile != null) playerProfile = RoomsController.ActualizePlayerProfile(playerProfile, player.Id);
            RPC(player.NetPeer, "PlayerController", "SetPlayerData", playerProfile);
        }

        [RPC] public static void SetPlayerData(Player player, PlayerProfile playerProfile)
        {
            if (!player.IsRegistered) return;

            UserService.UpdatePlayerProfileData(playerProfile);
            RPC(player.NetPeer, "PlayerController", "SetPlayerData", playerProfile);
        }
    }
}