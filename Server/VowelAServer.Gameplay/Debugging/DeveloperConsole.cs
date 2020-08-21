using System;
using System.Collections.Generic;
using System.Reflection;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Models;
using VowelAServer.Utilities.Helpers;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Gameplay.Debugging
{
    public interface IDevCommands
    {
        string GroupName { get; set; }
    }
    
    public class DeveloperConsole : StaticNetworkObject
    {
        private static readonly Dictionary<(string groupName, string methodName), MethodInfo> commands = new Dictionary<(string, string), MethodInfo>();
        
        static DeveloperConsole()
        {
            var commandClasses = typeof(IDevCommands).DerivedTypes();
            foreach (var commandClass in commandClasses)
            {
                var groupName = commandClass.GetProperty(nameof(IDevCommands.GroupName))?.GetValue(Activator.CreateInstance(commandClass))?.ToString();
                if (groupName == null) continue;
                var methods   = commandClass.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var method in methods) commands[(groupName.ToUpper(), method.Name.ToUpper())] = method;
            }
        }
        
        [RPC]
        public static void ProcessCommand(Player player, string groupName, string commandName, params object[] args)
        {
            if (commands.TryGetValue((groupName.ToUpper(), commandName.ToUpper()), out var commandInfo))
                commandInfo.Invoke(null, args);
        }
    }
}