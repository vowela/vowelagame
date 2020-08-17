using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VowelAServer.Shared.Models;
using VowelAServer.Shared.Networking;
using VowelAServer.Utilities.Helpers;

namespace VowelAServer.Server.Managers
{
    public static class RPCManager
    {
        public static Dictionary<(string targetName, string methodName), MethodInfo> RPCMethods = new Dictionary<(string, string), MethodInfo>();
        
        public static void GetOrBuildLookup()
        {
            // Putting all controllers from all assemblies together in one poll
            var netControllers = typeof(NetObject).DerivedTypes();
            foreach (var netControllerType in netControllers)
            {
                var methodRPCs = netControllerType.MethodsWithAttribute<RPC>().ToArray();
                foreach (var method in methodRPCs) RPCMethods[(netControllerType.Name, method.Name)] = method;
            }

            var netStaticControllers = typeof(StaticNetObject).DerivedTypes();
            foreach (var netStaticControllerType in netStaticControllers)
            {
                var methodRPCs = netStaticControllerType.MethodsWithAttribute<RPC>().ToArray();
                foreach (var method in methodRPCs) RPCMethods[(netStaticControllerType.Name, method.Name)] = method;
            }
        }
    }
}