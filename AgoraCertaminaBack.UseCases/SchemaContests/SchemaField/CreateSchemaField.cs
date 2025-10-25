using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.SchemaContests.SchemaField
{
    public class CreateSchemaField(IMongoRepository<SchemaContest> _mongoRepository, GetByIdSchema _getByIdSchema, IMongoRepository<Tenant> _tenantRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(string schemaId, FieldRequest request)
        {
            return await _getByIdSchema.Execute(schemaId)
                .Bind(schema => ValidateUniqueFieldName(schema, request))
                .Bind(schema => ValidateCatalog(schema, request))
                .Bind(schema => AddSchemaField(schema, request));
        }

        public static Result<SchemaContest> ValidateUniqueFieldName(SchemaContest schema, FieldRequest request)
        {
            bool fieldExists = schema.SchemaFields.Any(f =>
                f.IsActive &&
                f.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)
            );
            if (fieldExists)
                return Result.NotFound<SchemaContest>("A field with the same name already exists in this schema");
            return schema.Success();
        }

        private async Task<Result<SchemaContest>> ValidateCatalog(SchemaContest schema, FieldRequest request)
        {
            // validacion para campos de tipo CustomCatalog
            if (request.Type != FieldTypeEnum.CustomCatalog)
                return schema.Success();

            if (string.IsNullOrEmpty(request.CatalogId))
                return Result.Failure<SchemaContest>("A catalog must be provided");

            // se obtiene el cliente actual para verificar que el catalogo exista
            var customer = await _tenantRepository.FindByIdAsync(_userRequest.OrganizationId);
            if (customer == null)
                return Result.NotFound<SchemaContest>("Customer not found");

            var catalogExists = customer.Catalogs.Any(c => c.Id == request.CatalogId && c.IsActive);
            if (!catalogExists)
                return Result.Failure<SchemaContest>("The catalog does not exist or is not active");

            return schema.Success();
        }

        public async Task<Result<string>> AddSchemaField(SchemaContest schema, FieldRequest request)
        {
            var customField = request.FieldTypeRequestToFieldType(isBase: false);
            schema.SchemaFields.Add(customField);

            await _mongoRepository.ReplaceOneAsync(schema);
            return customField.Id.Success(HttpStatusCode.Created);
        }
    }
}