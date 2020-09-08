using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace VowelAServer.Shared.Models.Db
{
    public class Item
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public BsonType Type { get; set; }

        public TypeCode BsonTypeToTypeCode()
        {
            switch(Type)
            {
                case BsonType.Boolean:
                    return TypeCode.Boolean;
                case BsonType.Int32:
                    return TypeCode.Int32;
                case BsonType.Double:
                    return TypeCode.Double;
                case BsonType.DateTime:
                    return TypeCode.DateTime;
                case BsonType.String:
                    return TypeCode.String;
                default:
                    return TypeCode.Object;
            }
        }
    }
}
