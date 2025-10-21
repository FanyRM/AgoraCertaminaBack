using System.Security.Claims;

namespace AgoraCertaminaBack.Models.General
{
    //La clase se instancia en cada peticion como servicio SCOPED en el middleware
    public class UserRequestContext
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string OrganizationId { get; set; }
    }

    public static class ClaimsUser
    {
        public static readonly string Identifier = ClaimTypes.NameIdentifier;
        public static readonly string Fullname = "fullname";
        public static readonly string Email = ClaimTypes.Email;
        public static readonly string TenantId = "tenant_id";
    }
}
