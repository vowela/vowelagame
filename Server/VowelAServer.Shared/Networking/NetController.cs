using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Utils;
using VowelAServer.Utilities.Logging;

namespace VowelAServer.Server.Controllers
{
    public abstract class NetController : IComparable<NetController>
    {
        public int NetId { get; set; }
        
        public int CompareTo(NetController other) => other == null ? 1 : NetId.CompareTo(other.NetId);
    }
}