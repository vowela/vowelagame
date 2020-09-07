using UnityEngine;
using UnityEngine.Events;
using VowelAServer.Shared.Interfaces;
using VowelAServer.Shared.Models.Multiplayer;
using RPC = VowelAServer.Shared.Models.RPC;

public class Player : SingletonController<Player>
{
    public UnityEvent PlayerDataChanged = new UnityEvent();
    
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
