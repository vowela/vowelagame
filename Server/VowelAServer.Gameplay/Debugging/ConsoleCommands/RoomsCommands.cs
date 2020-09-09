using VowelAServer.Gameplay.Controllers;
using VowelAServer.Server.Models;

namespace VowelAServer.Gameplay.Debugging.ConsoleCommands
{
    public class RoomsCommands : IDevCommands
    {
        public string GroupName { get; set; } = "Rooms";

        public static void CreateRoom(string name)     => RoomsController.CreateRoomManually(name);
    }
}