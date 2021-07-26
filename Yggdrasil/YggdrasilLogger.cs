using BepInEx.Logging;

namespace Yggdrasil {
    public static class YggdrasilLogger {
        private static readonly ManualLogSource _logger;

        static YggdrasilLogger() {
            _logger = new ManualLogSource("Yggdrasil");
            Logger.Sources.Add(_logger);
        }

        private static string Format(object msg) => msg.ToString();

        public static void Info(object data) => _logger.LogMessage(Format(data));
        public static void Debug(object data) => _logger.LogDebug(Format(data));
        public static void Error(object data) => _logger.LogError(Format(data));
    }
}
