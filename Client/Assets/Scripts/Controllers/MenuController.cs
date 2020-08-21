using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text Nickname;
    public InputField NicknameField;

    private void Start()
    {
        Player.Instance.PlayerDataChanged += OnPlayerDataChanged;
    }

    void OnEnable()
    {
        StaticNetworkComponent.RPC("PlayerController", "GetPlayerData");
    }

    private void OnDisable()
    {
        Nickname.text = "";
    }

    public void ChangePlayerData()
    {
        Player.Instance.Profile.Nickname = NicknameField.text;
        StaticNetworkComponent.RPC("PlayerController", "SetPlayerData", Player.Instance.Profile);
    }
    
    private void OnPlayerDataChanged()
    {
        Nickname.text = Player.Instance.Profile.Nickname;
    }
}