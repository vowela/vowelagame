using System;
using System.Collections.Generic;
using System.Linq;
using VowelAServer.Server.Models;

namespace VowelAServer.Server.Controllers
{
    public abstract class NetController : IComparable<NetController>
    {
        public int NetId { get; set; }
        public Dictionary<string, Action> RPCMethods;

        protected NetController()
        {
            var methods = this.GetType().Assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(Attributes.RPCAttribute), false).Length > 0)
                .ToArray();
        }

        public int CompareTo(NetController other) => other == null ? 1 : NetId.CompareTo(other.NetId);
    }
}