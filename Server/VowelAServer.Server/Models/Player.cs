using System;
using System.Diagnostics.CodeAnalysis;

namespace VowelAServer.Server.Models
{
    public class Player: IEquatable<Player>
    {
        public uint Id;

        public Player(uint playerId)
        {
            this.Id = playerId;
        }

        public bool Equals([AllowNull] Player other)
        {
            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
