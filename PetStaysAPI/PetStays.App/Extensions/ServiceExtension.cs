using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetStays.Persistence;
using PetStays.SQL;
using System.Reflection;

namespace PetStays.App.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services, string connectionString)
        {
            RegisterDBServices(services, connectionString);
            RegisterValidationService(services);
            RegisterAutoMapperService(services);
            return services;
        }

        public static void RegisterDBServices(IServiceCollection services, string connectionString)
        {
            services.AddScoped<IPetRepository, PetRepository>();
        }

        private static void RegisterAutoMapperService(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void RegisterValidationService(IServiceCollection services)
        {
            var assembliesToRegister = new List<Assembly>() { typeof(InitializeMediatR).Assembly };
            AssemblyScanner.FindValidatorsInAssemblies(assembliesToRegister).ForEach(pair =>
            {
                services.Add(ServiceDescriptor.Transient(pair.InterfaceType, pair.ValidatorType));
            });
        }
    }
}
