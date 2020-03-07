namespace EFCore_Demo.Configuration 
{
    using Mapper;
    using AutoMapper;
    using Service.Interface;
    using Repository.Interface;
    using Service.Implementation;
    using Repository.Implementation;
    using Microsoft.Extensions.DependencyInjection;

    public static class ContainerProvider {
        public static IServiceCollection ConfigureServiceCollection(this IServiceCollection services) {
            ConfigureContainer(services);
            ConfigureMapper(services);

            return services;
        }

        private static void ConfigureContainer(IServiceCollection services) {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentService, StudentService>();
        }

        private static void ConfigureMapper(IServiceCollection services) {
            var automapperConfig = new MapperConfiguration(configuration => {
                                                               configuration.AddProfile(new StudentProfile());
                                                           });

            services.AddSingleton(automapperConfig.CreateMapper());
        }
    }
}
