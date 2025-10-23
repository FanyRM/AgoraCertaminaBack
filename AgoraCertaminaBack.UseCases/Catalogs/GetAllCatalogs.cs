using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Catalog;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using ROP;

namespace AgoraCertaminaBack.UseCases.Catalogs
{
    public class GetAllCatalogs(IMongoRepository<Customer> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<List<CatalogResponse>>> Execute()
        {
            var customer = await _mongoRepository.FindOneAsync(
                    filter => filter.Id == _userRequest.CustomerId);

            if (customer == null || customer.Catalogs == null)
            {
                return new List<CatalogResponse>().Success();
            }

            return customer.Catalogs
                .Where(catalog => catalog.IsActive)
                .Select(catalog => catalog.ToCustomCatalogResponse())
                .ToList()
                .Success();
        }
    }
}