using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations
{
    /// <summary>
    /// Plantilla para formularios de retroalimentación general
    /// </summary>
    public class FeedbackFormTemplate : IFormTemplateFactory
    {
        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            // Tags
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Feedback",
                Color = "#F59E0B",
                Category = TagCategory.Form
            });

            // Catálogos
            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Calificacion",
                Values = new List<string>
                {
                    "1 - Muy malo",
                    "2 - Malo",
                    "3 - Regular",
                    "4 - Bueno",
                    "5 - Excelente"
                }
            });

            // Campos
            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 0,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Email",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 1,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Calificacion General",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 2,
                StaticValue = "",
                CatalogId = "CALIFICACION_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Comentarios",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 3,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Fecha",
                Type = FieldTypeEnum.Date,
                IsRequired = true,
                Order = 4,
                StaticValue = ""
            });

            return data;
        }

        public FormTemplateCategory GetCategory()
        {
            return FormTemplateCategory.FeedbackForm;
        }

        public string GetDescription()
        {
            return "Formulario simple de retroalimentación con calificación y comentarios";
        }
    }
}