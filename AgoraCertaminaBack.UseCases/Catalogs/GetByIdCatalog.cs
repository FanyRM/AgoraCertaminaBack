using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Catalog;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using ROP;

namespace AgoraCertaminaBack.UseCases.Catalogs
{
    public class GetByIdCatalog(IMongoRepository<Customer> mongoRepository, UserRequestContext userRequest)
    {
        private readonly IMongoRepository<Customer> _mongoRepository = mongoRepository;
        private readonly UserRequestContext _userRequest = userRequest;

        public async Task<Result<CatalogResponse>> Execute(string catalogId)
        {
            var customer = await _mongoRepository.FindOneAsync(
                filter => filter.Id == _userRequest.CustomerId);

            if (customer == null || customer.Catalogs == null)
            {
                return Result.Failure<CatalogResponse>("Customer or catalogs not found");
            }

            var catalog = customer.Catalogs
                .FirstOrDefault(catalog => catalog.Id == catalogId && catalog.IsActive);

            if (catalog == null)
            {
                return Result.Failure<CatalogResponse>("Catalog not found or inactive");
            }

            return catalog.ToCustomCatalogResponse().Success();
        }
    }
}