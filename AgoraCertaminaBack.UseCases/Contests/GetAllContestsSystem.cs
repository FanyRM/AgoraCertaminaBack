using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class GetAllContestsSystem(IMongoRepository<Contest> mongoRepository, ConvertToContestDTO convertToContestDTO)
    {
        public async Task<Result<List<ContestDTO>>> Execute()
        {
            return await getContests()
                .Bind(convertToContestDTO.Execute);
        }

        private async Task<Result<List<Contest>>> getContests()
        {
            var contests = await mongoRepository.FilterByAsync(contest =>
                contest.IsActive);

            if (contests == null || !contests.Any())
                return Result.NotFound<List<Contest>>("Contest not found");
            return Result.Success(contests.ToList());
        }
    }
}
