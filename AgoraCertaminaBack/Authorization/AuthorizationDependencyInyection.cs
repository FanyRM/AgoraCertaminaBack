using AgoraCertaminaBack.Authorization.AttributeHandler;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Authorization;
using AgoraCertaminaBack.Authorization.Settings;

namespace AgoraCertaminaBack.Authorization
{
    public static class AuthorizationDependencyInyection
    {
        public static void AddCustomAuthorization(this IServiceCollection services, CognitoSettings cognitoSettings)
        {
            services.AddAuthorization();
            services.AddSingleton<ICognitoSettings>(cognitoSettings);

            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, HasPermissionRequirementHandler>();
            services.AddTransient<IAuthorizationMiddlewareResultHandler, HasPermissionsRequirementMiddleware>();

            // ❌ ELIMINAR ESTAS LÍNEAS - El cliente ya se registra en Program.cs
            // services.AddSingleton<IAmazonCognitoIdentityProvider>(options =>
            // {
            //     return new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast1);
            // });
        }
    }
}