using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Debugging
{
    /// <summary>
    /// Message logging class.
    /// </summary>
    public class Log
    {
        public enum LogLevel
        {
            Info,
            Warn,
            Fail
        }

        // TODO: Investigate Log4Net.
        // TODO: Log items to a log file.
        // TODO: Log items to debug window in the game engine.
        // TODO: Log items to a debug HUD in the game engine.
        // TODO: Enable log items in a particular area of code only.

        private static void LogMessage(LogLevel level, string message)
        {
            string logLevel = string.Empty;
            switch (level)
            {
                case LogLevel.Info:
                    logLevel = "[INFO]";
                    break;
                case LogLevel.Warn:
                    logLevel = "[WARN]";
                    break;
                case LogLevel.Fail:
                    logLevel = "[FAIL]";
                    break;
            }

            Console.WriteLine(logLevel + " " + message);
        }

        public static void Info(string message)
        {
            LogMessage(LogLevel.Info, message);
        }

        public static void Info(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public static void Warn(string message)
        {
            LogMessage(LogLevel.Warn, message);
        }

        public static void Warn(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public static void Fail(string message)
        {
            LogMessage(LogLevel.Fail, message);
        }

        public static void Fail(string format, params object[] args)
        {
            Fail(string.Format(format, args));
        }
    }
}
