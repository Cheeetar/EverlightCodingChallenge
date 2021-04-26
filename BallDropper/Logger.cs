using BallDropper.Enums;
using System;

namespace BallDropper
{
    public static class Logger
    {
        public static LogLevel LogLevel { get; set; }

        public static void LogVerboseDebug(string message, bool lineBreak = false)
        {
            if (LogLevel <= LogLevel.VerboseDebug)
            {
                message += lineBreak ? "\r\n" : "";
                Console.Write(message);
            }
        }

        public static void LogDebug(string message, bool lineBreak = false)
        {
            if (LogLevel <= LogLevel.Debug)
            {
                message += lineBreak ? "\r\n" : "";
                Console.Write(message);
            }
        }

        public static void LogInfo(string message, bool lineBreak = false)
        {
            if (LogLevel <= LogLevel.Info)
            {
                message += lineBreak ? "\r\n" : "";
                Console.Write(message);
            }
        }

        public static void Log(string message, bool lineBreak = false)
        {
            message += lineBreak ? "\r\n" : "";
            Console.Write(message);
        }
    }
}