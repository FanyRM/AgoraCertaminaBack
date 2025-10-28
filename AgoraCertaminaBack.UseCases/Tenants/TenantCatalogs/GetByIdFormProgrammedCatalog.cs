using ROP;
using AgoraCertaminaBack.Models.DTOs.CustomCatalog;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantCatalogs
{
    public class GetByIdFormCatalog(
        GetFormToResponse _getFormToResponse,
        GetByIdTenantCatalog _getByIdTenantCatalog)
    {
        public async Task<Result<CustomCatalogDTO>> Execute(string formId, string catalogId)
        {
            return await _getFormToResponse.Execute(formId)
                .Bind(formDto => _getByIdTenantCatalog.Execute(catalogId, formDto.OrganizationId));
        }
    }
}