using System;
using System.Collections.Generic;

namespace VowelAServer.Shared.Data
{
    [Serializable]
    public class SceneData
    {
        public HashSet<ContainerData> Added   = new HashSet<ContainerData>();
        public HashSet<ContainerData> Changed = new HashSet<ContainerData>();
        public HashSet<string> Removed        = new HashSet<string>();
    }
}
