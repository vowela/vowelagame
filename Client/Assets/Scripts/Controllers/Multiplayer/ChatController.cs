using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Gameplay;
using VowelAServer.Shared.Models;
using RPC = VowelAServer.Shared.Models.RPC;


public class ChatController : StaticNetworkComponent
{
    public delegate void NewMessageHandler(ChatMessage chatMessage);
    public static event NewMessageHandler NewMessageEvent;
    
    [RPC] public static void SetNewMessage(ChatMessage chatMessage)
    {
        NewMessageEvent?.Invoke(chatMessage);
    }

    [RPC] public static void SetMessagesFromHistory(ChatHistory chatHistory)
    {
        foreach (var chatMessage in chatHistory.Messages) NewMessageEvent?.Invoke(chatMessage);
    }
}
