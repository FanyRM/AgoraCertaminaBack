using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Cryptography; // Necesario para crear un hash para la clave simulada

// Usings para la configuración de tu proyecto AgoraCertaminaBack
using AgoraCertaminaBack.Data.Settings;
using AgoraCertaminaBack.Data;
using AgoraCertaminaBack.UseCases;
using AgoraCertaminaBack.Services;
using AgoraCertaminaBack.Models.General; // Para UserRequestContext



var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
string configurationCORS = "ConfigurationCors";

// --- SIMULACIÓN DE CLASES DE CONFIGURACIÓN ---
// ATENCIÓN: En una aplicación real, estas clases deben cargarse de forma segura 
// desde Azure Key Vault, AWS Secrets Manager, o archivos de configuración.


// 2. Configuración de MongoDB
var mongoDBSettings = new MongoDBSettings
{
    ConnectionString = "mongodb://localhost:27017",
    DatabaseName = "AgoraCertaminaDB"
};

// --- REGISTRO DE SERVICIOS EN EL CONTENEDOR DI ---

//builder.Services.AddExternalServices(serviceSetttings); // Asumiendo que esta extensión existe
builder.Services.AddData(mongoDBSettings);
builder.Services.AddUseCases();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor(); // Necesario para acceder a HttpContext en middlewares/servicios

// --- AUTORIZACIÓN Y CONTEXTO DE USUARIO ---
//builder.Services.AddCustomAuthorization(cognitoSettings, verifiedSettings); // Asumiendo que esta extensión existe
builder.Services.AddScoped<UserRequestContext>(); // Solución al error de dependencia

// Registro de objetos de configuración
//builder.Services.AddSingleton(serviceSetttings);
// builder.Services.AddSingleton<IQRService, QRService>(); // Añadir si QRService es necesario

// --- AUTENTICACIÓN JWT ---
//builder.Services
//    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//    {
//        options.Authority = cognitoSettings.Authority;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidAudience = cognitoSettings.ClientId,
//            ValidIssuer = cognitoSettings.Authority,
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ClockSkew = TimeSpan.FromSeconds(10)
//        };
//    });

// --- SWAGGER/OPENAPI CON SEGURIDAD ---
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer"
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// --- CONFIGURACIÓN CORS ---
builder.Services.AddCors(options =>
{
    if (isDevelopment)
    {
        options.AddPolicy(name: configurationCORS, builder =>
        {
            builder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
    }
    else
    {
        options.AddPolicy(name: configurationCORS, builder =>
        {
            builder.WithOrigins(
                "https://sensus.nadglobal.com" // Asegúrate de cambiar esto al dominio real de Agora
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("*");
        });
    }
});

var app = builder.Build();

// --- CONFIGURACIÓN DEL PIPELINE HTTP (MIDDLEWARES) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS debe ir primero
app.UseCors(configurationCORS);

// Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

// Middleware personalizado para establecer el contexto de usuario (UserRequestContext)
//app.UseMiddleware<UserContextMiddleware>();

app.MapControllers();

// Endpoint de prueba de salud (health check)
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .AllowAnonymous();

app.Run();
