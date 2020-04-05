using System;
using System.Collections.Generic;

namespace VowelAServer.Shared.Data
{
    [Serializable]
    public class SceneData
    {
        public List<ContainerData> Added = new List<ContainerData>();
        public List<string> Removed      = new List<string>();
    }
}
