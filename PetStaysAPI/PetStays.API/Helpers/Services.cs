using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PetStays.App;
using PetStays.App.Extensions;
using PetStays.App.Helpers;
using PetStays.Domain.Models;

namespace PetStays.API.Helpers
{
    public class Services
    {
        /// <summary>
        /// Registers the custom service.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void RegisterCustomService(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddInternalServices(ServiceDefaultSettings.GetService<IOptions<ConnectionSettings>>(services).Value.DefaultConnection);
        }

        /// <summary>
        /// Registers the swagger documentation.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void RegisterSwaggerDocumentation(IServiceCollection services)
        {
            //Swagger Settings
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Pet Stays API", Version = "v1" });
                var security = new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", new string[] { }},
                    };

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            },
                        new List<string>()
                    }
                });
            });
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void LoadConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            var x = configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionSettings>(x);
            services.AddSingleton<IConfiguration>(configuration);
        }

        /// <summary>
        /// Registers the mediatr service.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void RegisterMediatrService(IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(InitializeMediatR).Assembly));
        }
    }
}
