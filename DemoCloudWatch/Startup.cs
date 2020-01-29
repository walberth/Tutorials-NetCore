using Microsoft.AspNetCore.Mvc;
using NLog.AWS.Logger;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

namespace DemoCloudWatch {
    using Microsoft.AspNetCore.Http;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

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

            config.AddRuleForOneLevel(NLog.LogLevel.Error, awsTarget);
            config.AddRuleForOneLevel(NLog.LogLevel.Fatal, awsTarget);
            config.AddRuleForOneLevel(NLog.LogLevel.Warn, awsTarget);
            config.AddRuleForOneLevel(NLog.LogLevel.Info, awsTarget);
            config.AddRuleForOneLevel(NLog.LogLevel.Trace, fileTarget);

            InternalLogger.LogFile = Configuration["Logging:InternalLog"];
            NLog.LogManager.Configuration = config;
        }

        private AWSTarget CreateAwsTarget(string logGroup, string region, string accessKey, string secretKey, string layout) {
            return new AWSTarget {
                LogGroup = logGroup,
                Region = region,
                Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey),
                Layout = layout
            };
        }

        private static FileTarget CreateFileTarget(string path, string layout) {
            return new FileTarget("FileTarget") {
                FileName = path,
                Layout = layout
            };
        }
    }
}
