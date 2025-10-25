using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using MongoDB.Bson;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class FieldMappers
    {
        public static Field FieldTypeRequestToFieldType(this FieldRequest request, bool isBase)
        {
            var customField = new Field
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Type = request.Type,
                IsRequired = request.IsRequired,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsBase = isBase
            };

            // Asigna CatalogId solo si el tipo es CustomCatalog y se proporciona un catálogo
            if (request.Type == FieldTypeEnum.CustomCatalog && !string.IsNullOrWhiteSpace(request.CatalogId))
            {
                customField.CatalogId = request.CatalogId;
            }

            return customField;
        }
    }
}
