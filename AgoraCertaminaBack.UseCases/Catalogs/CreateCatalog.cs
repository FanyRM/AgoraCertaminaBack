using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.DTOs.Catalog;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.UseCases.Customers;
using ROP;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Catalogs
{
    public class CreateCatalog(IMongoRepository<Customer> _mongoRepository, GetByIdCustomer _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(CreateCatalogRequest request)
        {
            var result = await _getById.Execute(_userRequest.CustomerId)
                .Bind(customer => ValidateUniqueCatalogName(customer, request.Name))
                .Bind(customer => AddCustomerCatalog(customer, request));

            return result;
        }

        private static Result<Customer> ValidateUniqueCatalogName(Customer customer, string catalogName)
        {
            bool catalogExists = customer.Catalogs.Any(c =>
                c.IsActive &&
                c.Name.Equals(catalogName, StringComparison.CurrentCultureIgnoreCase)
            );

            if (catalogExists)
                return Result.Failure<Customer>("A catalog with this name already exists for this customer");

            return customer.Success();
        }

        public async Task<Result<string>> AddCustomerCatalog(Customer customer, CreateCatalogRequest request)
        {
            var newCatalog = request.CustomerCatalogRequestToCustomerCatalog();

            customer.Catalogs.Add(newCatalog);

            await _mongoRepository.ReplaceOneAsync(customer);

            return newCatalog.Id.Success(HttpStatusCode.Created);
        }
    }
}