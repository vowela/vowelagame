using System;
namespace VowelAServer.Shared.Data.Multiplayer
{
    public enum PacketId : byte
    {
        None                  = 0,
        PlayerDisconnectEvent = 1,
        MenuRequest           = 2,
        MenuResponse          = 3,
        ObjectChangesRequest  = 4,
        ObjectChangesEvent    = 5,
        AreaRequest           = 6,
        AreaResponse          = 7
    }
}
