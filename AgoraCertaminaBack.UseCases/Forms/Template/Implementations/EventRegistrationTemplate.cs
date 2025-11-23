using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations
{
    /// <summary>
    /// Plantilla para registro de asistentes a eventos
    /// </summary>
    public class EventRegistrationTemplate : IFormTemplateFactory
    {
        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            // Tags
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Evento",
                Color = "#EC4899",
                Category = TagCategory.Form
            });
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Registro",
                Color = "#10B981",
                Category = TagCategory.Form
            });

            // Catálogos
            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tipo de Asistente",
                Values = new List<string>
                {
                    "Participante",
                    "Ponente",
                    "Patrocinador",
                    "Staff",
                    "Prensa"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Talla de Playera",
                Values = new List<string>
                {
                    "XS",
                    "S",
                    "M",
                    "L",
                    "XL",
                    "XXL"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Preferencia Alimenticia",
                Values = new List<string>
                {
                    "Sin restricciones",
                    "Vegetariano",
                    "Vegano",
                    "Sin gluten",
                    "Sin lactosa"
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
                Name = "Organización/Empresa",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 3,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tipo de Asistente",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "TIPO_ASISTENTE_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Talla de Playera",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 5,
                StaticValue = "",
                CatalogId = "TALLA_PLAYERA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Preferencia Alimenticia",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 6,
                StaticValue = "",
                CatalogId = "PREFERENCIA_ALIMENTICIA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "¿Requiere factura?",
                Type = FieldTypeEnum.Boolean,
                IsRequired = true,
                Order = 7,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Comentarios especiales",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 8,
                StaticValue = ""
            });

            return data;
        }

        public FormTemplateCategory GetCategory()
        {
            return FormTemplateCategory.EventRegistration;
        }

        public string GetDescription()
        {
            return "Formulario de registro para asistentes a eventos con información logística";
        }
    }
}