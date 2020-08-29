using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VowelAServer.Shared.Gameplay;
using RPC = VowelAServer.Shared.Models.RPC;

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
            var text = ChatBox.text;
            ChatBox.text = "";
            
            // Process developer command
            if (text.StartsWith("/"))
            {
                var devCommand = text.Split(' ');
                if (devCommand.Length >= 2)
                {
                    var groupName   = devCommand[0].Remove(0, 1);
                    var commandName = devCommand[1];
                    var arguments   = devCommand.Length > 2 ? devCommand[2].Split(',') : new string[] {};
                    StaticNetworkComponent.RPC("DeveloperConsole", "ProcessCommand", groupName, commandName, arguments);
                    return;
                }
            }

            SendMessageToServer(text);
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
