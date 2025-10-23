using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantTags
{
    public class GetByIdTenantTag(IMongoRepository<Tenant> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<Tag>> Execute(string tagId)
        {
            if (string.IsNullOrEmpty(tagId))
                return Result.Failure<Tag>("Tag ID cannot be null or empty");

            var tenant = await _mongoRepository.FindByIdAsync(_userRequest.OrganizationId);

            if (tenant == null)
                return Result.Failure<Tag>("Tenant not found");

            var tag = tenant.Tags?.FirstOrDefault(t => t.Id == tagId && t.IsActive);

            return tag != null
                ? tag.Success()
                : Result.Failure<Tag>("Tag not found or inactive");
        }
    }
}