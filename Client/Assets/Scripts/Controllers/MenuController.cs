using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Multiplayer;

public class MenuController : MonoBehaviour
{
    public Text Nickname;
    public InputField NicknameField;

    public Button FindRoomButton;
    public Text FindRoomStatus;

    private void Start()
    {
        Player.Instance.PlayerDataChanged.AddListener(OnPlayerDataChanged);
    }

    void OnEnable()
    {
        StaticNetworkComponent.RPC("PlayerController", "GetPlayerData");
        StaticNetworkComponent.RPC("RoomsController", "GetRoomTryJoin");
        RoomsController.OnConnectedToRoom.AddListener(OnJoiningRoom);
        RoomsController.OnStopRoomSearch.AddListener(StopRoomSearch);
    }

    private void OnDisable()
    {
        Nickname.text = "";
        RoomsController.OnConnectedToRoom.RemoveListener(OnJoiningRoom);
        RoomsController.OnStopRoomSearch.RemoveListener(StopRoomSearch);
        StopRoomSearch();
    }

    public void ChangePlayerData()
    {
        Player.Instance.Profile.Nickname = NicknameField.text;
        StaticNetworkComponent.RPC("PlayerController", "SetPlayerData", Player.Instance.Profile);
    }

    public void FindAndConnectToRoom()
    {
        // Send a request to server that we would like to join available room or create new
        FindRoomButton.interactable = false;
        FindRoomStatus.text         = "Searching Room..";
        RoomsController.StartRoomSearch();
    }
    
    public void StopRoomSearch()
    {
        // Remove requesting process, stop search
        FindRoomButton.interactable = true;
        FindRoomStatus.text         = "";
    }

    private void OnJoiningRoom(Room room)
    {
        FindRoomStatus.text = "Connected to room";
    }

    private void OnPlayerDataChanged()
    {
        Nickname.text = Player.Instance.Profile.Nickname;
    }
}