using UnityEngine;
using VowelAServer.Shared.Interfaces;
using VowelAServer.Shared.Models;
using RPC = VowelAServer.Shared.Models.RPC;

public class Player : SingletonController<Player>
{
    public delegate void PlayerDataHandler();
    public event PlayerDataHandler PlayerDataChanged;
    
    public PlayerProfile Profile;

    public void UpdateData() => PlayerDataChanged?.Invoke();
}

public class PlayerController : StaticNetworkComponent
{
    [RPC]
    public static void SetPlayerData(PlayerProfile playerProfile)
    {
        if (playerProfile == null)
        {
            Debug.Log("Error while getting profile");
            return;
        }
        Player.Instance.Profile = playerProfile;
        Player.Instance.UpdateData();
    }
}
