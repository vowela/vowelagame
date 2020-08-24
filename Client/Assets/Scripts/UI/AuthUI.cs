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
    Connected,
    Authorized,
    Unauthorized
}

public class AuthUI : MonoBehaviour
{
    public Text ConnectingMessage;
    public GameObject ConnectingUI, LoginUI, MenuUI;

    public bool AllowAutoLogin;

    private string login;
    private string password;
    private AuthState currentState = AuthState.Connecting;
    
    private void Start()
    {
        AuthController.AuthorizationNotify += result => 
        {
            currentState = result == AuthResult.Authorized ? AuthState.Authorized : AuthState.Unauthorized;
            ChangeAuthState();
        };
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

        ChangeAuthState();
    }

    private void ChangeAuthState()
    {
        // Disable/enable auth windows with connection to server
        switch (currentState)
        {
            case AuthState.Connected when AllowAutoLogin:
                // Auth with Session ID saved (because of auto login)
                var result = LoginSession();
                AllowAutoLogin = false;
                if (result)
                {
                    ConnectingMessage.text = "Authorizing..";
                    currentState = AuthState.LoggingIn;
                }
                break;
            case AuthState.Connected when !LoginUI.activeSelf:
                // Show LoginUI if it's disabled, but connected to server
                currentState = AuthState.LoggingIn;
                ConnectingUI.SetActive(false);
                LoginUI.SetActive(true);
                break;
            case AuthState.Disconnected when LoginUI.activeSelf:
                // Hide LoginUI if disconnected from server
                ConnectingUI.SetActive(true);
                LoginUI.SetActive(false);
                break;
            case AuthState.Authorized:
            case AuthState.Unauthorized:
                LoginUI.SetActive(currentState != AuthState.Authorized);
                MenuUI.SetActive(currentState == AuthState.Authorized);
                break;
        }
    }

    /// <summary> Send logout request </summary>
    public void Logout() => StaticNetworkComponent.RPC("AuthController", "Logout");

    /// <summary> Send login request with login data </summary>
    public void Login()  => StaticNetworkComponent.RPC("AuthController", "Login", new UserDto {Login = login, Password = password});

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
