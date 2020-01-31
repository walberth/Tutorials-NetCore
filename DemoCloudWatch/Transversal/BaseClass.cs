namespace DemoCloudWatch.Transversal
{
    using NLog;
    using Model;
    using System;

    public class BaseClass {
        private static Logger _logger;
        private static LogEventInfo _logEventInfo;

        public BaseClass() {
            _logger = LogManager.GetCurrentClassLogger();
            _logEventInfo = new LogEventInfo(LogLevel.Error, "", "customs values");
        }

        public void RegisterLogFatal(Exception ex) {
            var log = new LogEventInfo(LogLevel.Fatal, "", "") {
                Message = ex.Message,
                Exception = ex
            };

            var test = ex.Source;
            var test1 = ex.TargetSite;
            var test2 = ex.Data;
            var test3 = test1.Name;
            var test5 = test1.Name;
            var test6 = test1.DeclaringType.FullName;

            log.Properties.Add("idlog", Guid.NewGuid().ToString());
            log.Properties.Add("methodName", $"{ex.TargetSite.DeclaringType?.FullName}.{ex.TargetSite.Name}");

            //var information = new LogEventInfo(LogLevel.Fatal, "", "") { Message = ex.Message, Exception = ex };
            //information.Properties["IdLog"] = "${event-properties:item=idlog}";
            //information.Properties["LogTimeStamp"] = "${longdate}";
            //information.Properties["MachineName"] = "${aspnet-request-ip}";
            //information.Properties["Level"] = "${level}";
            //information.Properties["Message"] = "${message}";
            //information.Properties["Exception"] = "${stacktrace}";
            //information.Properties["Payload"] = "${when:when='${aspnet-request-method}' == 'GET':inner='${aspnet-request-querystring}':else='${aspnet-request-posted-body}'}";
            //information.Properties["CallSite"] = "${aspnet-request-url:IncludePort=true:IncludeQueryString=true}";
            //information.Properties["Action"] = "${aspnet-request-method}";
            //information.Properties["Username"] = "${aspnet-sessionid}";
            //information.Properties["MethodName"] = "${aspnet-mvc-action}";
            //information.Properties["ApplicationName"] = "${event-properties:item=idlog}";

            //_logger.Fatal(information);

            _logger.Fatal(log);
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
