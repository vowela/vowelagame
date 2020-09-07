using System;
using System.Collections.Generic;
using ENet;
using VowelAServer.Shared.Models.Multiplayer;

namespace VowelAServer.Server.Models
{
    public class Player
    {
        private static readonly Dictionary<Guid, Player> sidPlayers = new Dictionary<Guid, Player>();
        public static Player Undefined(Peer peer) => new Player(peer);

        public int Id;
        public Peer NetPeer;
        public Room ConnectedRoom;
        
        public bool IsRegistered => sessionId != Guid.Empty;

        private Guid sessionId;

        // Creates undefined player
        public Player(Peer peer) { NetPeer = peer; }

        public static Player GetPlayerBySID(Guid sid)
        {
            if (sidPlayers.TryGetValue(sid, out var player)) return player;
            return null;
        }

        public void Unregister(Guid sessionId)
        {
            this.sessionId = Guid.Empty;
            Id             = -1;
            sidPlayers.Remove(sessionId);
        }
        
        public void Register(Guid sessionId, int playerId)
        {
            if (this.sessionId != Guid.Empty && sidPlayers.ContainsKey(this.sessionId))
                sidPlayers.Remove(this.sessionId);
            this.sessionId        = sessionId;
            Id                    = playerId;
            sidPlayers[sessionId] = this;
        }

        public Guid GetSId() => sessionId;
    }
}
