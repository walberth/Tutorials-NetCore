using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DemoCloudWatch.Middleware
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next) {
            _next = next;
        }

        public Task Invoke(HttpContext context) {
            var test1 = context.Request.Host.Value;
            var test2 = context.Request.Path.Value;
            var test3 = context.TraceIdentifier;

            var haveUsernameValue = context.Request.Headers.TryGetValue("username", out var username);

            if (haveUsernameValue) {
                Environment.SetEnvironmentVariable("username", $"{username[0]}"); // SET A DEFAULT VALUE 
            }

            Environment.SetEnvironmentVariable("httpMethod", $"{context.Request.Method}"); // POST (ACTION)
            Environment.SetEnvironmentVariable("action", $"{context.Request.Method}"); // NAME OF THE ACTION
            Environment.SetEnvironmentVariable("callSite", $"{context.Request.Host.Value}{context.Request.Path.Value}"); // URL

            return _next.Invoke(context);
        }
    }
}
