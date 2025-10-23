using AgoraCertaminaBack.Authorization;
using AgoraCertaminaBack.Authorization.Settings;
using AgoraCertaminaBack.Data;
using AgoraCertaminaBack.Data.Settings;
using AgoraCertaminaBack.Middlewares;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases;
using AgoraCertaminaBack.Services;
using AgoraCertaminaBack.Services.Settings;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var serviceSettings = new ServiceSettings
{
    BucketName = "agora-certamina-dev",
    BaseUrlFront = "http://localhost:4200",
    UseLocalS3Simulator = true,
    LocalSimulatorUrl = "http://localhost:3000/s3"
};

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
string configurationCORS = "ConfigurationCors";

// ✅ CARGAR CONFIGURACIONES DESDE appsettings.json
var mongoDBSettings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>()
    ?? throw new Exception("MongoDB settings not found");

var cognitoSettings = builder.Configuration.GetSection("Cognito").Get<CognitoSettings>()
    ?? throw new Exception("Cognito settings not found");

// ✅ REGISTRAR AWS COGNITO CLIENT
var awsOptions = new AWSOptions
{
    Region = RegionEndpoint.USEast1 // ⚠️ CAMBIADO A USEast1 (tu UserPool está en us-east-1)
};
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonCognitoIdentityProvider>();

// ✅ REGISTRAR ICognitoSettings COMO SINGLETON
builder.Services.AddSingleton<ICognitoSettings>(cognitoSettings);

// ✅ REGISTRAR SERVICIOS
// Add services to the container.
builder.Services.AddExternalServices(serviceSettings);
builder.Services.AddData(mongoDBSettings);
builder.Services.AddUseCases();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// ✅ AUTORIZACIÓN PERSONALIZADA
builder.Services.AddCustomAuthorization(cognitoSettings);

// ✅ CONTEXTO DE USUARIO
builder.Services.AddScoped<UserRequestContext>();

// ✅ SWAGGER CON JWT
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Scheme = "Bearer"
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// ✅ AUTENTICACIÓN JWT CON COGNITO
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = cognitoSettings.Authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false, // ⚠️ CAMBIADO: Access tokens de Cognito NO tienen audience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(5), // ⚠️ Aumentado para evitar problemas de sincronización
            RoleClaimType = "cognito:groups"
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();
                logger.LogError(context.Exception, "❌ Authentication failed");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();
                var userId = context.Principal?.FindFirst(ClaimsUser.Identifier)?.Value;
                var tenantId = context.Principal?.FindFirst(ClaimsUser.TenantId)?.Value;
                logger.LogInformation("✅ Token validated for user: {UserId}, Tenant: {TenantId}", userId, tenantId);

                // Debug: Mostrar todos los claims
                if (context.Principal != null)
                {
                    foreach (var claim in context.Principal.Claims)
                    {
                        logger.LogDebug("Claim: {Type} = {Value}", claim.Type, claim.Value);
                    }
                }

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();
                logger.LogWarning("⚠️ Authentication challenge: {Error}", context.Error);
                return Task.CompletedTask;
            }
        };
    });

// ✅ CORS
builder.Services.AddCors(options =>
{
    if (isDevelopment)
    {
        options.AddPolicy(name: configurationCORS, builder =>
        {
            builder.WithOrigins("http://localhost:5173", "http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
    }
    else
    {
        options.AddPolicy(name: configurationCORS, builder =>
        {
            builder.WithOrigins("https://d84l1y8p4kdic.cloudfront.net")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("*");
        });
    }
});

builder.Services.AddHttpClient();

var app = builder.Build();

// ===== ✅ PIPELINE HTTP (ORDEN CRÍTICO) =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(configurationCORS);

// 🔥 ORDEN CORRECTO:
app.UseAuthentication();              // 1️⃣ Valida JWT y crea ClaimsPrincipal
app.UseMiddleware<UserContextMiddleware>(); // 2️⃣ Popula UserRequestContext desde claims
app.UseAuthorization();               // 3️⃣ Verifica permisos basados en el contexto

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

// ✅ Health check
app.MapGet("/api/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}))
.AllowAnonymous();

app.Run();
