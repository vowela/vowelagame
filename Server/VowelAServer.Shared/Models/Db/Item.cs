using System;
using System.Collections.Generic;
using System.Text;

namespace VowelAServer.Shared.Models.Db
{
    public class Item
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public TypeCode Type { get; set; }
    }
}
