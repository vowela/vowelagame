using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VowelAServer.Db.Services;
using VowelAServer.Gameplay.Debugging;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Gameplay;
using VowelAServer.Shared.Models;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Gameplay.Controllers
{
    public class ChatManager : StaticNetworkObject
    {
        public static List<ChatMessage> Messages = new List<ChatMessage>();
        
        [RPC] public static void SendToAll(Player player, string message)
        {
            if (!player.IsRegistered) return;
            Messages ??= new List<ChatMessage>();

            var newMessage = new ChatMessage
            {
                Sender = UserService.GetPlayerProfileBySID(player.GetSId()),
                Text   = message
            };
            Messages.Add(newMessage);
            RPC("ChatController", "SetNewMessage", newMessage);
        }
        
        [RPC] public static void GetMessagesFromHistory(Player player, int offset, int count)
        {
            Messages ??= new List<ChatMessage>();
            var newChatHistory = new ChatHistory {Messages = Messages.SkipLast(offset).TakeLast(count).ToList()};
            RPC(player.NetPeer, "ChatController", "SetMessagesFromHistory", newChatHistory);
        }
    }
}