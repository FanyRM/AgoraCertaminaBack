using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Tenants
{
    public class GetByIdTenant(IMongoRepository<Tenant> _mongoRepository)
    {
        public async Task<Result<Tenant>> Execute(string tenantId)
        {
            var tenant = await _mongoRepository.FindByIdAsync(tenantId);

            if (tenant == null)
                return Result.NotFound<Tenant>("Tenant not found");

            return tenant;
        }
    }
}
