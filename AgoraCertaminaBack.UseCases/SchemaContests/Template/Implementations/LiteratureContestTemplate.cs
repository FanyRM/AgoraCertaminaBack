using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations
{
    public class LiteratureContestTemplate : IContestTemplateFactory
    {
        private const string CATALOG_GENERO_LITERARIO = "Género literario";
        private const string CATALOG_SUBGENERO_LITERARIO = "Subgénero literario";
        private const string CATALOG_FORMATO_FILE = "Formato de archivo";
        public ContestTemplateData GetContestTemplateData()
        {
            var data = new ContestTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Literatura",
                Color = "blue",
                Category = TagCategory.Contest
            });
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Escritura",
                Color = "rose",
                Category = TagCategory.Contest
            });
            data.Tags.Add(new CustomTagRequest
            {
                Name = "Creatividad",
                Color = "teal",
                Category = TagCategory.Contest
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_GENERO_LITERARIO,
                Values = new List<string>
                {
                    "Narrativo",
                    "Lírico",
                    "Dramático",
                    "Didáctico",
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_SUBGENERO_LITERARIO,
                Values = new List<string>
                {
                    "Novela",
                    "Cuento",
                    "Fábula",
                    "Leyenda",
                    "Epopeya",
                    "Poema",
                    "Guión",
                    "Oda",
                    "Sátira",
                    "Soneto",
                    "Trágedia",
                    "Ensayo",
                    "Drama",
                    "Biografía",
                    "Carta",
                    "Discurso",
                    "Diálogo",
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_FORMATO_FILE,
                Values = new List<string>
                {
                    "PDF",
                    "Word",
                    "Archivo .rar",
                    "Físico",
                }
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Género literario",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                IsBase = false,
                CatalogId = "GÉNERO_LITERARIO_PLACEHOLDER"
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Subgénero literario",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                IsBase = false,
                CatalogId = "SUBGÉNERO_LITERARIO_PLACEHOLDER"
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Edad Mínima de participación",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                IsBase = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Edad Máxima de participación",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                IsBase = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Extensión máxima del texto",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                IsBase = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Tipo de entregable",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                IsBase = false,
                CatalogId = "FORMATO_DE_ARCHIVO_PLACEHOLDER"
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Tema de texto",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                IsBase = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Premio y recompensas",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                IsBase = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Lineamientos",
                Type = FieldTypeEnum.Archive,
                IsRequired = false,
                IsBase = false,
            });

            return data;
        }

        public CategoriesEnum GetCategory()
        {
            return CategoriesEnum.Literature;
        }

        public string GetDescription()
        {
            return "Concursos relacionados a la escritura de textos inéditos";
        }
    }
}
