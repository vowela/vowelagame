using System;
using System.Collections.Generic;
using System.Text;

namespace VowelAServer.Shared.Models.Db
{
    [Serializable]
    public class Row
    {
        public string Id { get; set; }

        public List<Item> Items { get; set; }

        public Row()
        {
            Items = new List<Item>();
        }
    }
}
