using System;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Dtos;
using VowelAServer.Shared.Networking;

public enum AuthState
{
    Connecting,
    Disconnected,
    LoggingIn,
    Connected
}

public class AuthUI : MonoBehaviour
{
    public static AuthUI obj;

    public Text ConnectingMessage;
    public GameObject ConnectingUI;
    public GameObject LoginUI;
    public GameObject MenuUI;

    public bool AllowAutoLogin;

    private string login;
    private string password;
    private AuthState currentState = AuthState.Connecting;

    private void Awake() { obj = this; }
    
    private void Start()
    {
        AuthController.AuthorizationNotify += result =>
        {
            if (result == AuthResult.Authorized)        HideAuthWindow();
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
        if (!ConnectionManager.IsConnected && currentState != AuthState.Connecting)
        {
            ConnectingMessage.text = "Disconnected";
            currentState = AuthState.Disconnected;
        }
        else if (ConnectionManager.IsConnected && (currentState == AuthState.Disconnected || currentState == AuthState.Connecting))
        {
            ConnectingMessage.text = "Connected";
            currentState = AuthState.Connected;
        }

        switch (currentState)
        {
            // Disable/enable login buttons with connection to server
            case AuthState.Connected when AllowAutoLogin:
                ConnectingMessage.text = "Authorizing..";
                var result = LoginSession();
                AllowAutoLogin = false;
                if (result) currentState = AuthState.LoggingIn;
                break;
            case AuthState.Connected when !LoginUI.activeSelf:
                currentState = AuthState.LoggingIn;
                ConnectingUI.SetActive(false);
                LoginUI.SetActive(true);
                break;
            case AuthState.Disconnected when LoginUI.activeSelf:
                ConnectingUI.SetActive(true);
                LoginUI.SetActive(false);
                break;
        }
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
    private bool LoginSession()
    {
        if (!PlayerPrefs.HasKey(AuthController.SessionID)) return false;
        var sessionIdStr = PlayerPrefs.GetString(AuthController.SessionID);
        if (Guid.TryParse(sessionIdStr, out var sessionId)) StaticNetworkComponent.RPC("AuthController", "LoginSession", sessionId);
        return true;
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
