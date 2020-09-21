using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VowelAServer.Db.Services;
using VowelAServer.Shared.Models.Db;

namespace VowelAServer.Db.DbParser
{
    public class DbViwer
    {
        /// <summary>
        /// Returns names of all collections in the db.
        /// </summary>
        public static IEnumerable<string> GetCollectionNames()
        {
            using var db = new LiteDatabase(DbContext.DbPath);
            
            return db.GetCollectionNames();
        }

        /// <summary>
        /// Returns rows of a collection.
        /// </summary>
        public static List<Row> GetAllItemsInCollection(string name)
        {
            using var db = new LiteDatabase(DbContext.DbPath);

            var collection = db.GetCollection(name);

            var docs = collection.FindAll();

            var rows = new List<Row>();

            foreach (var doc in docs)
            {
                var elements = doc.GetElements();

                var row = new Row()
                {
                    // '_id' element is the first in the result of GetElements()
                    Id = elements.ToList()[0].Value.ToString()
                };

                foreach (var el in elements)
                {
                    var item = new Item()
                    {
                        Key = el.Key,
                        Value = el.Value.ToString(),
                        Type = BsonTypeToTypeCode(el.Value.Type),
                    };

                    row.Items.Add(item);
                }

                rows.Add(row);
            }

            return rows;
        }

        public static void UpsertItemInCollection(string collectionName, Row row)
        {
            using var db = new LiteDatabase(DbContext.DbPath);

            var collection = db.GetCollection(collectionName);

            var dic = new Dictionary<string, BsonValue>();

            foreach (var item in row.Items)
            {
                var converted = Convert.ChangeType(item.Value, item.Type);

                var value = new BsonValue(converted);

                dic.Add(item.Key, value);
            }

            collection.Upsert(new BsonDocument(dic));
        }

        private static TypeCode BsonTypeToTypeCode(BsonType type)
        {
            switch (type)
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
