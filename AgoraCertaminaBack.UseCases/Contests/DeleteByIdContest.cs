using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Tags;
using ROP;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class DeleteByIdContest(IMongoRepository<Contest> _mongoRepository,GetByIdContest _getByIdContest)
    {
        public async Task<Result<Unit>> Execute(string id)
        {

            return await _getByIdContest.Execute(id)
                .Bind(contest => DeleteAsset(contest));
        }

        private async Task<Result<Unit>> DeleteAsset(Contest contest)
        {
            try
            {
                contest.IsActive = false;
                await _mongoRepository.ReplaceOneAsync(contest);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure<Unit>($"Error deleting asset: {ex.Message}");
            }
        }

    }
}