using System;
using LiteDB;
using VowelAServer.Server.Models;
using VowelAServer.Shared.Models;

namespace VowelAServer.Db.Services
{
    public class UserService
    {
        /// <summary> Create user in database or return false if exists </summary>
        public static bool CreateUser(User user)
        {
            using var db = new LiteDatabase(DbContext.DbPath);
            var users = db.GetCollection<User>("users");

            users.EnsureIndex(x => x.Login);

            var addedUser = users.FindOne(x => x.Login == user.Login);
            if (addedUser != null) return false;

            users.Insert(user);
            return users.FindOne(x => x.Login == user.Login) != null;
        }

        /// <summary> Update user's data in database and return result </summary>
        public static bool UpdateUserData(User user)
        {
            using var db = new LiteDatabase(DbContext.DbPath);
            var users = db.GetCollection<User>("users");

            users.EnsureIndex(x => x.Login);
            
            return users.Update(user);
        }

        public static User GetUserBySID(Guid sid)
        {
            using var db = new LiteDatabase(DbContext.DbPath);
            var users = db.GetCollection<User>("users");

            users.EnsureIndex(x => x.Login);

            return users.FindOne(x => x.SessionID == sid);
        }

        public static User GetUserByLogin(string login)
        {
            using var db = new LiteDatabase(DbContext.DbPath);
            var users = db.GetCollection<User>("users");

            users.EnsureIndex(x => x.Login);

            return users.FindOne(x => x.Login == login);
        }

        public static PlayerProfile GetPlayerProfileBySID(Guid sid)
        {
            using var db = new LiteDatabase(DbContext.DbPath);
            var users    = db.GetCollection<User>("users");

            var user = users.FindOne(x => x.SessionID == sid);
            if (user == null) return null;
            
            var playerProfiles = db.GetCollection<PlayerProfile>("playerProfiles");
            playerProfiles.EnsureIndex(x => x.Login);

            var playerProfile = playerProfiles.FindOne(x => x.Login == user.Login);
            if (playerProfile == null)
            {
                playerProfile = new PlayerProfile(user.Login);
                playerProfiles.Insert(playerProfile);
            }

            return playerProfile;
        }

        public static bool UpdatePlayerProfileData(PlayerProfile playerProfile)
        {   
            using var db       = new LiteDatabase(DbContext.DbPath);
            var playerProfiles = db.GetCollection<PlayerProfile>("playerProfiles");

            return playerProfiles.Update(playerProfile);
        }
    }
}
