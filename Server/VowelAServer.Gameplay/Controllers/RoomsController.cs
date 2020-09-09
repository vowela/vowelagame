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
                ConnectedPlayers = new List<int>(),
                MaxPlayersAmount = DefaultMaxPlayersInRoom,
                Teams            = new List<RoomTeam> { new RoomTeam(), new RoomTeam() }
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
            if (!room.ConnectedPlayers.Contains(player.Id))
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
            }

            // Send player a request for joining the available room
            RPC(player.NetPeer, "RoomsController", "JoinedRoom", room);
        }
        
        /// <summary> Send player profiles connected to concrete team </summary>
        private static void UpdateTeamListForPlayer(Player player, Room room)
        {
            // Update all teams on player
            for (var teamId = 0; teamId < room.Teams.Count; teamId++)
            {
                var playerProfileList = room.Teams[teamId].PlayersInTeam.Select(UserService.GetPlayerProfileById).ToList();
                RPC(player.NetPeer, "RoomsController", "TeamListUpdated", teamId, playerProfileList);
            }
        }
        
        /// <summary> Check if player is connected to any room which exists, if not - remove room from its records</summary>
        public static PlayerProfile ActualizePlayerProfile(PlayerProfile playerProfile, int playerId)
        {
            if (!string.IsNullOrEmpty(playerProfile.ConnectedRoomName))
            {
                if (Rooms.TryGetValue(playerProfile.ConnectedRoomName, out var room) && room.ConnectedPlayers.Contains(playerId)) { }
                else
                {
                    playerProfile.ConnectedRoomName = string.Empty;
                    UserService.UpdatePlayerProfileData(playerProfile);
                }
            }

            return playerProfile;
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
                {
                    Rooms[playerProfile.ConnectedRoomName].ConnectedPlayers.Remove(player.Id);
                    // Remove player from teams
                    foreach (var team in Rooms[playerProfile.ConnectedRoomName].Teams) team.PlayersInTeam.Remove(player.Id);
                }

                // Destroy info from player's profile
                playerProfile.ConnectedRoomName = string.Empty;
                UserService.UpdatePlayerProfileData(playerProfile);
            }
        }

        /// <summary> Update teams lists on player if he's connected to the room </summary>
        [RPC] public static void RequestTeams(Player player)
        {
            // Get current room which player connected to
            var playerProfile = UserService.GetPlayerProfileBySID(player.GetSId());
            if (playerProfile != null && playerProfile.ConnectedRoomName != string.Empty && Rooms.TryGetValue(playerProfile.ConnectedRoomName, out var connectedRoom))
                UpdateTeamListForPlayer(player, connectedRoom);
        }

        /// <summary> Add player to the team in its connected room by team id </summary>
        [RPC] public static void ChooseTeam(Player player, int teamId)
        {
            // Get current room which player connected to
            var playerProfile = UserService.GetPlayerProfileBySID(player.GetSId());
            if (playerProfile != null && playerProfile.ConnectedRoomName != string.Empty && Rooms.TryGetValue(playerProfile.ConnectedRoomName, out var connectedRoom))
            {
                if (connectedRoom != null && teamId < connectedRoom.Teams.Count)
                {
                    // Remove player from previous team (if any)
                    var teams = connectedRoom.Teams.Where(x => x.PlayersInTeam.Contains(player.Id));
                    foreach (var team in teams) team.PlayersInTeam.Remove(player.Id);
                    // Connect player to concrete room
                    connectedRoom.Teams[teamId].PlayersInTeam.Add(player.Id);
                    
                    UpdateTeamListForPlayer(player, connectedRoom);
                }
            }
        }
    }
}