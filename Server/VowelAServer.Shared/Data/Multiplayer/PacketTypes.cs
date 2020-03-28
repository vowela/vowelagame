using System;
namespace VowelAServer.Shared.Data.Multiplayer
{
    public enum PacketId : byte
    {
        LoginRequest = 1,
        LoginResponse = 2,
        LoginEvent = 3,
        LogoutEvent = 4
    }
}
