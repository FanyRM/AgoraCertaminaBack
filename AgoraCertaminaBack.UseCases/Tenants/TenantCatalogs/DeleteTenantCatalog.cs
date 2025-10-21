using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantCatalogs
{
    public class DeleteTenantCatalog(IMongoRepository<Tenant> mongoRepository, IMongoRepository<Form> _schemaAsset, GetByIdTenant _getByIdCustomer, UserRequestContext userRequest)
    {
        public async Task<Result<string>> Execute(string catalogId)
        {
            return await _getByIdCustomer.Execute(userRequest.OrganizationId)
                .Bind(tenant => FindCatalog(tenant, catalogId))
                .Bind(async tuple => await ValidateCatalogNotInUse(tuple.tenant, tuple.catalog))
                .Bind(tuple => DeleteCatalog(tuple.tenant, tuple.catalog));
        }

        private static Result<(Tenant tenant, Catalog catalog)> FindCatalog(Tenant tenant, string catalogId)
        {
            var catalog = tenant.Catalogs.FirstOrDefault(c => c.Id == catalogId && c.IsActive);
            if (catalog == null)
                return Result.Failure<(Tenant, Catalog)>("Catalog not found or already inactive");
            return (tenant, catalog).Success();
        }

        private async Task<Result<(Tenant tenant, Catalog catalog)>> ValidateCatalogNotInUse(Tenant tenant, Catalog catalog)
        {
            try
            {
                var isInUse = await IsCatalogInUse(catalog.Id, tenant.Id);

                if (isInUse)
                {
                    // TO DO: enviar el mensaje de error en inglés y español
                    //return Result.Failure<(Tenant, CustomerCatalog)>(
                    //    $"Cannot delete catalog '{catalog.Name}' because it is being used in one or more assets");

                    return Result.Failure<(Tenant, Catalog)>(
                        $"No se puede eliminar el catálogo '{catalog.Name}' porque esta siendo utilizado en uno o más activos");
                }

                return (tenant, catalog).Success();
            }
            catch (Exception)
            {
                return Result.Failure<(Tenant, Catalog)>("Error validating catalog usage");
            }
        }

        private async Task<bool> IsCatalogInUse(string catalogId, string customerId)
        {
            // Busca si existe algún esquema ACTIVO del cliente que tenga campos que referencien al catálogo
            var assetUsingCatalog = await _schemaAsset.FindOneAsync(
                a => a.OrganizationId == customerId &&
                     a.IsActive &&
                     a.FormFields.Any(f => f.CatalogId == catalogId && f.IsActive));

            return assetUsingCatalog != null;
        }

        private async Task<Result<string>> DeleteCatalog(Tenant tenant, Catalog catalog)
        {
            try
            {
                catalog.IsActive = false;

                await mongoRepository.ReplaceOneAsync(tenant);
                return catalog.Id.Success(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Result.Failure<string>("Error deleting catalog");
            }
        }
    }
}