using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations
{
    /// <summary>
    /// Plantilla para solicitudes de empleo
    /// </summary>
    public class JobApplicationTemplate : IFormTemplateFactory
    {
        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            // Tags
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Reclutamiento",
                Color = "#6366F1",
                Category = TagCategory.Form
            });
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Vacante",
                Color = "#14B8A6",
                Category = TagCategory.Form
            });

            // Catálogos
            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Nivel de Estudios",
                Values = new List<string>
                {
                    "Secundaria",
                    "Preparatoria",
                    "Técnico",
                    "Licenciatura",
                    "Maestría",
                    "Doctorado"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Años de Experiencia",
                Values = new List<string>
                {
                    "Sin experiencia",
                    "Menos de 1 año",
                    "1-3 años",
                    "3-5 años",
                    "5-10 años",
                    "Más de 10 años"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Nivel de Inglés",
                Values = new List<string>
                {
                    "Básico",
                    "Intermedio",
                    "Avanzado",
                    "Nativo"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Disponibilidad",
                Values = new List<string>
                {
                    "Inmediata",
                    "1 semana",
                    "2 semanas",
                    "1 mes",
                    "Más de 1 mes"
                }
            });

            // Campos
            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre Completo",
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
                Name = "Teléfono",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Fecha de Nacimiento",
                Type = FieldTypeEnum.Date,
                IsRequired = true,
                Order = 3,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nivel de Estudios",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "NIVEL_ESTUDIOS_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Años de Experiencia",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 5,
                StaticValue = "",
                CatalogId = "AÑOS_EXPERIENCIA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nivel de Inglés",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 6,
                StaticValue = "",
                CatalogId = "NIVEL_INGLES_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Disponibilidad",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 7,
                StaticValue = "",
                CatalogId = "DISPONIBILIDAD_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Expectativa Salarial (Mensual)",
                Type = FieldTypeEnum.Decimal,
                IsRequired = true,
                Order = 8,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "CV/Currículum",
                Type = FieldTypeEnum.Archive,
                IsRequired = true,
                Order = 9,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Carta de Presentación",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 10,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "LinkedIn",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 11,
                StaticValue = ""
            });

            return data;
        }

        public FormTemplateCategory GetCategory()
        {
            return FormTemplateCategory.JobApplication;
        }

        public string GetDescription()
        {
            return "Formulario de solicitud de empleo con información profesional y académica";
        }
    }
}