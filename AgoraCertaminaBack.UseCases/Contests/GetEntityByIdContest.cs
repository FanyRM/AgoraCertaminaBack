using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class GetEntityByIdContest(IMongoRepository<Contest> _mongoRepository)
    {
        public async Task<Result<ContestDTO>> Execute(string contestId)
        {
            try
            {
                var contest = await _mongoRepository.FindOneAsync(a => a.Id == contestId);
                if (contest == null)
                {
                    return Result.NotFound<ContestDTO>("Contest not found");
                }

                var contestDTO = contest.ConvertToContestDTO();
                return Result.Success(contestDTO, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Result.Failure<ContestDTO>($"Error retrieving the contest: {ex.Message}");
            }
        }
    }
}