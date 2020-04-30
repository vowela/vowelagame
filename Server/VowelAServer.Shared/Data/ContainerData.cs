using System;
using VowelAServer.Shared.Data.Math;

namespace VowelAServer.Shared.Data
{
    [Serializable]
    public class ContainerData : IEquatable<ContainerData>
    {
        public string Id              = Guid.NewGuid().ToString();
        public string ContainerName   = "";
        public string ClientLuaCode   = "";
        public string ServerLuaCode   = "";
        public string ParentId        = "";
        public Vector Position        = new Vector();
        public Vector Size            = new Vector();

        public bool Equals(ContainerData other)
        {
            return other != null && other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
