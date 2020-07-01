using VowelAServer.Server.Controllers;
using VowelAServer.Server.Models;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Gameplay.Utilities
{
    using RPC = Attributes.RPCAttribute;
    
    // This class adds support for server-client console which could receive and activate different commands
    public class DevConsole : NetController
    {
        [RPC]
        public void GetCommand()
        {
            Logger.Write("Hey, someone writes to me via RPC!");
        }
    }
}