using System;

namespace VowelAServer.Shared.Models
{
    [Serializable]
    public class PlayerProfile
    {
        public int Id          { get; set; }
        public string Login    { get; set; }
        public string Nickname { get; set; }
        public int Rate        { get; set; }

        public PlayerProfile(string login)
        {
            Login    = login;
            Nickname = "New Player";
            Rate     = 0;
        }
    }
}