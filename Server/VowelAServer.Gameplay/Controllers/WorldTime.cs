using System;
using System.Diagnostics;

namespace VowelAServer.Gameplay.Controllers
{
    public class WorldTime
    {
        private static WorldTime instance;
        private static object syncRoot = new object();

        public static WorldTime Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null) instance = new WorldTime();
                }
            }
            return instance;
        }

        private Stopwatch stopwatch;

        public WorldTime()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        // Milliseconds
        public float GetWorldTime()
        {
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
