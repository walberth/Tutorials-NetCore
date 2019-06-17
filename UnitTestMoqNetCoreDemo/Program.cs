using System;

namespace UnitTestMoqNetCoreDemo
{
    using ApplicationLayer;
    using ApplicationLayerInterface;
    using Microsoft.Extensions.DependencyInjection;
    using RepositoryLayer;
    using RepositoryLayerInterface;

    class Program {
        static void Main(string[] args) {
            var serviceProvider = new ServiceCollection()
                                              .AddSingleton<IPersonApplication, PersonApplication>()
                                              .AddSingleton<IPersonRepository, PersonRepository>()
                                              .BuildServiceProvider();

            var personApplication = serviceProvider.GetService<IPersonApplication>();

            var test = personApplication.GetCompleteName("walberth", "gutierrez");

            Console.WriteLine(test);
        }
    }
}
