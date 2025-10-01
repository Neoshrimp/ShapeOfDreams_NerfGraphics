using System;
using System.Collections.Generic;
using System.Text;

namespace SoD_NerfGraphics
{
    internal static class Log
    {
        public static void LogMessage(object o) => Plugin.log.LogMessage(o);
        public static void LogInfo(object o) => Plugin.log.LogInfo(o);
        public static void LogDebug(object o) => Plugin.log.LogDebug(o);
        public static void LogWarning(object o) => Plugin.log.LogWarning(o);
        public static void LogError(object o) => Plugin.log.LogError(o);
        public static void LogFatal(object o) => Plugin.log.LogFatal(o);


    }
}
