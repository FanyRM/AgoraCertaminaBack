using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Forms.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.UseCases.Forms.Template.Implementations.Literary
{
    public class ShortStoryContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "literary-short-story";

        public string GetName() => "Concurso de Cuento Corto";

        public FormTemplateCategory GetCategory() => FormTemplateCategory.LiteraryContest;

        public string GetDescription() =>
            "Plantilla para concursos de narrativa corta con límite de palabras y género literario";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Narrativa",
                Color = "blue",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Género Narrativo",
                Values = new List<string>
                {
                    "Realismo",
                    "Ciencia Ficción",
                    "Fantasía",
                    "Terror",
                    "Romance"
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
                Name = "Título del Cuento",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 1,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Género Narrativo",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 2,
                StaticValue = "",
                CatalogId = "GÉNERO_NARRATIVO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Archivo del Cuento",
                Type = FieldTypeEnum.Archive,
                IsRequired = true,
                Order = 3,
                StaticValue = ""
            });

            return data;
        }
    }
}
