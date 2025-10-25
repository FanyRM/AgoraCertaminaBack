using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests.SchemaField
{
    public class DeleteSchemaField(IMongoRepository<SchemaContest> _mongoRepository, GetByIdSchema _getById)
    {
        public async Task<Result<Unit>> Execute(string schemaId, string schemaFieldId)
        {
            return await _getById.Execute(schemaId)
                .Bind(schema => DeleteField(schema, schemaFieldId));
        }

        public async Task<Result<Unit>> DeleteField(SchemaContest schema, string schemaFieldId)
        {
            var fieldToDelete = schema.SchemaFields.FirstOrDefault(field => field.Id == schemaFieldId);

            if (fieldToDelete == null)
            {
                return Result.NotFound<Unit>("Tag not found");
            }

            if (!fieldToDelete.IsActive)
            {
                return Result.NotFound<Unit>("Already deleted");
            }

            fieldToDelete.IsActive = false;

            await _mongoRepository.ReplaceOneAsync(schema);

            return Result.Success();
        }
    }
}