using System;
using System.Collections.Generic;
using System.Text;
using VowelAServer.Shared.Data.Enums;

namespace VowelAServer.Server.Models
{
    public class User
    {
        public string Login { get; set; }

        public string HashedPassword { get; set; }

        public Roles Roles { get; set; }
    }
}
