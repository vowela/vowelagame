using System;
using System.Collections.Generic;
using ENet;

namespace VowelAServer.Server.Models
{
    public class Player
    {
        private static readonly Dictionary<Guid, Player> sidPlayers = new Dictionary<Guid, Player>();
        public static Player Undefined(Peer peer) => new Player(peer);
        
        public bool IsRegistered => sessionId != Guid.Empty;
        
        public Peer NetPeer;
        
        private Guid sessionId;

        // Creates undefined player
        public Player(Peer peer) { NetPeer = peer;}

        public static Player GetPlayerBySID(Guid sid)
        {
            if (sidPlayers.TryGetValue(sid, out var player)) return player;
            return null;
        }

        public void Unregister(Guid sessionId)
        {
            this.sessionId = Guid.Empty;
            sidPlayers.Remove(sessionId);
        }
        
        public void Register(Guid sessionId)
        {
            if (this.sessionId != Guid.Empty && sidPlayers.ContainsKey(this.sessionId))
                sidPlayers.Remove(this.sessionId);
            this.sessionId        = sessionId;
            sidPlayers[sessionId] = this;
        }

        public Guid GetSId() => sessionId;
    }
}
