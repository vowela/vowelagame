using System;
namespace VowelAServer.Shared.Data
{
    public class ContainerObject
    {
        public Guid UniqueId        { get; set; }
        public string LuaCode       { get; set; }
        public string ContainerName { get; set; }

        public ContainerObject() { }
    }
}
