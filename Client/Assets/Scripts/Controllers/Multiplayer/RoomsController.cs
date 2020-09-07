using UnityEngine.Events;
using VowelAServer.Shared.Models.Multiplayer;
using RPC = VowelAServer.Shared.Models.RPC;

public class RoomEvent : UnityEvent<Room> { }

public class RoomsController : StaticNetworkComponent
{
    public static UnityEvent OnStopRoomSearch = new UnityEvent();
    
    public static RoomEvent OnConnectedToRoom = new RoomEvent();

    // Alert everything that we've just accepted connecting to room
    [RPC] public static void JoinedRoom(Room room)
    {
        if (room == null) OnStopRoomSearch?.Invoke();
        else              OnConnectedToRoom?.Invoke(room);
    }

    public static void LeaveRoom()
    {
        RPC("RoomsController", "TryLeaveRoom");
    }

    public static void StartRoomSearch()
    {
        RPC("RoomsController", "TryJoinRoom");
    }
}
