namespace AgoraCertaminaBack.Data.Settings
{
    public interface IMongoDbSettings
    {
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
    }
}
