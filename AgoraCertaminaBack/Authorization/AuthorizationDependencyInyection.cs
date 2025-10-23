using AgoraCertaminaBack.Authorization.AttributeHandler;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Authorization;
using AgoraCertaminaBack.Authorization.Settings;

namespace AgoraCertaminaBack.Authorization
{
    public static class AuthorizationDependencyInyection
    {
        // ✅ CAMBIAR la firma del método (solo recibe cognitoSettings)
        public static void AddCustomAuthorization(this IServiceCollection services, CognitoSettings cognitoSettings)
        {
            services.AddAuthorization();
            services.AddSingleton<ICognitoSettings>(cognitoSettings);

            // ❌ ELIMINAR todo lo relacionado con Verified Permissions:
            // services.AddSingleton<IVerifiedPermissionsSettings>(verifiedSettings);
            // services.AddTransient<IVerifiedPermissionsUtil, VerifiedPermissionsUtil>();
            // services.AddTransient<IAmazonVerifiedPermissions, AmazonVerifiedPermissionsClient>();

            // ✅ MANTENER estos (necesarios para la autorización):
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, HasPermissionRequirementHandler>();
            services.AddTransient<IAuthorizationMiddlewareResultHandler, HasPermissionsRequirementMiddleware>();

            // Cognito client
            services.AddSingleton<IAmazonCognitoIdentityProvider>(options =>
            {
                return new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast1);
            });
        }
    }
}