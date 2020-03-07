namespace EFCore_Demo.Configuration
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.OpenApi.Models;
    using Microsoft.AspNetCore.Builder;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using Microsoft.Extensions.DependencyInjection;

    public static class SwaggerServiceExtensions {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services) {
            var info = new OpenApiInfo();
            info.Version = "v1";
            info.Title = "EFCore-Demo";
            info.Description = "Modelo de web api en NetCore 3.0 con EF";
            info.Contact = new OpenApiContact();
            info.Contact.Email = "w.felipe.gutierrez@gmail.com";
            info.Contact.Name = "Walberth Gutierrez Telles";
            info.Contact.Url = new Uri("https://codingwithnotrycatch.com/");


            services.AddSwaggerGen(c => {
                                       c.SwaggerDoc("v1", info);
                                       var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                                       var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                                       c.IncludeXmlComments(xmlPath);
                                   });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app) {
            app.UseSwagger();

            app.UseSwaggerUI(c => {
                                 c.SwaggerEndpoint("./swagger/v1/swagger.json", "Web API EF-Core v1");
                                 c.RoutePrefix = string.Empty;
                                 c.DocumentTitle = "EFCore-Demo Documentation";
                                 c.DocExpansion(DocExpansion.None);
                             });

            return app;
        }
    }
}
