using AgoraCertaminaBack.Authorization;
using AgoraCertaminaBack.Authorization.Settings;
using AgoraCertaminaBack.Data;
using AgoraCertaminaBack.Data.Settings;
using AgoraCertaminaBack.Middlewares;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
string configurationCORS = "ConfigurationCors";

// 🔍 DEBUG: Verificar configuración
var awsProfile = builder.Configuration["AWS:Profile"];
var awsRegion = builder.Configuration["AWS:Region"] ?? "us-east-1";

Console.WriteLine("========================================");
Console.WriteLine($"🔍 Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"🔍 AWS Profile: '{awsProfile}'");
Console.WriteLine($"🔍 AWS Region: {awsRegion}");
Console.WriteLine($"🔍 Cognito UserPoolId: {builder.Configuration["Cognito:UserPoolId"]}");
Console.WriteLine("========================================");

// ✅ CARGAR CONFIGURACIONES DESDE appsettings.json
var mongoDBSettings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>()
    ?? throw new Exception("MongoDB settings not found");

var cognitoSettings = builder.Configuration.GetSection("Cognito").Get<CognitoSettings>()
    ?? throw new Exception("Cognito settings not found");

Console.WriteLine($"✅ CognitoSettings loaded - UserPoolId: {cognitoSettings.UserPoolId}");

// ✅ CONFIGURAR AWS CREDENTIALS
AWSCredentials? awsCredentials = null;
var regionEndpoint = RegionEndpoint.GetBySystemName(awsRegion);

// 🔥 SOLUCIÓN: Cargar y verificar credenciales explícitamente
if (!string.IsNullOrEmpty(awsProfile) && isDevelopment)
{
    try
    {
        Console.WriteLine($"🔍 Loading credentials from profile: {awsProfile}");

        var credFile = new SharedCredentialsFile();

        if (credFile.TryGetProfile(awsProfile, out var profile))
        {
            awsCredentials = profile.GetAWSCredentials(credFile);

            // Verificar las credenciales inmediatamente
            var stsClient = new AmazonSecurityTokenServiceClient(awsCredentials, regionEndpoint);
            var identity = stsClient.GetCallerIdentityAsync(new GetCallerIdentityRequest()).GetAwaiter().GetResult();

            Console.WriteLine($"✅ Successfully loaded credentials from profile: {awsProfile}");
            Console.WriteLine($"✅ AWS Account: {identity.Account}");
            Console.WriteLine($"✅ IAM User ARN: {identity.Arn}");
            Console.WriteLine($"✅ User ID: {identity.UserId.Substring(0, Math.Min(10, identity.UserId.Length))}...");

            if (identity.Account != "916470730322")
            {
                throw new Exception($"❌ Wrong AWS Account! Expected: 916470730322, Got: {identity.Account}");
            }
        }
        else
        {
            throw new Exception($"❌ Profile '{awsProfile}' not found in credentials file at {credFile.FilePath}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ CRITICAL ERROR loading AWS credentials: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw;
    }
}
else
{
    Console.WriteLine("✅ Using default AWS credentials or IAM Role");
}

// 🔥 REGISTRAR COGNITO CLIENT MANUALMENTE CON LAS CREDENCIALES CORRECTAS
if (awsCredentials != null)
{
    builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(sp =>
    {
        var client = new AmazonCognitoIdentityProviderClient(awsCredentials, regionEndpoint);
        Console.WriteLine("✅ Created CognitoIdentityProvider client with explicit credentials");
        return client;
    });
}
else
{
    builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(sp =>
    {
        var client = new AmazonCognitoIdentityProviderClient(regionEndpoint);
        Console.WriteLine("✅ Created CognitoIdentityProvider client with default credentials");
        return client;
    });
}

// ✅ REGISTRAR ICognitoSettings COMO SINGLETON
builder.Services.AddSingleton<ICognitoSettings>(cognitoSettings);

// ✅ REGISTRAR SERVICIOS
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
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            RoleClaimType = ClaimsUser.Groups,
            NameClaimType = ClaimsUser.Identifier
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

                var subClaim = context.Principal?.FindFirst(ClaimsUser.Identifier);
                if (subClaim != null)
                {
                    var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
                    if (!claimsIdentity!.HasClaim(ClaimTypes.NameIdentifier, subClaim.Value))
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, subClaim.Value));
                    }
                }

                var userId = context.Principal?.FindFirst(ClaimsUser.Identifier)?.Value;
                var tenantId = context.Principal?.FindFirst(ClaimsUser.OrganizationId)?.Value;
                logger.LogInformation("✅ Token validated for user: {UserId}, Tenant: {TenantId}", userId, tenantId);

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

var app = builder.Build();

// ===== ✅ PIPELINE HTTP =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(configurationCORS);
app.UseAuthentication();
app.UseMiddleware<UserContextMiddleware>();
app.UseAuthorization();
app.MapControllers();

// ✅ Health check
app.MapGet("/api/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}))
.AllowAnonymous();

// 🔍 DEBUG ENDPOINT
app.MapGet("/api/debug/aws-config", async (
    IAmazonCognitoIdentityProvider cognitoClient,
    ICognitoSettings cognitoSettings) =>
{
    try
    {
        Console.WriteLine($"🔍 DEBUG: Attempting to describe user pool: {cognitoSettings.UserPoolId}");

        var callerIdentity = await cognitoClient.DescribeUserPoolAsync(
            new Amazon.CognitoIdentityProvider.Model.DescribeUserPoolRequest
            {
                UserPoolId = cognitoSettings.UserPoolId
            });

        Console.WriteLine($"✅ DEBUG: Successfully described user pool: {callerIdentity.UserPool.Name}");

        return Results.Ok(new
        {
            success = true,
            userPoolId = cognitoSettings.UserPoolId,
            userPoolName = callerIdentity.UserPool.Name,
            region = cognitoClient.Config.RegionEndpoint.SystemName,
            accountId = callerIdentity.UserPool.Arn?.Split(':')[4]
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ DEBUG: Error describing user pool: {ex.Message}");

        return Results.Ok(new
        {
            success = false,
            error = ex.Message,
            errorType = ex.GetType().Name,
            userPoolId = cognitoSettings.UserPoolId,
            innerException = ex.InnerException?.Message
        });
    }
})
.AllowAnonymous();

app.Run();