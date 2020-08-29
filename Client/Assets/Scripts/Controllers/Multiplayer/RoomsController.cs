using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VowelAServer.Shared.Models;
using VowelAServer.Shared.Models.Multiplayer;

public class RoomsController : StaticNetworkComponent
{
    [VowelAServer.Shared.Models.RPC] public static void OnRoomCreated()
    {
        
    }
    
    [VowelAServer.Shared.Models.RPC] public static void UpdateRoomsList(List<Room> rooms)
    {
        Debug.Log(rooms.Count);
    }
}
