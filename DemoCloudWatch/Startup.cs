namespace DemoCloudWatch {
    using NLog;
    using Business;
    using NLog.Common;
    using NLog.Config;
    using NLog.Targets;
    using NLog.AWS.Logger;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPersonApplication, PersonApplication>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<Middleware>();

            app.UseMvc();
            ConfigureLogger();
        }

        private void ConfigureLogger() {
            var config = new LoggingConfiguration();

            var awsTarget = CreateAwsTarget(Configuration["InformationAWS:LogGroup"],
                Configuration["InformationAWS:Region"],
                Configuration["InformationAWS:AccessKey"],
                Configuration["InformationAWS:SecretKey"],
                Configuration["InformationAWS:Layout"]);

            var fileTarget = CreateFileTarget(Configuration["Logging:File:Path"], Configuration["Logging:File:Layout"]);

            config.AddTarget("AWSTarget", awsTarget);
            config.AddTarget(fileTarget);

            config.AddRuleForOneLevel(LogLevel.Error, awsTarget);
            config.AddRuleForOneLevel(LogLevel.Fatal, awsTarget);
            config.AddRuleForOneLevel(LogLevel.Warn, awsTarget);
            config.AddRuleForOneLevel(LogLevel.Info, awsTarget);
            config.AddRuleForOneLevel(LogLevel.Trace, fileTarget);

            InternalLogger.LogFile = Configuration["Logging:InternalLog"];
            LogManager.Configuration = config;
        }

        private AWSTarget CreateAwsTarget(string logGroup, string region, string accessKey, string secretKey, string layout) {
            var target = new AWSTarget();
            target.Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            target.LogGroup = logGroup;
            target.Region = region;
            //target.Layout = new JsonLayout();

            //var information = new LogEventInfo();
            ////information.Properties["IdLog"] = "${event-properties:item=idlog}";
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

            var format = "{ " +
                             "Id=" + "${event-properties:item=idlog}" + '\'' +
                             ", LogTimeStamp='" + "${longdate}" + '\'' +
                             ", MachineName='" + "${aspnet-request-ip}" + '\'' +
                             ", Level='" + "${level}" + '\'' +
                             ", Message=" + "${message}" +
                             ", Exception=" + "${stacktrace}" +
                             ", Payload='" + "${when:when='${aspnet-request-method}' == 'GET':inner='${aspnet-request-querystring}':else='${aspnet-request-posted-body}'}" + '\'' +
                             ", CallSite='" + "${aspnet-request-url:IncludePort=true:IncludeQueryString=true}" + '\'' +
                             ", Action='" + "${aspnet-request-method}" + '\'' +
                             ", Username='" + "${aspnet-sessionid}" + '\'' +
                             ", MethodName='" + "${event-properties:item=methodName}" + '\'' +
                             ", ApplicationName=" + "Test" +
                         " }";

            //target.Layout.Precalculate(information);
            target.LogStreamNameSuffix = "hola";
            target.LogStreamNamePrefix = "again";
            target.Layout = format;

            //return new AWSTarget {
            //    LogGroup = logGroup,
            //    Region = region,
            //    Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey),
            //    Layout = layout
            //};

            return target;
        }

        private static FileTarget CreateFileTarget(string path, string layout) {
            return new FileTarget("FileTarget") {
                FileName = path,
                Layout = layout
            };
        }
    }
}
