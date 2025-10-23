using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Customers;
using ROP;

namespace AgoraCertaminaBack.UseCases.Tags
{
    public class DeleteTag(IMongoRepository<Customer> _mongoRepository, IMongoRepository<Contest> _assetRepository, GetByIdCustomer _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<Unit>> Execute(string customerTagId)
        {
            return await _getById.Execute(_userRequest.CustomerId)
                .Bind(customer => CheckIfTagIsInUse(customer.Id, customerTagId))
                .Bind(_ => _getById.Execute(_userRequest.CustomerId))
                .Bind(customer => DeleteTagExecute(customer, customerTagId));
        }

        public async Task<Result<Unit>> DeleteTagExecute(Customer customer, string customerTagId)
        {
            var tagToDelete = customer.Tags.Find(field => field.Id == customerTagId);

            if (tagToDelete == null)
                return Result.NotFound<Unit>("Tag not found");

            if (!tagToDelete.IsActive)
                return Result.NotFound<Unit>("Already deleted");

            tagToDelete.IsActive = false;

            await _mongoRepository.ReplaceOneAsync(customer);

            return Result.Success();

        }

        private async Task<Result<Unit>> CheckIfTagIsInUse(string customerId, string tagId)
        {
            var allAssets = await _assetRepository.GetAllAsync();

            var assetsWithTag = allAssets
                .Where(asset =>
                    asset.CustomerId == customerId &&
                    asset.IsActive &&
                    asset.Tags.Any(tag => tag.Id == tagId))
                .ToList();

            if (assetsWithTag.Any())
            {
                var assetCount = assetsWithTag.Count;
                return Result.Failure($"Cannot delete tag because it is being used in {assetCount} asset(s).");
            }

            return Result.Success();
        }
    }
}
