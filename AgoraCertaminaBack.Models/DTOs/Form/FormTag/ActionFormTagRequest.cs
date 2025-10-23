using AgoraCertaminaBack.Models.DTOs.CustomTag;

namespace AgoraCertaminaBack.Models.DTOs.Form.FormTag
{
    public class ActionFormTagRequest
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
        public TagCategory Category { get; set; }
        public string? Subcategory { get; set; } = string.Empty;
    }
}
