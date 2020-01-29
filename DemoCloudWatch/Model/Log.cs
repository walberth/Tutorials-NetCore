namespace DemoCloudWatch.Model
{
    using System;

    public class Log {
        public Guid IdLog { get; set; }
        public DateTime LogTimeStamp { get; set; }
        public string MachineName { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Payload { get; set; }
        public string CallSite { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }
        public string MethodName { get; set; }
        public string ApplicationName { get; set; }

        public override string ToString() {
            return "{ " +
                   "Id=" + IdLog +
                   ", LogTimeStamp='" + LogTimeStamp + '\'' +
                   ", MachineName='" + MachineName + '\'' +
                   ", Level='" + Level + '\'' +
                   ", Message=" + Message +
                   ", Exception=" + Exception +
                   ", Payload='" + Payload + '\'' +
                   ", CallSite='" + CallSite + '\'' +
                   ", Action='" + Action + '\'' +
                   ", Username='" + Username + '\'' +
                   ", MethodName='" + MethodName + '\'' +
                   ", ApplicationName=" + ApplicationName +
                   " }";
        }
    }
}
