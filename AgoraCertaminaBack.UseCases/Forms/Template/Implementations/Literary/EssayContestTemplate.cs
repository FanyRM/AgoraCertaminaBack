using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Forms.Templates;

namespace AgoraCertaminaBack.UseCases.Forms.Template.Implementations.Literary
{
    public class EssayContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "literary-essay";
        public string GetName() => "Concurso de Ensayo";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.LiteraryContest;
        public string GetDescription() => "Plantilla para concursos de ensayo académico o literario";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Ensayo",
                Color = "blue",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tipo de Ensayo",
                Values = new List<string>
                {
                    "Argumentativo",
                    "Expositivo",
                    "Narrativo",
                    "Descriptivo",
                    "Crítico",
                    "Científico"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Nivel Educativo",
                Values = new List<string>
                {
                    "Secundaria",
                    "Preparatoria",
                    "Universidad",
                    "Posgrado",
                    "Profesional"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Autor",
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
                Name = "Nivel Educativo",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 2,
                StaticValue = "",
                CatalogId = "NIVEL_EDUCATIVO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Título del Ensayo",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 3,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tipo de Ensayo",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "TIPO_DE_ENSAYO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tema Principal",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Contenido del Ensayo",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Número de Palabras",
                Type = FieldTypeEnum.Integer,
                IsRequired = true,
                Order = 7,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Referencias Bibliográficas",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 8,
                StaticValue = ""
            });

            return data;
        }
    }

    public class MicrostoryContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "literary-microstory";
        public string GetName() => "Concurso de Microrrelato";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.LiteraryContest;
        public string GetDescription() => "Plantilla para microrrelatos (máximo 500 palabras)";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Microrrelato",
                Color = "purple",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Género Literario",
                Values = new List<string>
                {
                    "Terror",
                    "Ciencia Ficción",
                    "Fantasía",
                    "Romance",
                    "Misterio",
                    "Realismo",
                    "Humor"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Límite de Palabras",
                Values = new List<string>
                {
                    "100 palabras",
                    "250 palabras",
                    "500 palabras"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Autor",
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
                Name = "Título del Microrrelato",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Género Literario",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "GÉNERO_LITERARIO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Límite de Palabras",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "LÍMITE_DE_PALABRAS_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Microrrelato Completo",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Número de Palabras",
                Type = FieldTypeEnum.Integer,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }
}