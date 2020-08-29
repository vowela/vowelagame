using System;
using VowelAServer.Shared.Data.Enums;

namespace VowelAServer.Shared.Models.Multiplayer
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }

        public byte[] HashedPassword { get; set; }

        public byte[] Salt { get; set; }

        public Roles Roles { get; set; }
        public Guid SessionID { get; set; }
    }
}
