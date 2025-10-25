using AgoraCertaminaBack.Services.Interfaces;
using AgoraCertaminaBack.Services.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace AgoraCertaminaBack.Services
{
    public static class ServicesDependencyInjection
    {
        public static void AddExternalServices(this IServiceCollection services, ServiceSettings serviceSettings)
        {
            services.AddSingleton<IServiceSettings>(serviceSettings);

            services.AddScoped<IFileManager, LocalFileManager>();

            services.AddScoped<IAWS, LocalAWSService>();
        }
    }
}
