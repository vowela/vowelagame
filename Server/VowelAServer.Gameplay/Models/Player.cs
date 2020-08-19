using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VowelAServer.Server.Models
{
    public class Player: IEquatable<Player>
    {
        private static readonly HashSet<Player> players = new HashSet<Player>();
        
        public static Player Undefined() => new Player(-1);
        
        public readonly int Id;
        public int NetworkID;
        
        public Player(int playerId)
        {
            this.Id = playerId;
        }

        public bool Equals([AllowNull] Player other)
        {
            return other != null && this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static Player GetPlayerByNetID(int netId)
        {
            if (players.TryGetValue(new Player(netId), out var player)) return player;
            var undefined = Undefined();
            undefined.NetworkID = netId;
            return undefined;
        } 
    }
}
