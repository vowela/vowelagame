﻿using System;
namespace VowelAServer.Shared.Data.Multiplayer
{
    public enum PacketId : byte
    {
        None          = 0,
        LoginRequest  = 1,
        LoginResponse = 2,
        LoginEvent    = 3,
        LogoutEvent   = 4,
        LuaRequest    = 5
    }
}
