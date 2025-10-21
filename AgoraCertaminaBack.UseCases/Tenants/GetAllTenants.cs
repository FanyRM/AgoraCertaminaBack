using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Tenant;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Tenants
{
    public class GetAllTenants(IMongoRepository<Tenant> _mongoRepository)
    {
        public async Task<Result<List<TenantDTO>>> Execute()
        {
            var tenants = await _mongoRepository.FilterByAsync(tenant => tenant.IsActive == true);

            var tenantsDTO = tenants.Select(tenant => new TenantDTO
            {
                Id = tenant.Id,
                TenantName = tenant.TenantName,
                CatalogsCount = tenant.Catalogs.Count(c => c.IsActive),
                CreatedAt = tenant.CreatedAt,
                IsActive = tenant.IsActive
            })
            .ToList();

            return tenantsDTO.Success();
        }
    }
}