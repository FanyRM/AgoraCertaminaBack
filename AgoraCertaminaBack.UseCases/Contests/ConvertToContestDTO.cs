using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using ROP;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class ConvertToContestDTO()
    {
        public async Task<Result<List<ContestDTO>>> Execute(List<Contest> contests)
        {
            var contestsIds = contests.Select(a => a.Id);
            var contestsReferences = contests.Select(a => a.ReferenceNumber);

            var contestsDTOs = contests.Select(contest =>
            {
                var dto = contest.ConvertToContestDTO();

                dto.Status = (contest.IsActive, contest.IsSuspended, contest.IsEvalued, DateTime.Now) switch
                {
                    (false, _, _, _) => ContestStatusEnum.Inactive,
                    (true, true, _, _) => ContestStatusEnum.Suspended,
                    (true, false, true, _) => ContestStatusEnum.Evaluation,
                    (true, false, false, var now) when now > contest.EndDate => ContestStatusEnum.Ended,
                    (true, false, false, var now) when now < contest.EndDate => ContestStatusEnum.Active,
                    _ => ContestStatusEnum.Inactive
                };
                return dto;
            }).ToList();

            return contestsDTOs.Success();
        }
    }
}