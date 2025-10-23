using System.Security.Claims;

namespace AgoraCertaminaBack.Models.General
{
    // La clase se instancia en cada peticion como servicio SCOPED en el middleware
    public class UserRequestContext
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string OrganizationId { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public bool IsAuthenticated { get; set; }

        // Métodos helper para verificar roles
        public bool HasRole(string role)
        {
            return Roles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        public bool HasAnyRole(params string[] roles)
        {
            return roles.Any(HasRole);
        }

        public bool IsAdministrator => HasRole("Administrator");
        public bool IsManager => HasRole("Manager");
        public bool IsOperator => HasRole("Operator");
    }

    public static class ClaimsUser
    {
        // Claims de Cognito
        public const string Identifier = "sub"; // Cognito usa "sub" para el user ID
        public const string Email = "email";
        public const string Name = "name";
        public const string FamilyName = "family_name";
        public const string OrganizationId = "custom:organization_id"; // Tu atributo personalizado
        public const string Groups = "cognito:groups"; // Roles/grupos de Cognito
        public const string Username = "cognito:username";

        // Alias para compatibilidad con tu código existente
        public const string Fullname = "name";
    }
}