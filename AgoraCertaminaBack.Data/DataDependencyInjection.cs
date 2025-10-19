using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Data.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace AgoraCertaminaBack.Data
{
    public static class DataDependencyInjection
    {
        public static void AddData(this IServiceCollection services, MongoDBSettings mongoDBSettings)
        {
            services.AddSingleton<IMongoDbSettings>(mongoDBSettings);

            services.AddSingleton<MongoDBContext>();

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
        }

    }
}
