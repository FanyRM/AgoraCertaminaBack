using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.Models.DTOs.CustomCatalog;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantCatalogs
{
    public class GetAllTenantCatalogs(IMongoRepository<Tenant> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<List<CustomCatalogDTO>>> Execute()
        {
            var tenant = await _mongoRepository.FindOneAsync(
                    filter => filter.Id == _userRequest.OrganizationId);

            if (tenant == null || tenant.Catalogs == null)
            {
                return new List<CustomCatalogDTO>().Success();
            }

            return tenant.Catalogs
                .Where(catalog => catalog.IsActive)
                .Select(catalog => catalog.ToCustomCatalogDTO())
                .ToList()
                .Success();
        }
    }
}