namespace AgoraCertaminaBack.Authorization.Settings
{
    public class CognitoSettings : ICognitoSettings
    {
        public string Authority { get; init; } = null!;
        public string UserPoolId { get; init; } = null!;
        public string Domain { get; init; } = null!;
        public string ClientId { get; init; } = null!;
        public string ClientSecret { get; init; } = null!;
        public string RedirectUri { get; init; } = null!;
        public int RefreshTokenExpiresIn { get; init; }
    }
}
