using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantTags
{
    public class GetAllTenantTags(IMongoRepository<Tenant> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<List<Tag>>> Execute(List<TagCategory>? categories)
        {
            var customTags = await _mongoRepository.FilterByAsync(
                filter => filter.Id == _userRequest.OrganizationId,
                project => project.Tags.Where(tag => tag.IsActive).ToList());

            if (categories != null && categories.Count > 0)
            {
                customTags = customTags.Where(tag => categories.Contains(tag.Category)).ToList();
            }

            return customTags;
        }
    }
}