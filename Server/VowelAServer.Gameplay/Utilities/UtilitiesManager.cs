using VowelAServer.Server.Models;
using VowelAServer.Shared.Utils;

namespace VowelAServer.Gameplay.Utilities
{
    public class UtilitiesManager
    {
        private DevConsole console;
        public void CreateUtilities()
        {
            console = new DevConsole();
        }
    }
}