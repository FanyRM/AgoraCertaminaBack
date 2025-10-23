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

        public record UserRequest(string Name, string Lastname, List<string> Roles, string Email, string? TenantId);

        public record UserGroupRequest(string Identifier, string GroupName);
        public record CreateTenantRequest(string Name, string Email);
    }
}