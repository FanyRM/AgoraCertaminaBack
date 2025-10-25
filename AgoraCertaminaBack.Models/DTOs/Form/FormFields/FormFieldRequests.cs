using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.Models.DTOs.Form.FormFields
{
    public class UpdateFormFieldRequest
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required FieldTypeEnum Type { get; set; }
        public required bool IsRequired { get; set; }
        public string StaticValue { get; set; } = string.Empty;
    }
}
