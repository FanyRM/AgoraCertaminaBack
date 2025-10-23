using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using System.Net;
using AgoraCertaminaBack.Models.DTOs.CustomCatalog;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantCatalogs
{
    public class CreateTenantCatalog(IMongoRepository<Tenant> _mongoRepository, GetByIdTenant _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(CreateCustomCatalogRequest request)
        {
            return await _getById.Execute(_userRequest.OrganizationId)
                .Bind(tenant => ValidateUniqueName(tenant, request.Name))
                .Bind(tenant => AddCatalog(tenant, request));
        }

        private static Result<Tenant> ValidateUniqueName(Tenant tenant, string catalogName)
        {
            bool catalogExists = tenant.Catalogs.Any(c =>
                c.IsActive &&
                c.Name.Equals(catalogName, StringComparison.CurrentCultureIgnoreCase)
            );

            if (catalogExists)
                return Result.Failure<Tenant>("A catalog with this name already exists for this tenant");

            return tenant.Success();
        }

        public async Task<Result<string>> AddCatalog(Tenant tenant, CreateCustomCatalogRequest request)
        {
            var newCatalog = request.ToCustomCatalog();

            tenant.Catalogs.Add(newCatalog);

            await _mongoRepository.ReplaceOneAsync(tenant);

            return newCatalog.Id.Success(HttpStatusCode.Created);
        }
    }
}