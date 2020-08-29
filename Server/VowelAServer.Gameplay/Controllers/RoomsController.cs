using System.Collections.Generic;
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
        public static List<Room> Rooms = new List<Room>();
        
        private static void CreateRoom(string name)
        {
            var newRoom = new Room {Id = Rooms.Count, Name = name};
            Rooms.Add(newRoom);
        }

        [RPC] public static void CreateRoomManually(string name)
        {
            CreateRoom(name);
        }
        
        [RPC] public static void GetRoomsList(Player player) => RPC(player.NetPeer, "RoomsController", "UpdateRoomsList", Rooms);
    }
}