using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations
{
    public class ProjectsContestTemplate : IContestTemplateFactory
    {
        private const string CATALOG_TIPO_PROYECTO = "Tipo de proyecto";

        public ContestTemplateData GetContestTemplateData()
        {
            var data = new ContestTemplateData();

            data.Tags.Add(new CustomTagRequest { Name = "Proyectos", Color = "green", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Innovación", Color = "teal", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Desarrollo", Color = "blue", Category = TagCategory.Contest });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_TIPO_PROYECTO,
                Values = new List<string>
                {
                    "Investigación",
                    "Proyecto escolar",
                    "Proyecto universitario",
                    "Prototipo",
                    "Emprendimiento"
                }
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Tipo de proyecto",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                IsBase = false,
                CatalogId = "TIPO_PROYECTO"
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Duración estimada del proyecto",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                IsBase = false,
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Objetivo general del proyecto",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                IsBase = false,
            });

            return data;
        }

        public CategoriesEnum GetCategory() => CategoriesEnum.Projects;

        public string GetDescription() => "Concursos orientados a proyectos escolares, de investigación o emprendimiento.";
    }
}
