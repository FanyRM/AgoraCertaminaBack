namespace AgoraCertaminaBack.Data.Settings
{
    public class MongoDBSettings : IMongoDbSettings
    {
        public string ConnectionString { get; init; } = null!;
        public string DatabaseName { get; init; } = null!;
    }
}
