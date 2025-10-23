using AgoraCertaminaBack.Models.DTOs.Catalog;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class SchemaMappers
    {
        public static SchemaContestDTOs ToSchemaContestDTO(this SchemaContest schema)
        {
            return new SchemaContestDTOs
            {
                Id = schema.Id,
                SchemaName = schema.SchemaName,
                CreatedAt = schema.CreatedAt,
                CountSchemaFields = schema.SchemaFields?.Count(field => field.IsActive) ?? 0,
                Tags = schema.Tags?
                    .Where(tag => tag.IsActive)
                    .Select(tag => tag.ToCustomTagDTO())
                    .ToList() ?? [],
                SchemaFields = schema.SchemaFields?
                    .Where(field => field.IsActive)
                    .Select(field => field.ToCustomFieldDTO())
                    .ToList() ?? []
            };
        }

        public static CustomFieldDTO ToCustomFieldDTO(this Field field)
        {
            return new CustomFieldDTO
            {
                Id = field.Id,
                Name = field.Name,
                CatalogId = field.CatalogId ?? "",
                Type = field.Type,
                IsBase = field.IsBase,
                IsRequired = field.IsRequired
            };
        }

        public static DataSchemaDTO ToDataSchemaDTO(
            this SchemaContest schema,
            List<CatalogReferenceDTO> catalogs)
        {
            return schema.ToDataSchemaDTO(
                schema.SchemaFields?.Where(field => field.IsActive).ToList() ?? new List<Field>(),
                catalogs);
        }

        public static DataSchemaDTO ToDataSchemaDTO(
            this SchemaContest schema,
            List<Field> allFields,
            List<CatalogReferenceDTO> catalogs)
        {
            return new DataSchemaDTO
            {
                Id = schema.Id,
                SchemaName = schema.SchemaName,
                CustomerId = schema.CustomerId,
                Fields = allFields
                    .Select(field => field.ToSchemaFieldDetailDTO(catalogs))
                    .ToList(),
                Catalogs = catalogs
            };
        }

        public static SchemaFieldDetailDTO ToSchemaFieldDetailDTO(
            this Field field,
            List<CatalogReferenceDTO> catalogs)
        {
            var catalog = !string.IsNullOrEmpty(field.CatalogId)
                ? catalogs.FirstOrDefault(c => c.CatalogId == field.CatalogId)
                : null;

            return new SchemaFieldDetailDTO
            {
                Id = field.Id,
                Name = field.Name,
                Type = field.Type,
                IsRequired = field.IsRequired,
                IsBase = field.IsBase,
                CatalogName = catalog?.CatalogName
            };
        }

        public static CatalogReferenceDTO ToCatalogReferenceDTO(this CatalogResponse catalog)
        {
            return new CatalogReferenceDTO
            {
                CatalogId = catalog.Id,
                CatalogName = catalog.Name,
                Values = catalog.Values ?? new List<string>()
            };
        }
    }
}
