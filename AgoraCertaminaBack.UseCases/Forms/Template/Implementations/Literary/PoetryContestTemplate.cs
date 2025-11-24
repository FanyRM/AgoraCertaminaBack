using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Literary
{
    public class PoetryContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "literary-poetry";

        public string GetName() => "Concurso de Poesía";

        public FormTemplateCategory GetCategory() => FormTemplateCategory.LiteraryContest;

        public string GetDescription() =>
            "Plantilla especializada para concursos de poesía con categorías de verso libre, soneto, haiku, etc.";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Poesía",
                Color = "red",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tipo de Poesía",
                Values = new List<string>
                {
                    "Verso Libre",
                    "Soneto",
                    "Haiku",
                    "Décima",
                    "Romance",
                    "Elegía"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Poeta",
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
                Name = "Título del Poema",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tipo de Poesía",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "TIPO_DE_POESÍA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Poema Completo",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 4,
                StaticValue = ""
            });

            return data;
        }
    }
}
