using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations
{
    public class KnowledgeContestTemplate : IContestTemplateFactory
    {
        private const string CATALOG_AREA_CONOCIMIENTO = "Área de conocimiento";

        public ContestTemplateData GetContestTemplateData()
        {
            var data = new ContestTemplateData();

            // Tags
            data.Tags.Add(new CustomTagRequest { Name = "Conocimiento", Color = "indigo", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Académico", Color = "blue", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Educativo", Color = "teal", Category = TagCategory.Contest });

            // Catalogs
            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_AREA_CONOCIMIENTO,
                Values = new List<string>
                {
                    "Matemáticas",
                    "Física",
                    "Química",
                    "Biología",
                    "Historia",
                    "Literatura",
                    "General"
                }
            });

            // Fields
            data.Fields.Add(new FieldRequest
            {
                Name = "Área evaluada",
                Type = FieldTypeEnum.CustomCatalog,
                CatalogId = "AREA_CONOCIMIENTO",
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Duración de la prueba",
                Type = FieldTypeEnum.String,
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Reglamento",
                Type = FieldTypeEnum.Archive,
                IsRequired = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Formato del examen",
                Type = FieldTypeEnum.String,
                IsRequired = true
            });

            return data;
        }

        public CategoriesEnum GetCategory() => CategoriesEnum.Knowledge;

        public string GetDescription() => "Concursos enfocados en conocimientos académicos.";
    }
}

