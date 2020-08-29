using System;

namespace VowelAServer.Shared.Models.Multiplayer
{
    [Serializable]
    public class Room
    {
        public int Id          { get; set; }
        public string Name     { get; set; }
    }
}