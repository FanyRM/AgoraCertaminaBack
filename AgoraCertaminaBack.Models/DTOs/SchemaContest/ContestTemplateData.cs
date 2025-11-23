using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;

namespace AgoraCertaminaBack.Models.DTOs.SchemaContest
{
    public class ContestTemplateData
    {
        public List<CustomTagRequest> Tags { get; set; } = new();
        public List<CreateCustomCatalogRequest> Catalogs { get; set; } = new();
        public List<FieldRequest> Fields { get; set; } = new();
    }
}
