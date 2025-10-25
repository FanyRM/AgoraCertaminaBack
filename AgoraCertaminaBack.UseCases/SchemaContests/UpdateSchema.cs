using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;
using MongoDB.Driver;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class UpdateSchema(IMongoRepository<SchemaContest> _mongoRepository)
    {
        public async Task<Result<SchemaContest>> Execute(string schemaId, UpdateSchemaRequest request)
        {
            return await ValidateSchemaExist(schemaId)
                .Bind(schema => ValidateUniqueSchemaName(schema, request))
                .Bind(schema => UpdateSchemaData(schema, request));
        }

        private async Task<Result<SchemaContest>> ValidateSchemaExist(string schemaId)
        {
            var schema = await _mongoRepository.FindByIdAsync(schemaId);

            if (schema == null || !schema.IsActive)
                return Result.Failure<SchemaContest>("Schema not found or inactive");

            return schema.Success();
        }

        private async Task<Result<SchemaContest>> ValidateUniqueSchemaName(SchemaContest schema, UpdateSchemaRequest request)
        {
            // Valida solo si el nombre cambió
            if (request.SchemaName == schema.SchemaName)
                return schema.Success();

            bool schemaExists = await _mongoRepository.ExistsAsync(s =>
            s.IsActive &&
            s.OrganizationId == schema.OrganizationId &&
            s.SchemaName == request.SchemaName &&
            s.Id != schema.Id); // para excluir el actual

            if (schemaExists)
                return Result.Failure<SchemaContest>("A schema with this name already exist for the customer");

            return schema.Success();
        }

        private async Task<Result<SchemaContest>> UpdateSchemaData(SchemaContest schema, UpdateSchemaRequest request)
        {
            schema.SchemaName = request.SchemaName;
            schema.Tags = request.Tags.Select(t => t.ToCustomTag()).ToList();

            await _mongoRepository.ReplaceOneAsync(schema);

            return schema.Success();
        }
    }
}

