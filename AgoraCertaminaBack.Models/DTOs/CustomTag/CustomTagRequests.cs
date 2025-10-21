namespace AgoraCertaminaBack.Models.DTOs.CustomTag
{
    public class CustomTagRequest
    {
        public required string Name { get; set; }
        public required string Color { get; set; }
        public TagCategory Category { get; set; }
        public string? Subcategory { get; set; }
    }

    public class EditCustomTagRequest
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
    }
}
