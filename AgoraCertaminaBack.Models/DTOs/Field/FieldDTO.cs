using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.Models.DTOs.Field
{
    public class ContestFieldDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string CatalogId { get; set; }
        public FieldTypeEnum Type { get; set; }
        public bool IsBase { get; set; }
        public bool IsRequired { get; set; }
    }
}
