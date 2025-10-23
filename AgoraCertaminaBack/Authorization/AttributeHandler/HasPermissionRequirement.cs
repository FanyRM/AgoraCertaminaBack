using Microsoft.AspNetCore.Authorization;

namespace AgoraCertaminaBack.Authorization.AttributeHandler
{
    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        public HasPermissionRequirement(string action)
        {
            Action = action;
            ResourceType = string.Empty;
            ResourceIdFormElementName = string.Empty;
            TakeResourceIdDirectly = false;
        }

        public HasPermissionRequirement(string action, string resourceType, string resourceIdFormElementName, bool takeResourceDirectly)
        {
            Action = action;
            ResourceType = resourceType;
            ResourceIdFormElementName = resourceIdFormElementName;
            TakeResourceIdDirectly = takeResourceDirectly;
        }

        public string Action { get; }
        public string ResourceType { get; set; }
        public string ResourceIdFormElementName { get; set; }
        public bool TakeResourceIdDirectly { get; set; }
    }
}