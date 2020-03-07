namespace EFCore_Demo.Configuration
{
    using System;
    using System.Net;
    using Transversal;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class ExceptionMiddleware {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext) {
            try {
                await _next(httpContext);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception) {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new Response<string>();
            response.IsSuccess = false;
            response.IsSuccess = true;
            response.Message = $"Ocurrió un error, contactar con el administrador: {Guid.NewGuid()}";

            return context.Response.WriteAsync(response.Serialize());
        }
    }
}
