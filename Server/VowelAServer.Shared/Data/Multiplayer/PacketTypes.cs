using System;
namespace VowelAServer.Shared.Data.Multiplayer
{
    public enum PacketId : byte
    {
        None                 = 0,
        LoginRequest         = 1,
        LoginResponse        = 2,
        LoginEvent           = 3,
        LogoutEvent          = 4,
        LuaRequest           = 5,
        SceneDataRequest     = 6,
        SceneDataResponse    = 7,
        MenuRequest          = 8,
        MenuResponse         = 9,
        ObjectChangesRequest = 10,
        ObjectChangesEvent   = 11
    }
}
