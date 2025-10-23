using AgoraCertaminaBack.Models.DTOs.Catalog;
using AgoraCertaminaBack.Models.Entities;
using MongoDB.Bson;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class CatalogMappers
    {
        public static Catalog CustomerCatalogRequestToCustomerCatalog(this CreateCatalogRequest request)
        {
            return new Catalog
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Values = request.Values.Select(v => BsonValue.Create(v)).ToList(),
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }

        public static CatalogResponse ToCustomCatalogResponse(this Catalog catalog)
        {
            return new CatalogResponse
            {
                Id = catalog.Id,
                Name = catalog.Name,
                Values = catalog.Values?.Select(v => v.ToString() ?? string.Empty).ToList() ?? [], // [] en lugar de "new List<string>()"
                CreatedAt = catalog.CreatedAt,
                IsActive = catalog.IsActive
            };
        }
    }
}