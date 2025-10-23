using AgoraCertaminaBack.Models.General;
using System.Security.Claims;
using AgoraCertaminaBack.Authorization; // Asegúrate de que este using esté presente

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
                    // ✅ DEBUG: Ver TODOS los claims (se mantiene para análisis)
                    _logger.LogWarning("========== TODOS LOS CLAIMS DEL TOKEN ==========");
                    foreach (var claim in context.User.Claims)
                    {
                        _logger.LogWarning($"Claim Type: {claim.Type} | Value: {claim.Value}");
                    }
                    _logger.LogWarning("===============================================");

                    // Extraer claims de Cognito usando las constantes actualizadas
                    var userId = context.User.FindFirstValue(ClaimsUser.Identifier);
                    // Usamos ClaimsUser.OrganizationId (que ahora es "organization_id")
                    var tenantId = context.User.FindFirstValue(ClaimsUser.OrganizationId);
                    var email = context.User.FindFirstValue(ClaimsUser.Email);
                    var name = context.User.FindFirstValue(ClaimsUser.Name);

                    // ✅ DEBUG: Ver valores específicos
                    // Se actualiza el log para usar el nombre correcto del claim para la búsqueda
                    _logger.LogWarning($"🔍 userId ({ClaimsUser.Identifier}): {userId}");
                    _logger.LogWarning($"🔍 tenantId ({ClaimsUser.OrganizationId}): {tenantId}");
                    _logger.LogWarning($"🔍 email: {email}");
                    _logger.LogWarning($"🔍 name: {name}");

                    // Extraer roles desde cognito:groups
                    var roles = context.User.FindAll(ClaimsUser.Groups)
                        .Select(c => c.Value)
                        .ToList();

                    _logger.LogWarning($"🔍 roles: {string.Join(", ", roles)}");

                    // Poblar el contexto
                    userContext.UserId = userId ?? string.Empty;
                    userContext.OrganizationId = tenantId ?? string.Empty;
                    userContext.Email = email ?? string.Empty;
                    userContext.Name = name ?? string.Empty;
                    userContext.Roles = roles;
                    userContext.IsAuthenticated = true;

                    _logger.LogWarning(
                        "✅ User context initialized - UserId: {UserId}, OrganizationId: {OrganizationId}, Roles: {Roles}",
                        userId,
                        tenantId,
                        string.Join(", ", roles));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error extracting user context from claims");
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
