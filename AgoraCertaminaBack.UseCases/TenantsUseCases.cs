

using AgoraCertaminaBack.UseCases.Tenants;
using AgoraCertaminaBack.UseCases.Tenants.TenantCatalogs;
using AgoraCertaminaBack.UseCases.Tenants.TenantTags;

namespace AgoraCertaminaBack.UseCases
{
    public record class TenantCatalogUseCases(
        CreateTenantCatalog CreateTenantCatalog,
        GetAllTenantCatalogs GetAllTenantCatalogs,
        GetByIdTenantCatalog GetByIdTenantCatalog,
        UpdateTenantCatalogValues UpdateTenantCatalogValues,
        DeleteTenantCatalog DeleteTenantCatalog
        //GetByIdFormProgrammedCatalog GetByIdFormProgrammedCatalog
    );

    public record class TenantsUseCases(
        GetByIdTenant GetByIdTenant,
        GetAllTenants GetAllTenants,
        CreateTenant CreateTenant,
        DeleteByIdTenant DeleteByIdTenant
    );

    public record class TenantTagUseCases(
        CreateTenantTag CreateTenantTag,
        GetAllTenantTags GetAllTenantTags,
        GetByIdTenantTag GetByIdTenantTag,
        UpdateTenantTag UpdateTenantTag,
        DeleteTenantTag DeleteTenantTag
    );
}
