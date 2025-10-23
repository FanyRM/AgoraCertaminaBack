using MongoDB.Bson;
using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantCatalogs
{
    public class UpdateTenantCatalogValues(IMongoRepository<Tenant> _mongoRepository, GetByIdTenant _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(string catalogId, UpdateCustomCatalogValuesRequest request)
        {
            return await _getById.Execute(_userRequest.OrganizationId)
                .Bind(tenant => FindCatalog(tenant, catalogId))
                .Bind(tuple => UpdateCatalogValues(tuple.Tenant, tuple.Catalog, request.Values));
        }

        private static Result<(Tenant Tenant, Catalog Catalog)> FindCatalog(Tenant tenant, string catalogId)
        {
            var catalog = tenant.Catalogs.FirstOrDefault(c => c.Id == catalogId && c.IsActive);

            if (catalog == null)
                return Result.Failure<(Tenant, Catalog)>("Catalog not found or inactive");

            return (tenant, catalog).Success();
        }

        private async Task<Result<string>> UpdateCatalogValues(Tenant tenant, Catalog catalog, List<string> stringValues)
        {
            // Convertir strings a BsonValue
            var bsonValues = stringValues.Select(value => BsonValue.Create(value)).ToList();

            catalog.Values = bsonValues;

            await _mongoRepository.ReplaceOneAsync(tenant);

            return catalog.Id.Success(HttpStatusCode.OK);
        }
    }
}