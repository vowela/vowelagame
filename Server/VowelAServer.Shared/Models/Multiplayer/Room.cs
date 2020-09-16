using System;
using System.Collections.Generic;

namespace VowelAServer.Shared.Models.Multiplayer
{
    [Serializable]
    public class Room
    {
        public string Name                          { get; set; }
        public byte MaxPlayersAmount                { get; set; }
        public List<int> ConnectedPlayers           { get; set; } = new List<int>();
        public List<RoomTeam> Teams                 { get; set; } = new List<RoomTeam>();
        public bool IsClosed => ConnectedPlayers.Count >= MaxPlayersAmount;
    }

    [Serializable]
    public class RoomTeam
    {
        public string Name             { get; set; }
        public List<int> PlayersInTeam { get; set; } = new List<int>();
    }
}