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

var awsProfile = builder.Configuration["AWS:Profile"];
var awsRegion = builder.Configuration["AWS:Region"] ?? "us-east-1";

var mongoDBSettings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>()
    ?? throw new Exception("MongoDB settings not found");
var cognitoSettings = builder.Configuration.GetSection("Cognito").Get<CognitoSettings>()
    ?? throw new Exception("Cognito settings not found");

AWSCredentials? awsCredentials = null;
var regionEndpoint = RegionEndpoint.GetBySystemName(awsRegion);

if (!string.IsNullOrEmpty(awsProfile) && isDevelopment)
{
    var credFile = new SharedCredentialsFile();
    if (credFile.TryGetProfile(awsProfile, out var profile))
    {
        awsCredentials = profile.GetAWSCredentials(credFile);
        var stsClient = new AmazonSecurityTokenServiceClient(awsCredentials, regionEndpoint);
        var identity = stsClient.GetCallerIdentityAsync(new GetCallerIdentityRequest()).GetAwaiter().GetResult();

        if (identity.Account != "916470730322")
        {
            throw new Exception($"Wrong AWS Account! Expected: 916470730322, Got: {identity.Account}");
        }
    }
    else
    {
        throw new Exception($"Profile '{awsProfile}' not found in credentials file at {credFile.FilePath}");
    }
}

if (awsCredentials != null)
{
    builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(sp =>
        new AmazonCognitoIdentityProviderClient(awsCredentials, regionEndpoint));
}
else
{
    builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(sp =>
        new AmazonCognitoIdentityProviderClient(regionEndpoint));
}

builder.Services.AddSingleton<ICognitoSettings>(cognitoSettings);
builder.Services.AddData(mongoDBSettings);
builder.Services.AddUseCases();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCustomAuthorization(cognitoSettings);
builder.Services.AddScoped<UserRequestContext>();

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
            OnTokenValidated = context =>
            {
                var subClaim = context.Principal?.FindFirst(ClaimsUser.Identifier);
                if (subClaim != null)
                {
                    var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
                    if (!claimsIdentity!.HasClaim(ClaimTypes.NameIdentifier, subClaim.Value))
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, subClaim.Value));
                    }
                }
                return Task.CompletedTask;
            }
        };
    });

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

app.MapGet("/api/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}))
.AllowAnonymous();

app.Run();