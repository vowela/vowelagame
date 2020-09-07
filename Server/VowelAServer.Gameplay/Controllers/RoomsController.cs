using System;
using System.Collections.Generic;
using System.Linq;
using VowelAServer.Db.Services;
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
        public static Dictionary<string, Room> Rooms = new Dictionary<string, Room>();   // Whole rooms list
        public static Queue<Room> AvailableRooms     = new Queue<Room>();                // New rooms appear here
        private const byte DefaultMaxPlayersInRoom   = 2;
        
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
            Rooms[newRoom.Name] = newRoom;
            return newRoom;
        }

        // Find an empty room with a specified criteria (e.g players amount)
        private static Room FindAvailableRoom(Player player)
        {
            // Firstly, find a room a player could be connected to
            var playerProfile = UserService.GetPlayerProfileBySID(player.GetSId());
            if (playerProfile?.ConnectedRoomName != null)
            {
                if (Rooms.ContainsKey(playerProfile.ConnectedRoomName))
                    return Rooms[playerProfile.ConnectedRoomName];

                // If no room with saved name found, then erase it from player profile
                playerProfile.ConnectedRoomName = "";
                UserService.UpdatePlayerProfileData(playerProfile);
            }

            // If we're not connected to any room, try to find an empty room
            if (AvailableRooms.Any() && AvailableRooms.TryPeek(out var availableRoom)) return availableRoom;

            return null;
        }

        // Add player to room, close the room if need
        private static void JoinPlayerToRoom(Player player, Room room)
        {
            room.ConnectedPlayers.Add(player.Id);
            // Update connected room name in player's profile
            var playerProfile = UserService.GetPlayerProfileBySID(player.GetSId());
            if (playerProfile != null)
            {
                playerProfile.ConnectedRoomName = room.Name;
                UserService.UpdatePlayerProfileData(playerProfile);
            }
            // Remove from availability
            if (room.IsClosed) AvailableRooms.Dequeue();
            // Send player a request for joining the available room
            RPC(player.NetPeer, "RoomsController", "JoinedRoom", room);
        }
        
        [RPC] public static void CreateRoomManually(string name) => CreateRoom(name);

        // Room could be destroyed only if it's already closed
        public static void DestroyRoom(string roomName)
        {
            if (Rooms.TryGetValue(roomName, out var room) && room.IsClosed) Rooms.Remove(roomName);
        }

        // Find an empty room, if there's no available rooms - create new
        [RPC] public static void TryJoinRoom(Player player)
        {
            var availableRoom = FindAvailableRoom(player) ?? CreateRoom(Guid.NewGuid().ToString());
            // Join player to available room
            if (availableRoom != null) JoinPlayerToRoom(player, availableRoom);
            else RPC(player.NetPeer, "RoomsController", "JoinedRoom", null);
        }

        [RPC] public static void TryLeaveRoom(Player player)
        {
            var playerProfile = UserService.GetPlayerProfileBySID(player.GetSId());
            if (playerProfile != null && playerProfile.ConnectedRoomName != string.Empty)
            {
                // Remove player from its room
                if (Rooms.ContainsKey(playerProfile.ConnectedRoomName))
                    Rooms[playerProfile.ConnectedRoomName].ConnectedPlayers.Remove(player.Id);
                // Destroy info from player's profile
                playerProfile.ConnectedRoomName = string.Empty;
                UserService.UpdatePlayerProfileData(playerProfile);
            }
        }
    }
}