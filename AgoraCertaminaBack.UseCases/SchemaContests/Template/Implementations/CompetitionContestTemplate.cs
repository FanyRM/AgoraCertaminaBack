using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations
{
    public class CompetitionContestTemplate : IContestTemplateFactory
    {
        private const string CATALOG_DISCIPLINA = "Disciplina deportiva";

        public ContestTemplateData GetContestTemplateData()
        {
            var data = new ContestTemplateData();

            data.Tags.Add(new CustomTagRequest { Name = "Competencia", Color = "red", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Deporte", Color = "orange", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Resistencia", Color = "yellow", Category = TagCategory.Contest });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_DISCIPLINA,
                Values = new List<string>
                {
                    "Atletismo",
                    "Natación",
                    "Fútbol",
                    "Voleibol",
                    "Basquetbol",
                    "Ciclismo",
                    "Artes marciales"
                }
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Disciplina",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                IsBase = false,
                CatalogId = "DISCIPLINA_DEPORTIVA"
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Reglamento deportivo",
                Type = FieldTypeEnum.Archive,
                IsBase = false,
                IsRequired = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Duración del encuentro",
                Type = FieldTypeEnum.String,
                IsBase = false,
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Requisitos de seguridad",
                Type = FieldTypeEnum.String,
                IsBase = false,
                IsRequired = false
            });

            return data;
        }

        public CategoriesEnum GetCategory() => CategoriesEnum.Competition;

        public string GetDescription() => "Competencias físicas, deportivas o de habilidad.";
    }
}

