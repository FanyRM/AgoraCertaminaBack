using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using AgoraCertaminaBack.Models.Response;
using System.Text.Json;
using AgoraCertaminaBack.Models.Response;

namespace AgoraCertaminaBack.Authorization.AttributeHandler
{
    public class HasPermissionsRequirementMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
                return;
            }

            if (authorizeResult.Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var errors = authorizeResult.AuthorizationFailure?.FailureReasons
                    ?.Select(x => x.Message)
                    .ToList() ?? new List<string> { "Denied access" };

                var response = new GenericResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.Forbidden,
                    ResponseType = ResponseTypeEnum.Info.ToString(),
                    Errors = errors
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Convierte los nombres de las propiedades a camelCase
                };

                context.Response.StatusCode = (int)response.HttpStatusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
            else
            {
                await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
            }
        }
    }
}