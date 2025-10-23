using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using ROP;

namespace AgoraCertaminaBack.UseCases.Tags
{
    public class GetByIdTag(IMongoRepository<Customer> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<Tag>> Execute(string tagId)
        {
            try
            {
                if (string.IsNullOrEmpty(tagId))
                    return Result.Failure<Tag>("Tag ID cannot be null or empty");

                var customer = await _mongoRepository.FindByIdAsync(_userRequest.CustomerId);

                if (customer == null)
                    return Result.Failure<Tag>("Customer not found");

                var tag = customer.Tags?.FirstOrDefault(t => t.Id == tagId && t.IsActive);

                return tag != null
                    ? Result.Success(tag)
                    : Result.Failure<Tag>("Tag not found or inactive");
            }
            catch (Exception ex)
            {
                return Result.Failure<Tag>($"Error retrieving tag: {ex.Message}");
            }
        }
    }
}