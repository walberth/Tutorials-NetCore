namespace DemoCloudWatch {
    using NLog;
    using Business;
    using NLog.Common;
    using NLog.Config;
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
            app.UseMiddleware<Middleware.Middleware>();

            app.UseMvc();
            ConfigureLogger();
        }

        private void ConfigureLogger() {
            var config = new LoggingConfiguration();

            var awsTarget = CreateAwsTarget(Configuration["InformationAWS:LogGroup"],
                                            Configuration["InformationAWS:Region"],
                                            Configuration["InformationAWS:AccessKey"],
                                            Configuration["InformationAWS:SecretKey"]);


            config.AddTarget("AWSTarget", awsTarget);

            config.AddRuleForOneLevel(LogLevel.Error, awsTarget);
            config.AddRuleForOneLevel(LogLevel.Fatal, awsTarget);
            config.AddRuleForOneLevel(LogLevel.Warn, awsTarget);
            config.AddRuleForOneLevel(LogLevel.Info, awsTarget);

            InternalLogger.LogFile = Configuration["Logging:InternalLog"];
            LogManager.Configuration = config;
        }

        private AWSTarget CreateAwsTarget(string logGroup, string region, string accessKey, string secretKey) {
            var target = new AWSTarget();
            target.Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            target.LogGroup = logGroup;
            target.Region = region;

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

            target.LogStreamNameSuffix = "Demo";
            target.LogStreamNamePrefix = "Logger";
            target.Layout = format;

            return target;
        }
    }
}
