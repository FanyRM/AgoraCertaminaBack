
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using MongoDB.Bson;
using AgoraCertaminaBack.Models.DTOs.CustomField;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class CustomFieldMappers
    {
        public static CustomField ToCustomField(this CustomFieldRequest request)
        {
            var customField = new CustomField
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Type = request.Type,
                IsRequired = request.IsRequired,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Section = string.IsNullOrWhiteSpace(request.Section) ? null : request.Section,
                SubSection = string.IsNullOrWhiteSpace(request.SubSection) ? null : request.SubSection,
                Order = request.Order,
                StaticValue =request.StaticValue
               
            };

            // Asigna CatalogId solo si el tipo es CustomCatalog y se proporciona un catálogo
            if (request.Type == FieldTypeEnum.CustomCatalog && !string.IsNullOrWhiteSpace(request.CatalogId))
                customField.CatalogId = request.CatalogId;

            return customField;
        }

        public static CustomFieldDTO ToCustomFieldDTO(this CustomField field)
        {
            return new CustomFieldDTO
            {
                Id = field.Id,
                Name = field.Name,
                CatalogId = field.CatalogId ?? string.Empty,
                Section = field.Section ?? string.Empty,
                SubSection = field.SubSection ?? string.Empty,
                Order = field.Order,
                Type = field.Type,
                IsRequired = field.IsRequired,
                StaticValue =field.StaticValue 
            };
        }
    }
}
