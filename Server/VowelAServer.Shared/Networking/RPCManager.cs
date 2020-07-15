using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using VowelAServer.Server.Controllers;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Utils;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Shared.Networking
{
    public static class RPCManager
    {
        public static Dictionary<(string targetName, string methodName), MethodInfo> RPCMethods = new Dictionary<(string, string), MethodInfo>();

        public static void GetOrBuildLookup()
        {
            // Putting all controllers from all assemblies together in one poll
            var netControllers = typeof(NetController).DerivedTypes();
            foreach (var netControllerType in netControllers)
            {
                var methodRPCs = netControllerType.MethodsWithAttribute<Attributes.RPCAttribute>().ToArray();
                foreach (var method in methodRPCs)
                    RPCMethods[(netControllerType.Name, method.Name)] = method;
            }
        }
    }
}