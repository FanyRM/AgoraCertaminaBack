namespace AgoraCertaminaBack.Services.Settings
{
    public interface IServiceSettings
    {
        //S3
        public string BucketName { get; init; }

        //Ruta de Front
        public string BaseUrlFront { get; init; }

        public bool UseLocalS3Simulator { get; init; }
        public string LocalSimulatorUrl { get; init; } 
    }
}
