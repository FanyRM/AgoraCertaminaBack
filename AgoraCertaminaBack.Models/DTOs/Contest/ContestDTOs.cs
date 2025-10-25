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
        public List<CustomTagDTO> Tags { get; set; } = [];
        public List<FieldDTO> Fields { get; set; } = [];
        public ContestStatusEnum? Status { get; set; }
        public DateTime? DeletedAt { get; set; }
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
}
