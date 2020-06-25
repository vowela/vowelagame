using System;
using System.Collections.Generic;
using System.Text;

namespace VowelAServer.Shared.Data.Enums
{
    [Flags]
    public enum Roles
    {
        User = 0,
        Tester = 1 << 0,
        Developer = 1 << 1,
        Admin = 1 << 2,
    }
}
