using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AgoraCertaminaBack.Authorization.AttributeHandler
{
    public class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await _defaultPolicyProvider.GetDefaultPolicyAsync();
        }

        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return await _defaultPolicyProvider.GetDefaultPolicyAsync();
        }

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policyRequirements = await GetPolicyRequirements(policyName);
            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.RequireAuthenticatedUser();
            policyBuilder.AddRequirements(policyRequirements);
            return policyBuilder.Build();
        }

        private static Task<IAuthorizationRequirement[]> GetPolicyRequirements(string policyName)
        {
            if (policyName.StartsWith(Constants.PolicyPrefixes.HasPermissionOnAction))
            {
                var action = policyName.Replace(Constants.PolicyPrefixes.HasPermissionOnAction, "");
                var actionParts = action.Split('_');
                var requirements = new List<IAuthorizationRequirement>();

                if (actionParts.Length == 1 || (actionParts.Length > 0 && actionParts[1] == "" && actionParts[2] == ""))
                {
                    requirements.Add(new HasPermissionRequirement(action: actionParts[0]));
                }
                else if (actionParts.Length >= 4)
                {
                    if (!bool.TryParse(actionParts[3], out var takeResourceDirectly))
                    {
                        throw new InvalidOperationException($"Invalid boolean value for TakeResourceDirectly: {actionParts[3]} in policy {policyName}");
                    }

                    requirements.Add(new HasPermissionRequirement(
                        action: actionParts[0],
                        resourceType: actionParts[1],
                        resourceIdFormElementName: actionParts[2],
                        takeResourceDirectly: takeResourceDirectly
                    ));
                }
                else
                {
                    throw new InvalidOperationException($"Policy string is malformed or has an unexpected number of parts: {policyName}");
                }

                return Task.FromResult(requirements.ToArray());
            }

            throw new NotImplementedException("Unknown policy type");
        }
    }
}