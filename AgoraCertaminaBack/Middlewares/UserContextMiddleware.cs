using AgoraCertaminaBack.Models.General;
using System.Security.Claims;
using AgoraCertaminaBack.Authorization;

namespace AgoraCertaminaBack.Middlewares
{
    public class UserContextMiddleware(RequestDelegate _next)
    {

        public async Task Invoke(HttpContext context, UserRequestContext userContext)
        {
            if (!ExcludeRoute(context.Request.Path.ToString()))
            {
                var userId = context.User.FindFirstValue(ClaimsUser.Identifier);
                var tenantId = context.User.FindFirstValue(ClaimsUser.OrganizationId);
                var email = context.User.FindFirstValue(ClaimsUser.Email);
                var name = context.User.FindFirstValue(ClaimsUser.Name);
                var roles = context.User.FindAll(ClaimsUser.Groups)
                    .Select(c => c.Value)
                    .ToList();

                userContext.UserId = userId ?? string.Empty;
                userContext.OrganizationId = tenantId ?? string.Empty;
                userContext.Email = email ?? string.Empty;
                userContext.Name = name ?? string.Empty;
                userContext.Roles = roles;
                userContext.IsAuthenticated = true;
            }
            else
            {
                userContext.IsAuthenticated = false;
            }

            await _next(context);
        }

        private static bool ExcludeRoute(string route)
        {
            var ignoreRoutes = new List<string>
            {
                "/auth/refresh",
                "/auth/exchange",
                "/auth/logout",
                "/api/health",
                "/api/get-all-contests",
                "/api/get-file"
            };

            // Verifica si la ruta contiene alguno de los patrones definidos
            return ignoreRoutes.Any(r => route.Contains(r, StringComparison.OrdinalIgnoreCase));
        }
    }
}