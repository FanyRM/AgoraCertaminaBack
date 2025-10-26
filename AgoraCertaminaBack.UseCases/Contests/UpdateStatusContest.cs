using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using ROP;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class UpdateStatusContest (IMongoRepository<Contest> _mongoRepository, GetByIdContest _getByIdContest, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(string contestId, ContestStatusUpdateRequest request)
        {
            var contest = await _mongoRepository.FindOneAsync(a => 
                a.Id == contestId && 
                a.IsActive && 
                a.OrganizationId == _userRequest.OrganizationId);

            if (contest == null)
            {
                return Result.NotFound<string>("Contest not found");
            }

            contest.IsSuspended = request.IsSuspended;
            contest.IsEvalued = request.IsEvalued;

            await _mongoRepository.ReplaceOneAsync(contest);
            return Result.Success("Contest status updated successfully");

        }
    }
}
