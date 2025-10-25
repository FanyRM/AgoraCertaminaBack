using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests.SchemaTag
{
    public class DeleteSchemaTag(IMongoRepository<SchemaContest> _mongoRepository, GetByIdSchema _getByIdSchema)
    {
        public async Task<Result<Unit>> Execute(string schemaId, string schemaTagId)
        {
            return await _getByIdSchema.Execute(schemaId)
                .Bind(schema => DeleteTag(schema, schemaTagId));
        }

        public async Task<Result<Unit>> DeleteTag(SchemaContest schema, string schemaTagId)
        {
            var tagToDelete = schema.Tags.FirstOrDefault(tag => tag.Id == schemaTagId);

            if (tagToDelete == null)
            {
                return Result.NotFound<Unit>("Tag not found");
            }

            if (!tagToDelete.IsActive)
            {
                return Result.NotFound<Unit>("Already deleted");
            }

            tagToDelete.IsActive = false;

            await _mongoRepository.ReplaceOneAsync(schema);

            return Result.Success();
        }
    }
}