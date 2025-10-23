    using Newtonsoft.Json;

namespace AgoraCertaminaBack.Authorization
{
    public class AuthenticationEntities
    {
        public class TokenResponse
        {
            [JsonProperty("id_token")]
            public required string IDToken { get; set; }

            [JsonProperty("access_token")]
            public required string AccessToken { get; set; }

            [JsonProperty("refresh_token")]
            public required string RefreshToken { get; set; }

            [JsonProperty("expires_in")]
            public required int ExpiresIn { get; set; }
        }

        public record TokenDTO(string IdentityToken, int ExpiresIn);

        // ✅ Cambiado de record a class para permitir inicialización flexible
        public class UserRequest
        {
            public required string Name { get; set; }
            public required string Lastname { get; set; }
            public required List<string> Roles { get; set; }
            public required string Email { get; set; }
            public string? OrganizationId { get; set; }
        }

        public record UserGroupRequest(string Identifier, string GroupName);

        public record CreateTenantRequest(string Name, string Email);
    }
}