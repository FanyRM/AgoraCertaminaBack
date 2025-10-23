using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using ROP;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class GetContests(IMongoRepository<Contest> mongoRepository,ConvertToContestDTO convertToContestDTO,
        UserRequestContext userRequest)
    {
        public async Task<Result<List<ContestDTO>>> Execute() {
            return await getAssets()
                .Bind(convertToContestDTO.Execute);
        }

        private async Task<Result<List<Contest>>> getAssets()
        {
            var assets = await mongoRepository.FilterByAsync(asset =>
                asset.IsActive &&
                asset.CustomerId == userRequest.CustomerId);

            if (assets == null || !assets.Any())
                return Result.NotFound<List<Contest>>("Assets not found");
            return Result.Success(assets.ToList());
        }
    }
}