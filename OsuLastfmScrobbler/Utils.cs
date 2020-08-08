using Sync.Tools;

namespace OsuLastfmScrobbler
{
    public static class Logger
    {
        static Logger<OsuScrobblerPlugin> logger=new Logger<OsuScrobblerPlugin>();

        public static void Info(string message) => logger.LogInfomation(message);

        public static void Debug(string message)
        {
            if (Settings.Instance.DebugMode.ToBool())
                logger.LogInfomation(message);
        }

        public static void Error(string message) => logger.LogError(message);
        public static void Warn(string message) => logger.LogWarning(message);
    }
}