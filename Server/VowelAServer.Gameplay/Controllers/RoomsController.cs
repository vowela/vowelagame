using System;
using System.Collections.Generic;
using System.Linq;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Models;
using VowelAServer.Shared.Models.Multiplayer;
using VowelAServer.Utilities.Network;

namespace VowelAServer.Gameplay.Controllers
{
    /// <summary>
    /// Manages rooms on server, gives access to rooms API and manual rooms creation
    /// </summary>
    public class RoomsController : StaticNetworkObject
    {
        public static Queue<Room> AvailableRooms           = new Queue<Room>();   // New rooms appear here
        public static Dictionary<string, Room> ClosedRooms = new Dictionary<string, Room>();   // Closed rooms goes here, after finishing match the room should be removed
        private const byte DefaultMaxPlayersInRoom = 2;
        
        // Create a room with specified name and parameters
        private static Room CreateRoom(string name)
        {
            var newRoom = new Room
            {
                Name             = name, 
                ConnectedPlayers = new HashSet<int>(),
                MaxPlayersAmount = DefaultMaxPlayersInRoom
            };
            AvailableRooms.Enqueue(newRoom);
            return newRoom;
        }

        // Find an empty room with a specified criteria (e.g players amount)
        private static Room FindAvailableRoom(Player player)
        {
            // Firstly, find a room a player could be connected to
            if (player.ConnectedRoom != null) return player.ConnectedRoom;

            // If we're not connected to any room, try to find an empty room
            if (AvailableRooms.Any() && AvailableRooms.TryPeek(out var availableRoom)) return availableRoom;

            return null;
        }

        // Add player to room, close the room if need
        private static void JoinPlayerToRoom(Player player, Room room)
        {
            room.ConnectedPlayers.Add(player.Id);
            player.ConnectedRoom = room;
            if (room.ConnectedPlayers.Count >= room.MaxPlayersAmount)
            {
                AvailableRooms.Dequeue();
                ClosedRooms[room.Name] = room;
            }
            // Send player a request for joining the available room
            RPC(player.NetPeer, "RoomsController", "JoinedRoom", room);
        }
        
        [RPC] public static void CreateRoomManually(string name) => CreateRoom(name);
        
        public static void DestroyRoom(string roomName) => ClosedRooms.Remove(roomName);

        // If the player connected to any rooms, try connecting to them 
        [RPC] public static void GetRoomTryJoin(Player player)
        {
            if (player.ConnectedRoom != null) JoinPlayerToRoom(player, player.ConnectedRoom);
        }

        // Find an empty room, if there's no available rooms - create new
        [RPC] public static void TryJoinRoom(Player player)
        {
            var availableRoom = FindAvailableRoom(player) ?? CreateRoom(Guid.NewGuid().ToString());
            // Join player to available room
            if (availableRoom != null) JoinPlayerToRoom(player, availableRoom);
            else RPC(player.NetPeer, "RoomsController", "JoinedRoom", null);
        }
    }
}