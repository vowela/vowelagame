using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using VowelAServer.Shared.Utils;

namespace VowelAServer.Gameplay.Controllers
{
    public class WorldTime
    {
        private static WorldTime instance;
        private static object syncRoot = new object();

        private static string timeDataPath;

        public static string TimeDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(timeDataPath))
                {
                    timeDataPath = Utils.GetDirPath("Storage") + @"/time.json";
                }

                return timeDataPath;
            }
        }

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

        private DateTime startTime;

        public WorldTime()
        {
            stopwatch = new Stopwatch();
        }

        public void Start(DateTime dateTime)
        {
            startTime = dateTime;

            stopwatch.Start();
        }

        public DateTime GetCurrentTime() => startTime.AddMilliseconds(stopwatch.ElapsedMilliseconds);

        // Milliseconds
        public float GetWorldTime() => stopwatch.ElapsedMilliseconds;

        public float GetWorldTimeSeconds() => GetWorldTime() / 1000;

        public float GetWorldTimeMinutes() => GetWorldTimeSeconds() / 60;

        public float GetWorldTimeHours() => GetWorldTimeMinutes() / 60;

        public float GetWorldTimeDays() => GetWorldTimeHours() / 24;

        public void SaveTime()
        {
            var time = new Time()
            {
                Ticks = GetCurrentTime().Ticks,
            };

            File.WriteAllText(TimeDataPath, JsonConvert.SerializeObject(time));
        }

        public static DateTime GetSavedTime()
        {
            var file = File.ReadAllText(TimeDataPath);
            if (string.IsNullOrEmpty(file)) return DateTime.Now;

            var time = JsonConvert.DeserializeObject<Time>(file);

            return new DateTime(time.Ticks);
        }
    }

    class Time
    {
        public long Ticks;
    }
}
