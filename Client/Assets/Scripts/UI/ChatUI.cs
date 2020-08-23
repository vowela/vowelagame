using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Gameplay;

public class ClientChatMessage : ChatMessage
{
    public Text TextObject;
}

public class ChatUI : MonoBehaviour
{
    private const int maxMessages = 25;
    public List<ClientChatMessage> ClientMessages = new List<ClientChatMessage>();
    
    public GameObject ChatPanel, TextObject;
    public InputField ChatBox;
    public ScrollRect ChatScrollRect;

    private void Start()
    {
        ChatController.NewMessageEvent += SetNewMessage;
        ClearMessages();
        TakeMessages();
    }

    private void Update()
    {
        if (ChatBox.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToServer(ChatBox.text);
            ChatBox.text = "";
        }
    }

    private void SetNewMessage(ChatMessage chatmessage)
    {
        if (ClientMessages.Count >= maxMessages)
        {
            Destroy(ClientMessages[0].TextObject.gameObject);
            ClientMessages.Remove(ClientMessages[0]);
        }
        
        var newMessageObject = Instantiate(TextObject, ChatPanel.transform);
        var newMessage = new ClientChatMessage
        {
            Text       = chatmessage.Sender.Nickname + ": " + chatmessage.Text,
            TextObject = newMessageObject.GetComponent<Text>()
        };
        newMessage.TextObject.text = newMessage.Text;
        
        ChatScrollRect.velocity = new Vector2(0f,1000f);
    }
    
    public void ClearMessages() { }

    public void SendMessageToServer(string text) => StaticNetworkComponent.RPC("ChatManager", "SendToAll", text);
    public void TakeMessages() => StaticNetworkComponent.RPC("ChatManager", "GetMessagesFromHistory", 0, 25);
}
