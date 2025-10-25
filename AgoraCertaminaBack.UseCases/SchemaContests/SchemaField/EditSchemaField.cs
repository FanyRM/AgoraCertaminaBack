using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.SchemaContests.SchemaField
{
    // No se utilizará en el proyecto por complejidad
    public class EditSchemaField(IMongoRepository<SchemaContest> _mongoRepository, GetByIdSchema _getById)
    {
        public async Task<Result<Field>> Execute(string schemaId, UpdateSchemaFieldRequest request)
        {
            return await _getById.Execute(schemaId)
                .Bind(schema => UpdateSchemaField(schema, request));
        }

        public async Task<Result<Field>> UpdateSchemaField(SchemaContest schema, UpdateSchemaFieldRequest request)
        {
            var fieldToUpdate = schema.SchemaFields.FirstOrDefault(field => field.Id == request.Id);

            if (fieldToUpdate == null)
            {
                return Result.NotFound<Field>("Field not found");
            }

            fieldToUpdate.Name = request.Name;
            fieldToUpdate.Type = request.Type;
            fieldToUpdate.IsRequired = request.IsRequired;
            fieldToUpdate.IsBase = request.IsBase;

            await _mongoRepository.ReplaceOneAsync(schema);

            return fieldToUpdate.Success(HttpStatusCode.OK);
        }
    }
}