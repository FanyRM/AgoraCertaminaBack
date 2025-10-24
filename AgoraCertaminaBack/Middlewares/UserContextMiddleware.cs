using AgoraCertaminaBack.Models.General;
using System.Security.Claims;
using AgoraCertaminaBack.Authorization;

namespace AgoraCertaminaBack.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserRequestContext userContext)
        {
            if (context.User.Identity?.IsAuthenticated == true)
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
    }
}