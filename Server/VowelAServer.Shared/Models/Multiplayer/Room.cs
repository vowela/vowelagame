using System;
using System.Collections.Generic;

namespace VowelAServer.Shared.Models.Multiplayer
{
    [Serializable]
    public class Room
    {
        public string Name                          { get; set; }
        public byte MaxPlayersAmount                { get; set; }
        public HashSet<int> ConnectedPlayers        { get; set; }
        public bool IsClosed => ConnectedPlayers.Count >= MaxPlayersAmount;
    }
}