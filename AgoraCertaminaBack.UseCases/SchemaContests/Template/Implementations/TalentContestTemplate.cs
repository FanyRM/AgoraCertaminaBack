using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations
{
    public class TalentContestTemplate : IContestTemplateFactory
    {
        private const string CATALOG_TIPO_TALENTO = "Tipo de talento";

        public ContestTemplateData GetContestTemplateData()
        {
            var data = new ContestTemplateData();

            // Tags
            data.Tags.Add(new CustomTagRequest { Name = "Talento", Color = "rose", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Artístico", Color = "red", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Creatividad", Color = "purple", Category = TagCategory.Contest });

            // Catalogs
            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_TIPO_TALENTO,
                Values = new List<string>
                {
                    "Canto",
                    "Baile",
                    "Actuación",
                    "Stand-up",
                    "Instrumento musical",
                    "Arte dramático"
                }
            });

            // Fields
            data.Fields.Add(new FieldRequest
            {
                Name = "Tipo de talento",
                Type = FieldTypeEnum.CustomCatalog,
                CatalogId = "TIPO_TALENTO",
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Duración máxima de presentación",
                Type = FieldTypeEnum.String,
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Requisitos de vestuario",
                Type = FieldTypeEnum.String,
                IsRequired = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Pista o archivo musical",
                Type = FieldTypeEnum.Archive,
                IsRequired = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Video demostrativo",
                Type = FieldTypeEnum.Archive,
                IsRequired = false
            });

            return data;
        }

        public CategoriesEnum GetCategory() => CategoriesEnum.Talent;

        public string GetDescription() => "Concursos relacionados con habilidades artísticas y expresivas.";
    }
}

