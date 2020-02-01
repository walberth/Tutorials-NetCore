namespace DemoCloudWatch.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next) {
            _next = next;
        }

        public Task Invoke(HttpContext context) {
            var haveUsernameValue = context.Request.Headers.TryGetValue("username", out var username);

            if (haveUsernameValue) {
                Environment.SetEnvironmentVariable("username", $"{username[0]}"); 
            }

            Environment.SetEnvironmentVariable("httpMethod", $"{context.Request.Method}");
            Environment.SetEnvironmentVariable("action", $"{context.Request.Method}"); 
            Environment.SetEnvironmentVariable("callSite", $"{context.Request.Host.Value}{context.Request.Path.Value}"); 

            return _next.Invoke(context);
        }
    }
}
