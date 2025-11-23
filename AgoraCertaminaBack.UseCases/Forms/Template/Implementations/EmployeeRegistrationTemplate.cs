using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations
{
    /// <summary>
    /// Plantilla para registro de empleados con campos de información personal y laboral
    /// </summary>
    public class EmployeeRegistrationTemplate : IFormTemplateFactory
    {
        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            // Tags predefinidos
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Recursos Humanos",
                Color = "#3B82F6",
                Category = TagCategory.Form
            });
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Registro",
                Color = "#10B981",
                Category = TagCategory.Form
            });

            // Catálogos predefinidos
            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Departamentos",
                Values = new List<string>
                {
                    "Recursos Humanos",
                    "Tecnología",
                    "Ventas",
                    "Marketing",
                    "Finanzas",
                    "Operaciones",
                    "Legal"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tipo de Contrato",
                Values = new List<string>
                {
                    "Tiempo Completo",
                    "Medio Tiempo",
                    "Por Proyecto",
                    "Temporal",
                    "Prácticas"
                }
            });

            // Campos predefinidos (se agregarán los CatalogId después de crear los catálogos)
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
                Name = "Email Corporativo",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 1,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Fecha de Nacimiento",
                Type = FieldTypeEnum.Date,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Departamento",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "DEPARTAMENTOS_PLACEHOLDER" // Se reemplazará con el ID real
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tipo de Contrato",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "TIPO_CONTRATO_PLACEHOLDER" // Se reemplazará con el ID real
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Fecha de Inicio",
                Type = FieldTypeEnum.Date,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Salario Mensual",
                Type = FieldTypeEnum.Decimal,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Teléfono",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 7,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Dirección",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 8,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Fotografía",
                Type = FieldTypeEnum.Image,
                IsRequired = false,
                Order = 9,
                StaticValue = ""
            });

            return data;
        }

        public FormTemplateCategory GetCategory()
        {
            return FormTemplateCategory.EmployeeRegistration;
        }

        public string GetDescription()
        {
            return "Formulario de registro de empleados con información personal y laboral";
        }
    }
}