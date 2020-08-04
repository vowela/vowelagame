using System;

namespace VowelAServer.Shared.Controllers
{
    public abstract class NetController : IComparable<NetController>
    {
        public int NetId { get; set; }
        
        public int CompareTo(NetController other) => other == null ? 1 : NetId.CompareTo(other.NetId);
    }
}