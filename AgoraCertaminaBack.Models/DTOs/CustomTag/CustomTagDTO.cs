namespace AgoraCertaminaBack.Models.DTOs.CustomTag
{
    public class CustomTagDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public TagCategory Category { get; set; }
        public string? Subcategory { get; set; } = string.Empty;
    }
}
