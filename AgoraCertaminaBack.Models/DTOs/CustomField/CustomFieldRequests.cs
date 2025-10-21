using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.Models.DTOs.CustomField
{
    public class CustomFieldRequest
    {
        public required string Name { get; set; }
        public FieldTypeEnum Type { get; set; }
        public bool IsRequired { get; set; }
        public string? CatalogId { get; set; }
        public string? Section { get; set; }
        public string? SubSection { get; set; }
        public required int Order { get; set; }
        public string StaticValue { get; set; } = string.Empty;

    }

    public class ReorderFormFieldsRequest
    {
        public required string Id { get; set; }
        public required int Order { get; set; }
    }

}
