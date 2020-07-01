using System;

namespace VowelAServer.Utilities.Logging
{
    public static class Logger
    {
        private static void WriteToConsole(string message, ConsoleColor color = ConsoleColor.White)
        {
            var oldColor            = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.Now:T}] {message}");
            Console.ForegroundColor = oldColor;
        }

        public static void Write(string message)        => WriteToConsole(message);
        public static void WriteError(string message)   => WriteToConsole(message, ConsoleColor.Red);
        public static void WriteSuccess(string message) => WriteToConsole(message, ConsoleColor.Green);
        public static void WriteWarning(string message) => WriteToConsole(message, ConsoleColor.Yellow);
    }
}