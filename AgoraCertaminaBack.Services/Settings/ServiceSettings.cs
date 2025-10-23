namespace AgoraCertaminaBack.Services.Settings
{
    public class ServiceSettings : IServiceSettings
    {
        //Interfaz
        public string BucketName { get; init; } = null!;
        public string BaseUrlFront { get; init; } = null!;

        //S3 Local Simulator
        public bool UseLocalS3Simulator { get; init; } = false;
        public string LocalSimulatorUrl { get; init; } = "http://localhost:3000/s3";
    }
}
