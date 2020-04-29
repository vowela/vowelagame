using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VowelAServer.Shared.Utils
{
    public class Utils
    {
        public static string GetDirPath(string dirName)
        {
            var path = "";

            var lastParent = Directory.GetParent(Path.Combine(Directory.GetCurrentDirectory()));
            var done = false;
            for (var i = 0; i < 5; i++)
            {
                foreach (var dir in lastParent.GetDirectories())
                {
                    if (dir.Name == dirName)
                    {
                        done = true;
                        path = dir.FullName.ToString();
                        break;
                    }
                }

                if (done) break;

                lastParent = lastParent.Parent;
                path = lastParent.ToString();
            }

            return path;
        }
    }
}
