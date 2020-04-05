using System;

namespace VowelAServer.Shared.Data
{
    [Serializable]
    public class ContainerData
    {
        public string Id              = Guid.NewGuid().ToString();
        public string ContainerName   = "";
        public string LuaCode         = "";
        public string ParentId        = "";
    }
}
