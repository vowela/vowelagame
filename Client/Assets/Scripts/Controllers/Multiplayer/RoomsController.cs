using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VowelAServer.Shared.Models.Multiplayer;
using RPC = VowelAServer.Shared.Models.RPC;

public class RoomEvent : UnityEvent<Room> { }
public class TeamPlayersUpdateEvent : UnityEvent<(int teamId, List<PlayerProfile> playerProfiles)> { }

public class RoomsController : StaticNetworkComponent
{
    public static UnityEvent OnStopRoomSearch                 = new UnityEvent();
    
    public static RoomEvent OnConnectedToRoom                 = new RoomEvent();
    public static TeamPlayersUpdateEvent OnTeamPlayersUpdated = new TeamPlayersUpdateEvent();

    // Alert everything that we've just accepted connecting to room
    [RPC] public static void JoinedRoom(Room room)
    {
        if (room == null) OnStopRoomSearch?.Invoke();
        else              OnConnectedToRoom?.Invoke(room);
    }

    [RPC] public static void TeamListUpdated(int teamId, List<PlayerProfile> playerProfiles)
    {
        OnTeamPlayersUpdated?.Invoke((teamId, playerProfiles));
    }

    public static void RequestTeams()       => RPC("RoomsController", "RequestTeams");

    public static void LeaveRoom()          => RPC("RoomsController", "TryLeaveRoom");

    public static void StartRoomSearch()    => RPC("RoomsController", "TryJoinRoom");

    public static void ChooseTeamId(int id) => RPC("RoomsController", "ChooseTeam", id);
}
