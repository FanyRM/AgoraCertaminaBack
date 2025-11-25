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

            data.Tags.Add(new CustomTagRequest { Name = "Talento", Color = "rose", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Artístico", Color = "red", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Creatividad", Color = "purple", Category = TagCategory.Contest });

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

            data.Fields.Add(new FieldRequest
            {
                Name = "Tipo de talento",
                Type = FieldTypeEnum.CustomCatalog,
                CatalogId = "TIPO_DE_TALENTO_PLACEHOLDER",
                IsBase = false,
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Duración máxima de presentación",
                Type = FieldTypeEnum.String,
                IsBase = false,
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Requisitos de vestuario",
                Type = FieldTypeEnum.String,
                IsBase = false,
                IsRequired = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Pista o archivo musical",
                Type = FieldTypeEnum.Archive,
                IsBase = false,
                IsRequired = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Video demostrativo",
                Type = FieldTypeEnum.Archive,
                IsBase = false,
                IsRequired = false
            });

            return data;
        }

        public CategoriesEnum GetCategory() => CategoriesEnum.Talent;

        public string GetDescription() => "Concursos relacionados con habilidades artísticas y expresivas.";
    }
}

