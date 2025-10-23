using AgoraCertaminaBack.Data.Settings;
using AgoraCertaminaBack.Data;
using AgoraCertaminaBack.UseCases;
using AgoraCertaminaBack.Services;
using AgoraCertaminaBack.Services.Settings;

var mongoDBSettings = new MongoDBSettings
{
    ConnectionString = "mongodb://localhost:27017",
    DatabaseName = "AgoraCertaminaDB"
};

var serviceSettings = new ServiceSettings
{
    BucketName = "agora-certamina-dev",
    BaseUrlFront = "http://localhost:4200",
    UseLocalS3Simulator = true,
    LocalSimulatorUrl = "http://localhost:3000/s3"
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExternalServices(serviceSettings);
builder.Services.AddData(mongoDBSettings);
builder.Services.AddUseCases();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

app.MapGet("/api/storage/health", async (HttpClient httpClient, IServiceSettings settings) =>
{
    if (!settings.UseLocalS3Simulator)
        return Results.Ok(new { status = "Using AWS S3" });

    try
    {
        var response = await httpClient.GetAsync($"{settings.LocalSimulatorUrl}/health");
        if (response.IsSuccessStatusCode)
        {
            return Results.Ok(new
            {
                status = "AmazonSimulator connected",
                simulatorUrl = settings.LocalSimulatorUrl
            });
        }
        else
        {
            return Results.Problem("AmazonSimulator not responding");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem($"AmazonSimulator connection failed: {ex.Message}");
    }
});
