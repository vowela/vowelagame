﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VowelAServer.Shared.Models.Dtos
{
    [Serializable]
    public class UserDto
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
