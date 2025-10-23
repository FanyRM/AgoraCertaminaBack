using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantTags
{
    public class DeleteTenantTag(IMongoRepository<Tenant> _mongoRepository, GetByIdTenant _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<Unit>> Execute(string customerTagId)
        {
            return await _getById.Execute(_userRequest.OrganizationId)
                .Bind(tenant => DeleteTag(tenant, customerTagId));
        }

        public async Task<Result<Unit>> DeleteTag(Tenant tenant, string customerTagId)
        {
            var tagToDelete = tenant.Tags.Find(field => field.Id == customerTagId);

            if (tagToDelete == null)
                return Result.NotFound<Unit>("Tag not found");

            if (!tagToDelete.IsActive)
                return Result.NotFound<Unit>("Already deleted");

            tagToDelete.IsActive = false;

            await _mongoRepository.ReplaceOneAsync(tenant);

            return Result.Success();

        }
    }
}
