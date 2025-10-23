using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.Models.DTOs.CustomField
{
    public class CustomFieldDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required int Order { get; set; }
        public required string CatalogId { get; set; }
        public required string Section { get; set; }
        public required string SubSection { get; set; }
        public FieldTypeEnum Type { get; set; }
        public bool IsRequired { get; set; }
        public string StaticValue { get; set; } = string.Empty; 
    }
}
