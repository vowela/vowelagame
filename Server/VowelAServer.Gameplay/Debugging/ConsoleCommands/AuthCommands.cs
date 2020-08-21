using VowelAServer.Utilities.Logging;

namespace VowelAServer.Gameplay.Debugging.ConsoleCommands
{
    public class AuthCommands : IDevCommands
    {
        public string GroupName { get; set; } = "Auth";
        
        public static void Register(string data)
        {
            Logger.WriteError("Someone joined here");
        }

    }
}