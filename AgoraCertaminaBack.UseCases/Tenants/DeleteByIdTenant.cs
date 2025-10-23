using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Tenants
{
    public class DeleteByIdTenant(IMongoRepository<Tenant> _mongoRepository, GetByIdTenant _getById)
    {
        public async Task<Result<Unit>> Execute(string customerId)
        {
            return await _getById.Execute(customerId)
                .Bind(DeleteTenant);
        }

        public async Task<Result<Unit>> DeleteTenant(Tenant tenant)
        {
            if (!tenant.IsActive)
                return Result.NotFound<Unit>("Tenant not found or already deleted");

            tenant.IsActive = false;

            await _mongoRepository.ReplaceOneAsync(tenant);

            return Result.Success();
        }
    }
}
