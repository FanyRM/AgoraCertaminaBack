using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations
{
    /// <summary>
    /// Plantilla para encuestas de satisfacción de clientes
    /// </summary>
    public class CustomerSurveyTemplate : IFormTemplateFactory
    {
        private const string CATALOG_NIVEL_SATISFACCION = "Nivel de Satisfacción";
        private const string CATALOG_FRECUENCIA_USO = "Frecuencia de Uso";
        private const string CATALOG_RECOMENDARIA = "¿Recomendaría el Producto?";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            // Tags
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Encuesta",
                Color = "#8B5CF6",
                Category = TagCategory.Form
            });
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Satisfacción",
                Color = "#F59E0B",
                Category = TagCategory.Form
            });

            // Catálogos
            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_NIVEL_SATISFACCION,
                Values = new List<string>
                {
                    "Muy Insatisfecho",
                    "Insatisfecho",
                    "Neutral",
                    "Satisfecho",
                    "Muy Satisfecho"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_FRECUENCIA_USO,
                Values = new List<string>
                {
                    "Primera vez",
                    "Ocasionalmente",
                    "Mensualmente",
                    "Semanalmente",
                    "Diariamente"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_RECOMENDARIA,
                Values = new List<string>
                {
                    "Definitivamente no",
                    "Probablemente no",
                    "Tal vez",
                    "Probablemente sí",
                    "Definitivamente sí"
                }
            });

            // Campos
            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 0,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Email",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 1,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Frecuencia de Uso",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 2,
                StaticValue = "",
                CatalogId = "FRECUENCIA_DE_USO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Satisfacción General",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "NIVEL_DE_SATISFACCIÓN_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Satisfacción con el Servicio",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "NIVEL_DE_SATISFACCIÓN_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "¿Recomendaría nuestro producto/servicio?",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 5,
                StaticValue = "",
                CatalogId = "¿RECOMENDARÍA_EL_PRODUCTO?_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "¿Qué es lo que más te gusta?",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 6,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "¿Qué podríamos mejorar?",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 7,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Comentarios adicionales",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 8,
                StaticValue = ""
            });

            return data;
        }

        public FormTemplateCategory GetCategory()
        {
            return FormTemplateCategory.CustomerSurvey;
        }

        public string GetDescription()
        {
            return "Encuesta de satisfacción de clientes con evaluación de producto y servicio";
        }
    }
}