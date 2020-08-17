using System;

namespace VowelAServer.Shared.Models
{
    /// <summary> Creates an RPC that can be called on client or server. </summary>
    [AttributeUsage(AttributeTargets.Method)] public class RPC : Attribute { }
}