using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.Models.DTOs.CustomCatalog;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantCatalogs
{
    public class GetByIdTenantCatalog(IMongoRepository<Tenant> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<CustomCatalogDTO>> Execute(string catalogId, string? tenantId = null)
        {
            var tenant = await _mongoRepository.FindOneAsync(filter => filter.Id == (tenantId ?? _userRequest.OrganizationId));

            if (tenant == null || tenant.Catalogs == null)
            {
                return Result.Failure<CustomCatalogDTO>("Tenant or catalogs not found");
            }

            var catalog = tenant.Catalogs.FirstOrDefault(catalog => catalog.Id == catalogId && catalog.IsActive);

            if (catalog == null)
            {
                return Result.Failure<CustomCatalogDTO>("Catalog not found or inactive");
            }

            return catalog.ToCustomCatalogDTO().Success();
        }
    }
}