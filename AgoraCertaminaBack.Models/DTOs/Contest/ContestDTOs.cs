using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Tag;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.Models.DTOs.Contest
{
    public class ContestDTO
    {
        public required string Id { get; set; }
        public required string ReferenceNumber { get; set; }
        public required string SchemaId { get; set; }
        public required string SchemaName { get; set; }
        public required string ImageUrl { get; set; }
        public required string ContestName { get; set; }
        public required string DescriptionContest { get; set; }
        public required string OrganizationName { get; set; }
        public required string FormId { get; set; }
        public required bool IsPay { get; set; }
        public double? Price { get; set; }
        public List<CustomTagDTO> Tags { get; set; } = [];
        public List<FieldDTO> Fields { get; set; } = [];
        public ContestStatusEnum? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public class FieldDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public string? CatalogId { get; set; }
        public FieldTypeEnum Type { get; set; }
        public bool IsRequired { get; set; }
        public required bool IsBase { get; set; }
    }

    public class ContestResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string OrganizationId { get; set; } = null!;
        public string SchemaId { get; set; } = null!;
        public string FormId { get; set; } = null!;
        public string ReferenceNumber { get; set; } = null!;
        public string OrganizationName { get; set; } = null!;
        public string SchemaName { get; set; } = null!;
        public string ContestName { get; set; } = null!;
        public string DescriptionContest { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CustomTagDTO> Tags { get; set; } = new List<CustomTagDTO>();
        public List<FieldValueDTO> Fields { get; set; } = new List<FieldValueDTO>();
        public bool IsEvalued { get; set; } = false;
        public bool IsSuspended { get; set; } = false;
        public bool IsPay { get; set; } = false;
        public double? Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }

    public class FieldValueDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FieldName { get; set; } = null!;
        public object? Value { get; set; }
        public FieldTypeEnum Type { get; set; }
        public string? CatalogId { get; set; }
        public bool IsBase { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
