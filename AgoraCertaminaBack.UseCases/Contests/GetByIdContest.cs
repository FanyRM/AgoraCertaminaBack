using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class GetByIdContest(IMongoRepository<Contest> _mongoRepository)
    {
        public async Task<Result<Contest>> Execute(string contestId)
        {
            var contest = await _mongoRepository.FindOneAsync(a => a.Id == contestId && a.IsActive);

            return contest ?? Result.NotFound<Contest>("Asset not found");
        }
    }
}
