using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class GetByIdSchema(IMongoRepository<SchemaContest> _mongoRepository)
    {
        public async Task<Result<SchemaContest>> Execute(string schemaId)
        {
            var schema = await _mongoRepository.FindByIdAsync(schemaId);

            if (schema == null)
                return Result.NotFound<SchemaContest>("Schema not found");

            return schema;
        }
    }
}
