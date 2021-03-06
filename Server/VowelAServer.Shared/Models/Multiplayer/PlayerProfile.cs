using System;

namespace VowelAServer.Shared.Models.Multiplayer
{
    [Serializable]
    public class PlayerProfile
    {
        public int Id                   { get; set; }
        public string Login             { get; set; }
        public string Nickname          { get; set; }
        public int Rate                 { get; set; }
        public string ConnectedRoomName { get; set; }

        public PlayerProfile(string login)
        {
            Login    = login;
            Nickname = "New Player";
            Rate     = 0;
        }
    }
}