using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Catalog;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Customers;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Catalogs
{
    public class DeleteCatalog(IMongoRepository<Customer> mongoRepository, IMongoRepository<SchemaContest> _schemaAsset, GetByIdCustomer _getByIdCustomer, UserRequestContext userRequest )
    {
        public async Task<Result<string>> Execute(string catalogId)
        {
            return await _getByIdCustomer.Execute(userRequest.CustomerId)
                .Bind(customer => FindCatalog(customer, catalogId))
                .Bind(async tuple => await ValidateCatalogNotInUse(tuple.customer, tuple.catalog))
                .Bind(tuple => DeleteCatalogExecute(tuple.customer, tuple.catalog));
        }

        private static Result<(Customer customer, Catalog catalog)> FindCatalog(Customer customer, string catalogId)
        {
            var catalog = customer.Catalogs.FirstOrDefault(c => c.Id == catalogId && c.IsActive);
            if (catalog == null)
                return Result.Failure<(Customer, Catalog)>("Catalog not found or already inactive");
            return (customer, catalog).Success();
        }

        private async Task<Result<(Customer customer, Catalog catalog)>> ValidateCatalogNotInUse(Customer customer, Catalog catalog)
        {
            try
            {
                var isInUse = await IsCatalogInUse(catalog.Id, customer.Id);

                if (isInUse)
                {
                    // TO DO: enviar el mensaje de error en inglés y español
                    //return Result.Failure<(Customer, CustomerCatalog)>(
                    //    $"Cannot delete catalog '{catalog.Name}' because it is being used in one or more assets");

                    return Result.Failure<(Customer, Catalog)>(
                        $"No se puede eliminar el catálogo '{catalog.Name}' porque esta siendo utilizado en uno o más activos");
                }

                return (customer, catalog).Success();
            }
            catch (Exception)
            {
                return Result.Failure<(Customer, Catalog)>("Error validating catalog usage");
            }
        }

        private async Task<bool> IsCatalogInUse(string catalogId, string customerId)
        {
            // Busca si existe algún esquema ACTIVO del cliente que tenga campos que referencien al catálogo
            var assetUsingCatalog = await _schemaAsset.FindOneAsync(
                a => a.CustomerId == customerId &&
                     a.IsActive &&
                     a.SchemaFields.Any(f => f.CatalogId == catalogId && f.IsActive));

            return assetUsingCatalog != null;
        }

        private async Task<Result<string>> DeleteCatalogExecute(Customer customer, Catalog catalog)
        {
            try
            {
                catalog.IsActive = false;

                await mongoRepository.ReplaceOneAsync(customer);
                return catalog.Id.Success(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Result.Failure<string>("Error deleting catalog");
            }
        }
    }
}