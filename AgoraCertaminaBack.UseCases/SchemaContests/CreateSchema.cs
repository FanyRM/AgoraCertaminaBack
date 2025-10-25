using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.UseCases.Tenants;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class CreateSchema(IMongoRepository<SchemaContest> _mongoRepository, GetByIdTenant _getByIdTenant, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(CreateSchemaContestRequest request)
        {
            var result = await _getByIdTenant.Execute(_userRequest.OrganizationId)
                .Bind(tenant => ValidateUniqueSchemaName(tenant, request))
                .Bind(tenant => CreateNewSchema(tenant, request));

            return result;
        }

        private async Task<Result<Tenant>> ValidateUniqueSchemaName(Tenant tenant, CreateSchemaContestRequest request)
        {
            bool schemaExists = await _mongoRepository.ExistsAsync(s =>
                s.IsActive &&
                s.OrganizationId == tenant.Id &&
                s.SchemaName == request.SchemaName
            );

            if (schemaExists)
                return Result.Failure<Tenant>("A schema with this name already exists");

            return tenant.Success();
        }

        private async Task<Result<string>> CreateNewSchema(Tenant tenant, CreateSchemaContestRequest request)
        {
            var tags = request.Tags.Select(t => t.ToCustomTag()).ToList();

            var newSchema = new SchemaContest
            {
                OrganizationId = tenant.Id,
                OrganizationName = tenant.TenantName,
                SchemaName = request.SchemaName,
                Tags = tags,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _mongoRepository.InsertOneAsync(newSchema);

            return newSchema.Id.Success(HttpStatusCode.Created);
        }
    }
}