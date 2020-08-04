using VowelAServer.Server.Models;
using VowelAServer.Shared.Controllers;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Gameplay.Utilities
{
    using RPC = Attributes.RPCAttribute;
    
    // This class adds support for server-client console which could receive and activate different commands
    public class DevConsole : NetController
    {
        [RPC]
        public static void GetCommand(string message)
        {
            Logger.WriteSuccess($"Client just sent me: {message}");
        }
    }
}