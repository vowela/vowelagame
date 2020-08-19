using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Dtos;
using VowelAServer.Shared.Networking;
using RPC = VowelAServer.Shared.Models.RPC;

public class AuthController : StaticNetworkComponent
{
    public Button LoginButton;
    
    private string login;
    private string password;

    public void OnLoginChange(string login) => this.login = login;

    public void OnPasswordChange(string password) => this.password = password;

    private void Update()
    {
        if (ConnectionManager.IsConnected && !LoginButton.IsInteractable())      LoginButton.interactable = true;
        else if (!ConnectionManager.IsConnected && LoginButton.IsInteractable()) LoginButton.interactable = false;
    }
    
    public void Login()
    {
        var user = new UserDto {Login = login, Password = password};

        RPC("AuthController", "Login", user);
        RPC("DeveloperConsole", "ProcessCommand", "Auth", "Register", new object[] {"pipiska"});
    }

    public void Register()
    {
        var user = new UserDto {Login = login, Password = password};

        RPC("AuthController", "Register", user);
    }
    
    [RPC]
    public static void OnAuthorized(bool isAuthorized)
    {
        Debug.Log(isAuthorized ? "User exists" : "User not registered?");
    }
}
