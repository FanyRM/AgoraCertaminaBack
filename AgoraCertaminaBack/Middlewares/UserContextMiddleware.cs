using AgoraCertaminaBack.Models.General;
using System.Security.Claims;

namespace AgoraCertaminaBack.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserContextMiddleware> _logger;

        public UserContextMiddleware(RequestDelegate next, ILogger<UserContextMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, UserRequestContext userContext)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                try
                {
                    // Extraer claims de Cognito
                    var userId = context.User.FindFirstValue(ClaimsUser.Identifier);
                    var tenantId = context.User.FindFirstValue(ClaimsUser.TenantId);
                    var email = context.User.FindFirstValue(ClaimsUser.Email);
                    var name = context.User.FindFirstValue(ClaimsUser.Name);

                    // Extraer roles desde cognito:groups
                    var roles = context.User.FindAll(ClaimsUser.Groups)
                        .Select(c => c.Value)
                        .ToList();

                    // Poblar el contexto
                    userContext.UserId = userId ?? string.Empty;
                    userContext.OrganizationId = tenantId ?? string.Empty; // Mapear tenant_id a OrganizationId
                    userContext.Email = email ?? string.Empty;
                    userContext.Name = name ?? string.Empty;
                    userContext.Roles = roles;
                    userContext.IsAuthenticated = true;

                    _logger.LogDebug(
                        "User context initialized - UserId: {UserId}, OrganizationId: {OrganizationId}, Roles: {Roles}",
                        userId,
                        tenantId,
                        string.Join(", ", roles));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error extracting user context from claims");
                    userContext.IsAuthenticated = false;
                }
            }
            else
            {
                userContext.IsAuthenticated = false;
                _logger.LogDebug("Request from unauthenticated user");
            }

            await _next(context);
        }
    }
}