using System;
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
            return await getContests()
                .Bind(convertToContestDTO.Execute);
        }

        private async Task<Result<List<Contest>>> getContests()
        {
            var contests = await mongoRepository.FilterByAsync(contest =>
                contest.IsActive &&
                contest.OrganizationId == userRequest.OrganizationId);

            if (contests == null || !contests.Any())
                return Result.NotFound<List<Contest>>("Contest not found");
            return Result.Success(contests.ToList());
        }
    }
}