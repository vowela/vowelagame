using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Dtos;
using VowelAServer.Shared.Networking;

public class AuthUI : MonoBehaviour
{
    public GameObject LoginUI;
    public GameObject MenuUI;
    
    public Button LoginButton;
    
    public bool AllowAutoLogin;

    private string login;
    private string password;

    private void Start()
    {
        AuthController.AuthorizationNotify += result =>
        {
            if (result == AuthResult.Authorized) HideAuthWindow();
            else if (result == AuthResult.Unauthorized) ShowAuthWindow();
        };
    }

    private void HideAuthWindow()
    {
        LoginUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    private void ShowAuthWindow()
    {
        MenuUI.SetActive(false);
        LoginUI.SetActive(true);
    }

    private void Update()
    {
        // Auth with session ID if allowed
        if (ConnectionManager.IsConnected && AllowAutoLogin)
        {
            LoginSession();
            AllowAutoLogin = false;
        }
        // Disable/enable login buttons with connection to server
        if (ConnectionManager.IsConnected && !LoginButton.IsInteractable())      LoginButton.interactable = true;
        else if (!ConnectionManager.IsConnected && LoginButton.IsInteractable()) LoginButton.interactable = false;
    }

    /// <summary> Send logout request </summary>
    public void Logout()
    {
        StaticNetworkComponent.RPC("AuthController", "Logout");
    }
    
    /// <summary> Send login request with login data </summary>
    public void Login()
    {
        var user = new UserDto {Login = login, Password = password};
        StaticNetworkComponent.RPC("AuthController", "Login", user);
    }
    
    /// <summary> Send login request with session ID </summary>
    private void LoginSession()
    {
        if (!PlayerPrefs.HasKey(AuthController.SessionID)) return;
        var sessionIdStr = PlayerPrefs.GetString(AuthController.SessionID);
        if (Guid.TryParse(sessionIdStr, out var sessionId)) StaticNetworkComponent.RPC("AuthController", "LoginSession", sessionId);
    }

    /// <summary> Send register request with login data </summary>
    public void Register()
    {
        var user = new UserDto {Login = login, Password = password};
        StaticNetworkComponent.RPC("AuthController", "Register", user);
    }
    
    public void OnLoginChange(string login)       => this.login = login;
    public void OnPasswordChange(string password) => this.password = password;
}
