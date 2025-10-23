using AgoraCertaminaBack.Models.General;
using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.DTOs.Contest;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class GetDisabledContests(IMongoRepository<Contest> mongoRepository, UserRequestContext userRequest, ConvertToContestDTO convertToContestDTO)
    {
        public async Task<Result<List<ContestDTO>>> Execute()
        {
            return await getAssets()
                .Bind(convertToContestDTO.Execute);
        }

        private async Task<Result<List<Contest>>> getAssets()
        {
            var assets = await mongoRepository.FilterByAsync(asset =>
                asset.IsActive == false &&
                asset.CustomerId == userRequest.CustomerId
            );

            if (assets == null || !assets.Any())
                return Result.NotFound<List<Contest>>("Assets not found");
            return Result.Success(assets.ToList());
        }
    }

}