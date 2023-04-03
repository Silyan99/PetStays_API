using Microsoft.Extensions.DependencyInjection;

namespace PetStays.App.Helpers
{
    public static class ServiceDefaultSettings
    {
        public static T GetService<T>(IServiceCollection services)
        {
            IServiceProvider provider = services.BuildServiceProvider();
            return provider.GetRequiredService<T>();
        }
    }
}
