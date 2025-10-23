using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests.SchemaField
{
    public class GetAllSchemaFields(IMongoRepository<SchemaContest> _mongoRepository)
    {
        public async Task<Result<List<Field>>> Execute(string schemaId)
        {
            var schemaFields = await _mongoRepository.FilterByAsync(
                filter => filter.Id == schemaId,
                project => project.SchemaFields.Where(field => field.IsActive).ToList()
            );

            return schemaFields;
        }
    }
}