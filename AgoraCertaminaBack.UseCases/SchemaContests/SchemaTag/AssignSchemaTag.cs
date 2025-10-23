using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.UseCases.SchemaContests;
using ROP;
using System.Net;

namespace ControlStockAPI.UseCases.SchemaContests.SchemaTag
{
    public class AssignSchemaTag(IMongoRepository<SchemaContest> _mongoRepository, GetByIdSchema _getById)
    {
        public async Task<Result<Tag>> Execute(string schemaId, SchemaTagRequest request)
        {
            return await _getById.Execute(schemaId)
                .Bind(schema => ValidateUniqueTagName(schema, request))
                .Bind(schema => AddSchemaTag(schema, request));
        }

        public static Result<SchemaContest> ValidateUniqueTagName(SchemaContest schema, SchemaTagRequest request)
        {
            bool tagExists = schema.Tags.Any(t =>
                t.IsActive &&
                t.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)
            );

            if (tagExists)
                return Result.Failure<SchemaContest>("A tag with the same name already exists in this schema");

            return schema.Success();
        }

        public async Task<Result<Tag>> AddSchemaTag(SchemaContest schema, SchemaTagRequest request)
        {
            var newTag = request.ToCustomTag();

            schema.Tags.Add(newTag);

            await _mongoRepository.ReplaceOneAsync(schema);

            return newTag.Success(HttpStatusCode.OK);
        }
    }
}