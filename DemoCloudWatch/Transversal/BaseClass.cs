namespace DemoCloudWatch.Transversal
{
    using NLog;
    using Model;

    public class BaseClass {
        private static Logger _logger;
        private static LogEventInfo _logEventInfo;

        public BaseClass() {
            _logger = LogManager.GetCurrentClassLogger();
            _logEventInfo = new LogEventInfo(LogLevel.Error, "", "customs values");
        }

        public void RegisterLogFatal(Log log) {
            _logger.Fatal(log.ToString());
        }

        public void RegisterLogError(Log log) {
            _logger.Error(log.ToString());
        }

        public void RegisterLogWarning(Log log) {
            _logger.Warn(log.ToString());
        }

        public void RegisterLogInfo(Log log) {
            _logger.Info(log.ToString());
        }
    }
}
