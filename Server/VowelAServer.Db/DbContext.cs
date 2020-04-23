using System;
using System.Collections.Generic;
using System.Text;
using VowelAServer.Shared.Utils;

namespace VowelAServer.Db
{
    public class DbContext
    {
        private static string dbPath = "";

        public static string DbPath
        {
            get 
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    dbPath = Utils.GetDirPath("Storage") + @"\\VowelaData.db";
                }

                return dbPath;
            }
        }
    }
}
