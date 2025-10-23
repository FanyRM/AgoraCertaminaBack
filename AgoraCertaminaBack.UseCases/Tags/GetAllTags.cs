using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using ROP;

namespace AgoraCertaminaBack.UseCases.Tags
{
    public class GetAllTags(IMongoRepository<Customer> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<List<Tag>>> Execute()
        {
            var customTags = await _mongoRepository.FilterByAsync(
                filter => filter.Id == _userRequest.CustomerId,
                project => project.Tags.Where(tag => tag.IsActive).ToList());

            return customTags;
        }
    }
}