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
            return await getContests()
                .Bind(convertToContestDTO.Execute);
        }

        private async Task<Result<List<Contest>>> getContests()
        {
            var contests = await mongoRepository.FilterByAsync(contest =>
                contest.IsActive == false &&
                contest.OrganizationId == userRequest.OrganizationId
            );

            if (contests == null || !contests.Any())
                return Result.NotFound<List<Contest>>("Contests not found");
            return Result.Success(contests.ToList());
        }
    }

}