using System;
namespace VowelAServer.Shared.Data.Multiplayer
{
    public enum PacketId : byte
    {
        None                  = 0,
        PlayerDisconnectEvent = 1,
        SceneDataRequest      = 2,
        SceneDataResponse     = 3,
        MenuRequest           = 4,
        MenuResponse          = 5,
        ObjectChangesRequest  = 6,
        ObjectChangesEvent    = 7
    }
}
