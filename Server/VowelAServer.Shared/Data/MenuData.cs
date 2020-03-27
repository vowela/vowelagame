using System;
using System.Collections.Generic;

namespace VowelAServer.Shared.Data
{
    [Serializable]
    public class MenuButtonData
    {
        public string ButtonText;
        public string LuaCode;
    }

    [Serializable]
    public class MenuData
    {
        public List<MenuButtonData> ButtonData;
    }
}
