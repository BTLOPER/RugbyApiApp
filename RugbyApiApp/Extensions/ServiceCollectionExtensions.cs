using Microsoft.Extensions.DependencyInjection;
using RugbyApiApp.Data;
using RugbyApiApp.Services;

namespace RugbyApiApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Rugby API and Data services
        /// </summary>
        public static IServiceCollection AddRugbyApiServices(this IServiceCollection services, string apiKey)
        {
            services.AddScoped<RugbyApiClient>(provider => new RugbyApiClient(apiKey));
            services.AddScoped<DataService>();
            services.AddScoped<RugbyDbContext>();
            
            return services;
        }

        /// <summary>
        /// Register only the Rugby API client
        /// </summary>
        public static IServiceCollection AddRugbyApiClient(this IServiceCollection services, string apiKey)
        {
            services.AddScoped<RugbyApiClient>(provider => new RugbyApiClient(apiKey));
            return services;
        }

        /// <summary>
        /// Register only the Data service
        /// </summary>
        public static IServiceCollection AddRugbyDataService(this IServiceCollection services)
        {
            services.AddScoped<DataService>();
            services.AddScoped<RugbyDbContext>();
            return services;
        }
    }
}
