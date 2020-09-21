using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Multiplayer;

public class MenuUI : MonoBehaviour
{
    public Text Nickname;
    public InputField NicknameField;

    public GameRoomUI RoomUI;
    
    public GameObject MainPanel;
    public GameObject RoomPanel;
    public GameObject DbUI;

    public Button BackToRoomButton;
    public Button FindRoomButton;
    public Button CancelSearchButton;
    public Button LeaveRoomButton;
    public Text FindRoomStatus;
    
    private bool isFirstStart;

    private void Start()
    {
        Player.Instance.PlayerDataChanged.AddListener(OnPlayerDataChanged);
    }

    private void OnEnable()
    {
        StaticNetworkComponent.RPC("PlayerController", "GetPlayerData");
        RoomsController.OnConnectedToRoom.AddListener(OnJoiningRoom);
        RoomsController.OnStopRoomSearch.AddListener(StopRoomSearch);
        if (!MainPanel.activeSelf) MainPanel.SetActive(true);
        if (RoomPanel.activeSelf)  RoomPanel.SetActive(false);
        isFirstStart = true;
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
        if (LeaveRoomButton.gameObject.activeSelf) LeaveRoomButton.gameObject.SetActive(false);
        RoomsController.StartRoomSearch();
    }
    
    public void StopRoomSearch()
    {
        // Remove requesting process, stop search
        FindRoomButton.interactable = true;
        FindRoomStatus.text         = "";
        LeaveRoomButton.gameObject.SetActive(false);
        if (BackToRoomButton.gameObject.activeSelf) BackToRoomButton.gameObject.SetActive(false);
    }

    public void LeaveRoom() => RoomsController.LeaveRoom();

    private void OnJoiningRoom(Room room)
    {
        FindRoomStatus.text             = "Connected to room";
        CancelSearchButton.interactable = false;
        if (!BackToRoomButton.gameObject.activeSelf) BackToRoomButton.gameObject.SetActive(true);
        RoomUI.UpdateRoomData();
        
        if (MainPanel.activeSelf)  MainPanel.SetActive(false);
        if (!RoomPanel.activeSelf) RoomPanel.SetActive(true);
    }

    private void OnPlayerDataChanged()
    {
        Nickname.text = Player.Instance.Profile.Nickname;
        if (isFirstStart && !string.IsNullOrEmpty(Player.Instance.Profile.ConnectedRoomName))
        {
            // There is a room name, show a message that we can reconnect to the room
            FindRoomStatus.text = "Disconnected from room. Reconnect?";
            LeaveRoomButton.gameObject.SetActive(true);
        }

        isFirstStart = false;
    }
    
    public void Logout() => AuthController.Logout();

    public void OpenDbView() => DbUI.SetActive(true);
}