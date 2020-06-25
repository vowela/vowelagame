using System;
using System.Collections.Generic;
using System.ComponentModel;
using VowelAServer.Shared.Data.Math;

namespace VowelAServer.Shared.Data
{
    [Serializable]
    public class ContainerArea : IEquatable<ContainerArea>
    {
        public string Id                 = Guid.NewGuid().ToString();
        public List<string> ContainerIds = new List<string>();
        public Vector CenterPosition;

        public ContainerArea() {}
        public ContainerArea(Vector centerPosition)
        {
            CenterPosition = centerPosition;
        }
        
        public bool Equals(ContainerArea other) => other != null && other.Id.Equals(Id);
        public override int GetHashCode() => Id.GetHashCode();
    }
}