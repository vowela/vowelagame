using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VowelAServer.Shared.Models;
using VowelAServer.Shared.Networking;
using RPC = VowelAServer.Shared.Models.RPC;

public class RPCManager : MonoBehaviour
{
    public static Dictionary<(string targetName, string methodName), MethodInfo> RPCMethods = new Dictionary<(string, string), MethodInfo>();

    public void Start()
    {
        // Initial building lookup for rpc's
        GetOrBuildLookup();
    }
    
    public static void GetOrBuildLookup()
    {
        // Putting all controllers from all assemblies together in one poll
        var netControllers = typeof(NetworkComponent).DerivedTypes();
        foreach (var netControllerType in netControllers)
        {
            var methodRPCs = netControllerType.MethodsWithAttribute<RPC>().ToArray();
            foreach (var method in methodRPCs) RPCMethods[(netControllerType.Name, method.Name)] = method;
        }

        var netStaticControllers = typeof(StaticNetworkComponent).DerivedTypes();
        foreach (var netStaticControllerType in netStaticControllers)
        {
            var methodRPCs = netStaticControllerType.MethodsWithAttribute<RPC>().ToArray();
            foreach (var method in methodRPCs) RPCMethods[(netStaticControllerType.Name, method.Name)] = method;
        }
    }
}
