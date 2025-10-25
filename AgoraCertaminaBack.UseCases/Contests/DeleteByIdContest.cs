using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class DeleteByIdContest(IMongoRepository<Contest> _mongoRepository,GetByIdContest _getByIdContest)
    {
        public async Task<Result<Unit>> Execute(string id)
        {

            return await _getByIdContest.Execute(id)
                .Bind(contest => DeleteContest(contest));
        }

        private async Task<Result<Unit>> DeleteContest(Contest contest)
        {
            try
            {
                contest.IsActive = false;
                await _mongoRepository.ReplaceOneAsync(contest);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure<Unit>($"Error deleting contest: {ex.Message}");
            }
        }

    }
}