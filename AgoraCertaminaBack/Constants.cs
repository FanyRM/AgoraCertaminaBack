namespace AgoraCertaminaBack
{
    public class Constants
    {
        public static class Roles
        {
            public const string Administrator = "Administrator";
            public const string Manager = "Manager";
            public const string Operator = "Operator";
            public const string Viewer = "Viewer";
        }

        public static class Actions
        {
            public const string AddUsers = "AddUsers";
            public const string ReadUsers = "ReadUsers";
            public const string UpdateUsers = "UpdateUsers";
            public const string DeleteUsers = "DeleteUsers";

            public const string AddGroups = "AddGroups";
            public const string ReadGroups = "ReadGroups";
            public const string DeleteGroups = "DeleteGroups";

            public const string AddTenants = "AddTenants";
            public const string ReadTenants = "ReadTenants";
            public const string UpdateTenants = "UpdateTenants";
            public const string DeleteTenants = "DeleteTenants";

            public const string AddTenantCatalogs = "AddTenantCatalogs";
            public const string ReadTenantCatalogs = "ReadTenantCatalogs";
            public const string UpdateTenantCatalogs = "UpdateTenantCatalogs";
            public const string DeleteTenantCatalogs = "DeleteTenantCatalogs";

            public const string AddTenantTags = "AddTenantTags";
            public const string ReadTenantTags = "ReadTenantTags";
            public const string DeleteTenantTags = "DeleteTenantTags";

            public const string AddParticipants = "AddParticipants";
            public const string ReadParticipants = "ReadParticipants";
            public const string UpdateParticipants = "UpdateParticipants";
            public const string DeleteParticipants = "DeleteParticipants";

            public const string AddForms = "AddForms";
            public const string ReadForms = "ReadForms";
            public const string DeleteForms = "DeleteForms";

            public const string AddFormProgrammed = "AddFormProgrammed";
            public const string ReadFormProgrammed = "ReadFormProgrammed";
            public const string UpdateFormProgrammed = "UpdateFormProgrammed";
            public const string DeleteFormProgrammed = "DeleteFormProgrammed";

            public const string AddFormAssigned = "AddFormAssigned";
            public const string ReadFormAssigned = "ReadFormAssigned";
            public const string UpdateFormAssigned = "UpdateFormAssigned";
            public const string DeleteFormAssigned = "DeleteFormAssigned";

            public const string AddFormFields = "AddFormFields";
            public const string ReadFormFields = "ReadFormFields";
            public const string UpdateFormFields = "UpdateFormFields";
            public const string DeleteFormFields = "DeleteFormFields";

            public const string AddFormTags = "AddFormTags";
            public const string ReadFormTags = "ReadFormTags";
            public const string DeleteFormTags = "DeleteFormTags";

            public const string AddResponse = "AddResponse";
            public const string ReadResponse = "ReadResponse";
            public const string UpdateResponse = "UpdateResponse";
            public const string DeleteResponse = "DeleteResponse";
        }

        public static class PolicyPrefixes
        {
            public const string HasPermissionOnAction = "HasPermissionOnAction_";
        }
    }
}
