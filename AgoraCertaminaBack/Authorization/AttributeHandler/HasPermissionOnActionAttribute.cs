using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace AgoraCertaminaBack.Authorization.AttributeHandler
{
    public class HasPermissionOnActionAttribute : AuthorizeAttribute
    {
        public HasPermissionOnActionAttribute(string actionName, string resourceType = "", string resourceIdFormElementName = "", bool takeResourceDirectly = false)
        {
            Policy = $"{Constants.PolicyPrefixes.HasPermissionOnAction}{actionName}_{resourceType}_{resourceIdFormElementName}_{takeResourceDirectly}";
        }
    }
}



