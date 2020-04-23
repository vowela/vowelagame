using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using VowelAServer.Server.Models;

namespace VowelAServer.Db.Services
{
    public class UserService
    {
        public static void CreateUser(User user)
        {
            using (var db = new LiteDatabase(DbContext.DbPath))
            {
                var users = db.GetCollection<User>("users");

                users.EnsureIndex(x => x.Login);

                var addedUser = users.FindOne(x => x.Login == user.Login);

                if (addedUser != null) return;

                users.Insert(user);
            }
        }

        public static User GetUserByLogin(string login)
        {
            using (var db = new LiteDatabase(@"VowelAData.db"))
            {
                var users = db.GetCollection<User>("users");

                users.EnsureIndex(x => x.Login);

                return users.FindOne(x => x.Login == login);
            }
        }
    }
}
