using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class GetEntityByIdSchema(IMongoRepository<SchemaContest> _mongoRepository)
    {
        public async Task<Result<SchemaContestDTOs>> Execute(string schemaId)
        {
            var schema = await _mongoRepository.FindByIdAsync(schemaId);

            if (schema == null)
                return Result.NotFound<SchemaContestDTOs>("Schema not found");

            return schema.ToSchemaContestDTO();
        }
    }
}
