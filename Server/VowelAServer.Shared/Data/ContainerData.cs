using System;
using VowelAServer.Shared.Data.Math;

namespace VowelAServer.Shared.Data
{
    [Serializable]
    public class ContainerData : IEquatable<ContainerData>
    {
        public string Id              = Guid.NewGuid().ToString();
        public string AreaId          = Guid.NewGuid().ToString();
        public string ContainerName   = "";
        public string ClientLuaCode   = "";
        public string ServerLuaCode   = "";
        public string ParentId        = "";
        public Point Position         = new Point();
        public Vector Size            = new Vector();

        public bool Equals(ContainerData other) => other != null && other.Id.Equals(Id);
        public override int GetHashCode() => Id.GetHashCode();
    }
}
