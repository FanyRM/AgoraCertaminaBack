using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.Models.DTOs.Field
{
    public class FieldRequest
    {
        public required string Name { get; set; }
        public FieldTypeEnum Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsBase { get; set; }
        public string? CatalogId { get; set; }
    }

    public class UpdateFieldRequest
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public FieldTypeEnum Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
