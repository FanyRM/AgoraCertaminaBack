using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations
{
    public class TechnologyContestTemplate : IContestTemplateFactory
    {
        private const string CATALOG_TIPO_PROYECTO = "Tipo de proyecto tecnológico";
        private const string CATALOG_TIPO_ENTREGA = "Formato de entregable tecnológico";

        public ContestTemplateData GetContestTemplateData()
        {
            var data = new ContestTemplateData();

            data.Tags.Add(new CustomTagRequest { Name = "Tecnología", Color = "teal", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Innovación", Color = "purple", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Prototipos", Color = "rose", Category = TagCategory.Contest });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_TIPO_PROYECTO,
                Values = new List<string>
                {
                    "Software",
                    "Hardware",
                    "Aplicación móvil",
                    "Inteligencia Artificial",
                    "IoT",
                    "Prototipo funcional"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_TIPO_ENTREGA,
                Values = new List<string>
                {
                    "Repositorio GitHub",
                    "Archivo .zip",
                    "Video demostrativo",
                    "Documento técnico PDF"
                }
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Tipo de proyecto",
                Type = FieldTypeEnum.CustomCatalog,
                CatalogId = "TIPO_DE_PROYECTO_TECNOLÓGICO_PLACEHOLDER",
                IsRequired = true,
                IsBase = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Duración del proyecto (meses)",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                IsBase = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Formato de entrega",
                Type = FieldTypeEnum.CustomCatalog,
                CatalogId = "FORMATO_DE_ENTREGABLE_TECNOLÓGICO_PLACEHOLDER",
                IsBase = false,
                IsRequired = true

            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Requerimientos técnicos",
                Type = FieldTypeEnum.String,
                IsBase = false,
                IsRequired = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Documentación técnica necesaria",
                Type = FieldTypeEnum.Archive,
                IsBase = false,
                IsRequired = false
            });

            return data;
        }

        public CategoriesEnum GetCategory() => CategoriesEnum.Technology;

        public string GetDescription() => "Concursos orientados a proyectos tecnológicos, software, hardware y prototipos.";
    }
}

