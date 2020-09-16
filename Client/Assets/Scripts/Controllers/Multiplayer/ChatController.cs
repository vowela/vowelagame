using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VowelAServer.Shared.Gameplay;
using VowelAServer.Shared.Models;
using RPC = VowelAServer.Shared.Models.RPC;

public class MessageEvent : UnityEvent<ChatMessage> { }

public class ChatController : StaticNetworkComponent
{
    public static MessageEvent OnNewMessage = new MessageEvent();
    
    [RPC] public static void SetNewMessage(ChatMessage chatMessage)
    {
        OnNewMessage?.Invoke(chatMessage);
    }

    [RPC] public static void SetMessagesFromHistory(ChatHistory chatHistory)
    {
        foreach (var chatMessage in chatHistory.Messages) OnNewMessage?.Invoke(chatMessage);
    }
}
