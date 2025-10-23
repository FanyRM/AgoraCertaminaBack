using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Entities.Interfaces;
using AgoraCertaminaBack.Models.General;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AgoraCertaminaBack.Authorization.AttributeHandler
{
    public class HasPermissionRequirementHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMongoRepository<Tenant> _tenants;
        private readonly ILogger<HasPermissionRequirementHandler> _logger;

        public HasPermissionRequirementHandler(
            IHttpContextAccessor httpContextAccessor,
            IMongoRepository<Tenant> tenants,
            ILogger<HasPermissionRequirementHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenants = tenants;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasPermissionRequirement requirement)
        {
            // 1. Verificar que el usuario esté autenticado
            // Lectura más fiable del User ID: primero NameIdentifier, luego el claim de Cognito "sub"
            var nameIdentifier = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(nameIdentifier))
            {
                nameIdentifier = context.User.FindFirstValue(ClaimsUser.Identifier);
            }

            if (string.IsNullOrWhiteSpace(nameIdentifier))
            {
                _logger.LogWarning("User identifier is missing");
                context.Fail(new AuthorizationFailureReason(this, "User identifier is required"));
                return;
            }

            // 2. Obtener roles del usuario desde cognito:groups
            var userRoles = context.User.FindAll(ClaimsUser.Groups)
                .Select(c => c.Value)
                .ToArray();

            // ... el resto de la lógica es correcta ...

            // 3. Verificar si el usuario tiene permiso para la acción
            var hasPermission = CheckPermission(requirement.Action, userRoles);
            if (!hasPermission)
            {
                _logger.LogWarning(
                    "User {UserId} denied for action {Action}. User roles: {Roles}",
                    nameIdentifier,
                    requirement.Action,
                    string.Join(", ", userRoles));
                context.Fail(new AuthorizationFailureReason(
                    this,
                    $"You don't have permission to perform this action: {requirement.Action}"));
                return;
            }

            // 4. Si hay recurso específico, verificar que pertenece al tenant del usuario
            if (!string.IsNullOrWhiteSpace(requirement.ResourceType) &&
                !string.IsNullOrWhiteSpace(requirement.ResourceIdFormElementName))
            {
                var userOrganizationId = context.User.FindFirstValue(ClaimsUser.OrganizationId);
                if (string.IsNullOrWhiteSpace(userOrganizationId))
                {
                    _logger.LogWarning("User organization ID is missing");
                    context.Fail(new AuthorizationFailureReason(this, "Organization information is required"));
                    return;
                }

                // Obtener el ID del recurso
                var resourceId = GetResourceIdFromRequest(requirement.ResourceIdFormElementName);
                if (!string.IsNullOrWhiteSpace(resourceId))
                {
                    var resource = await GetResource(requirement.ResourceType, resourceId);
                    if (resource != null && resource.Id != userOrganizationId)
                    {
                        _logger.LogWarning(
                            "User {UserId} from organization {UserOrg} attempted to access resource {ResourceId} from organization {ResourceOrg}",
                            nameIdentifier,
                            userOrganizationId,
                            resourceId,
                            resource.Id);
                        context.Fail(new AuthorizationFailureReason(
                            this,
                            "You don't have access to this resource"));
                        return;
                    }
                }
            }

            // Todo OK
            _logger.LogInformation(
                "User {UserId} authorized for action {Action}",
                nameIdentifier,
                requirement.Action);
            context.Succeed(requirement);
        }

        private bool CheckPermission(string action, string[] userRoles)
        {
            var permissionMap = new Dictionary<string, string[]>
            {
                // Tenants - Solo Administrator
                { Constants.Actions.AddTenants, new[] { "Administrator" } },
                { Constants.Actions.ReadTenants, new[] { "Administrator" } },
                { Constants.Actions.UpdateTenants, new[] { "Administrator" } },
                { Constants.Actions.DeleteTenants, new[] { "Administrator" } },
                
                // Users - Administrator y Manager
                { Constants.Actions.AddUsers, new[] { "Administrator", "Manager" } },
                { Constants.Actions.ReadUsers, new[] { "Administrator", "Manager", "Operator" } },
                { Constants.Actions.UpdateUsers, new[] { "Administrator", "Manager" } },
                { Constants.Actions.DeleteUsers, new[] { "Administrator", "Manager" } },
                
                // Groups
                { Constants.Actions.ReadGroups, new[] { "Administrator", "Manager" } },
                { Constants.Actions.AddGroups, new[] { "Administrator" } },
                { Constants.Actions.DeleteGroups, new[] { "Administrator" } },
            };

            if (!permissionMap.TryGetValue(action, out var allowedRoles))
            {
                _logger.LogWarning("Action {Action} not found in permission map", action);
                return false;
            }

            return userRoles.Any(role => allowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase));
        }

        private async Task<IEntity?> GetResource(string resourceType, string resourceId)
        {
            var allowedResourceTypes = new List<string> { nameof(Tenant) };

            if (!allowedResourceTypes.Contains(resourceType))
            {
                _logger.LogWarning("Unknown resource type: {ResourceType}", resourceType);
                return null;
            }

            if (resourceType == nameof(Tenant))
            {
                return await _tenants.FindByIdAsync(resourceId);
            }

            return null;
        }

        private string? GetResourceIdFromRequest(string parameterName)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return null;

            // 1. Buscar en route values
            if (httpContext.Request.RouteValues.TryGetValue(parameterName, out var routeValue))
            {
                return routeValue?.ToString();
            }

            // 2. Buscar en query string
            if (httpContext.Request.Query.TryGetValue(parameterName, out var queryValue))
            {
                return queryValue.ToString();
            }

            // 3. Buscar en form
            if (httpContext.Request.HasFormContentType)
            {
                var form = httpContext.Request.Form;
                var formKey = form.Keys.FirstOrDefault(k =>
                    string.Equals(k, parameterName, StringComparison.OrdinalIgnoreCase));

                if (formKey != null && form.TryGetValue(formKey, out var formValues))
                {
                    return formValues.FirstOrDefault();
                }
            }

            return null;
        }
    }
}