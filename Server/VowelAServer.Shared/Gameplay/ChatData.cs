using System;
using System.Collections.Generic;
using VowelAServer.Shared.Models;
using VowelAServer.Shared.Models.Multiplayer;

namespace VowelAServer.Shared.Gameplay
{
    [Serializable]
    public class ChatMessage
    {
        public PlayerProfile Sender;
        public string Text;
    }

    [Serializable]
    public class ChatHistory
    {
        public List<ChatMessage> Messages;
    }
}