namespace AgoraCertaminaBack.Authorization.Settings
{
    public interface ICognitoSettings
    {
        public string Authority { get; init; }
        public string UserPoolId { get; init; }
        public string Domain { get; init; }
        public string ClientId { get; init; }
        public string ClientSecret { get; init; }
        public string RedirectUri { get; init; }
        public int RefreshTokenExpiresIn { get; init; }
    }
}
