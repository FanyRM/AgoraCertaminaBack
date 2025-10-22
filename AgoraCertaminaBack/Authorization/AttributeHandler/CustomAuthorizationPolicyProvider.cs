using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

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
            policyBuilder.AddRequirements(policyRequirements);
            return policyBuilder.Build();
        }

        private static Task<IAuthorizationRequirement[]> GetPolicyRequirements(string policyName)
        {
            if (policyName.StartsWith(Constants.PolicyPrefixes.HasPermissionOnAction))
            {
                var action = policyName.Replace(Constants.PolicyPrefixes.HasPermissionOnAction, "");
                var actionParts = action.Split("_");

                var requirements = new IAuthorizationRequirement[]
                {
                        new HasPermissionRequirement(action: actionParts[0],
                                                     resourceType: actionParts[1],
                                                     resourceIdFormElementName: actionParts[2],
                                                     takeResourceDirectly:bool.Parse(actionParts[3]))
                };

                return Task.FromResult(requirements);
            }

            throw new NotImplementedException("Unknown policy type");
        }
    }

}
