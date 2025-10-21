using AgoraCertaminaBack.Models.Entities;
using MongoDB.Bson;
using AgoraCertaminaBack.Models.DTOs.CustomCatalog;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class CustomCatalogMappers
    {
        public static Catalog ToCustomCatalog(this CreateCustomCatalogRequest request)
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

        public static CustomCatalogDTO ToCustomCatalogDTO(this Catalog catalog)
        {
            return new CustomCatalogDTO
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