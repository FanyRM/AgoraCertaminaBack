using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Catalog;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Customers;
using MongoDB.Bson;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.CustomCatalogs
{
    public class UpdateCatalogValues(IMongoRepository<Customer> _mongoRepository, GetByIdCustomer _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(string catalogId, UpdateCatalogValuesRequest request)
        {
            return await _getById.Execute(_userRequest.CustomerId)
                .Bind(customer => FindCatalog(customer, catalogId))
                .Bind(tuple => UpdateCatalogValuesExecute(tuple.Customer, tuple.Catalog, request.Values));
        }

        private static Result<(Customer Customer, Catalog Catalog)> FindCatalog(Customer customer, string catalogId)
        {
            var catalog = customer.Catalogs.FirstOrDefault(c => c.Id == catalogId && c.IsActive);

            if (catalog == null)
                return Result.Failure<(Customer, Catalog)>("Catalog not found or inactive");

            return (customer, catalog).Success();
        }

        private async Task<Result<string>> UpdateCatalogValuesExecute(Customer customer, Catalog catalog, List<string> stringValues)
        {
            // Convertir strings a BsonValue
            var bsonValues = stringValues.Select(value => BsonValue.Create(value)).ToList();

            catalog.Values = bsonValues;

            await _mongoRepository.ReplaceOneAsync(customer);

            return catalog.Id.Success(HttpStatusCode.OK);
        }
    }
}