using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Dtos;
using VowelAServer.Shared.Networking;
using RPC = VowelAServer.Shared.Models.RPC;

public class AuthController : StaticNetworkComponent
{
    public const string SessionID = "SessionID";

    public delegate void AuthorizationHandler(AuthResult result);
    public static event AuthorizationHandler AuthorizationNotify;

    [RPC]
    public static void OnRegistered(bool isRegistered)
    {
        Debug.Log(isRegistered ? "Registered new account" : "Can't register, try again");
    }
    
    [RPC]
    public static void OnAuthorized((AuthResult result, Guid sessionId) authResult)
    {
        if (authResult.result == AuthResult.Unauthorized || authResult.sessionId == Guid.Empty)
        {
            Debug.Log("Player Unauthorized");
            PlayerPrefs.DeleteKey(SessionID);
            PlayerPrefs.Save();
            AuthorizationNotify?.Invoke(AuthResult.Unauthorized);
            return;
        }
        
        PlayerPrefs.SetString(SessionID, authResult.sessionId.ToString());
        PlayerPrefs.Save();
        Debug.Log("SID saved, player logged in");
        AuthorizationNotify?.Invoke(AuthResult.Authorized);
    }
}
