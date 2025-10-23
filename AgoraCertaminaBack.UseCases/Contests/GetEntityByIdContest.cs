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
        public async Task<Result<ContestDTO>> Execute(string assetId)
        {
            try
            {
                var asset = await _mongoRepository.FindOneAsync(a => a.Id == assetId);
                if (asset == null)
                {
                    return Result.NotFound<ContestDTO>("Contest not found");
                }

                var assetDTO = asset.ConvertToContestDTO();
                return Result.Success(assetDTO, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Result.Failure<ContestDTO>($"Error retrieving the contest: {ex.Message}");
            }
        }
    }
}