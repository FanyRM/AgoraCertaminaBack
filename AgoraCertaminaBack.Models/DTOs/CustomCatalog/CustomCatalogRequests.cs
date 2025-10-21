namespace AgoraCertaminaBack.Models.DTOs.CustomCatalog
{

    public class CreateCustomCatalogRequest
    {
        public required string Name { get; set; }
        public required List<string> Values { get; set; }
    }

    public class UpdateCustomCatalogValuesRequest
    {
        public required List<string> Values { get; set; } = [];
    }

    public class CustomCatalogDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<string> Values { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
