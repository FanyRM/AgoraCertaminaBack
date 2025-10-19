using AgoraCertaminaBack.Data.Settings;
using AgoraCertaminaBack.Data;
using AgoraCertaminaBack.UseCases;
using AgoraCertaminaBack.Services;

var mongoDBSettings = new MongoDBSettings
{
    ConnectionString = "mongodb://localhost:27017",
    DatabaseName = "AgoraCertaminaDB"
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddExternalServices();
builder.Services.AddData(mongoDBSettings);
builder.Services.AddUseCases();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
