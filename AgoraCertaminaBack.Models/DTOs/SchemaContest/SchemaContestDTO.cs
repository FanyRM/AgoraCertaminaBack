using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.Models.DTOs.SchemaContest
{
    public class SchemaTagDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    public class SchemaContestDTOs
    {
        public required string Id { get; set; }
        public required string SchemaName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CountSchemaFields { get; set; }
        public List<SchemaTagDTO> Tags { get; set; } = [];
        public List<CustomFieldDTO> SchemaFields { get; set; } = [];
    }

    public class DataSchemaDTO
    {
        public required string Id { get; set; }
        public required string SchemaName { get; set; }
        public required string CustomerId { get; set; }
        public List<SchemaFieldDetailDTO> Fields { get; set; } = [];
        public List<CatalogReferenceDTO> Catalogs { get; set; } = [];
    }

    public class SchemaFieldDetailDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public FieldTypeEnum Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsBase { get; set; }
        public string? CatalogName { get; set; }
    }

    public class CatalogReferenceDTO
    {
        public required string CatalogId { get; set; }
        public required string CatalogName { get; set; }
        public required List<string> Values { get; set; }
    }

    public class SchemaTagRequest
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
        public string? Subcategory { get; set; } = string.Empty;
    }

    public class UpdateSchemaFieldRequest
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required FieldTypeEnum Type { get; set; }
        public required bool IsRequired { get; set; }
        public required bool IsBase { get; set; }
    }
}
