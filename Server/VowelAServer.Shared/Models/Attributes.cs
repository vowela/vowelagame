using System;

namespace VowelAServer.Server.Models
{
    public class Attributes
    {
        /// <summary> Creates an RPC that can be called on the client. </summary>
        [AttributeUsage(AttributeTargets.Method)] public class RPCAttribute : Attribute { }
        
    }
}