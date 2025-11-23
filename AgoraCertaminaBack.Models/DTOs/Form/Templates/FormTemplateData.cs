using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;

namespace AgoraCertaminaBack.Models.DTOs.Form.Templates
{
    public class FormTemplateData
    {
        public List<CustomTagRequest> Tags { get; set; } = new();
        public List<CreateCustomCatalogRequest> Catalogs { get; set; } = new();
        public List<CustomFieldRequest> Fields { get; set; } = new();
    }

    public class CustomTagRequest
    {
        public required string Name { get; set; }
        public required string Color { get; set; }
        public TagCategory Category { get; set; }
    }
}