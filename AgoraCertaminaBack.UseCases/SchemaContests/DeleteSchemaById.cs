using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class DeleteSchemaById(IMongoRepository<SchemaContest> mongoRepository, IMongoRepository<Contest> assetRepository, GetByIdSchema getByIdSchema)
    {
        public async Task<Result<Unit>> Execute(string schemaId)
        {
            return await getByIdSchema.Execute(schemaId)
                .Bind(ValidateSchemaCanBeDeleted)
                .Bind(DeleteSchema);
        }

        private async Task<Result<SchemaContest>> ValidateSchemaCanBeDeleted(SchemaContest schema)
        {
            // Verificar si existen assets activos que usen este schema
            var activeAssets = await assetRepository.FindOneAsync(a =>
                a.SchemaId == schema.Id && a.IsActive == true);

            if (activeAssets != null)
            {
                return Result.Failure<SchemaContest>("The schema cannot be deleted because it has associated active assets");
            }

            return Result.Success(schema);
        }

        public async Task<Result<Unit>> DeleteSchema(SchemaContest schema)
        {
            schema.IsActive = false;
            await mongoRepository.ReplaceOneAsync(schema);
            return Result.Success();
        }
    }
}