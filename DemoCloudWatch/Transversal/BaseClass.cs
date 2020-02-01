namespace DemoCloudWatch.Transversal
{
    using NLog;
    using System;

    public class BaseClass {
        private static Logger _logger;

        public BaseClass() {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void RegisterLogFatal(Exception ex, Guid identifier) {
            var log = new LogEventInfo(LogLevel.Fatal, "", "") {
                Message = ex.Message,
                Exception = ex
            };

            log.Properties.Add("idlog", identifier.ToString());
            log.Properties.Add("methodName", $"{ex.TargetSite.DeclaringType?.FullName}.{ex.TargetSite.Name}");

            _logger.Fatal(log);
        }
    }
}
