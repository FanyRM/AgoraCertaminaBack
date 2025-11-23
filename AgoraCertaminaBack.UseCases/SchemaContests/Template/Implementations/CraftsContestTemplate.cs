using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations
{
    public class CraftsContestTemplate : IContestTemplateFactory
    {
        private const string CATALOG_MATERIAL = "Material permitido";
        private const string CATALOG_TIPO_ENTREGA = "Tipo de entrega";

        public ContestTemplateData GetContestTemplateData()
        {
            var data = new ContestTemplateData();

            data.Tags.Add(new CustomTagRequest { Name = "Artesanías", Color = "amber", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Creatividad manual", Color = "emerald", Category = TagCategory.Contest });
            data.Tags.Add(new CustomTagRequest { Name = "Diseño", Color = "violet", Category = TagCategory.Contest });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_MATERIAL,
                Values = new List<string>
                {
                    "Madera",
                    "Cerámica",
                    "Metal",
                    "Papel",
                    "Reciclado",
                    "Textil",
                    "Piedra"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = CATALOG_TIPO_ENTREGA,
                Values = new List<string>
                {
                    "Físico",
                    "Fotografías",
                    "Video",
                    "Presentación"
                }
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Material utilizado",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                CatalogId = "MATERIAL_PERMITIDO"
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Dimensiones máximas de la pieza",
                Type = FieldTypeEnum.String,
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Peso máximo de la pieza",
                Type = FieldTypeEnum.String,
                IsRequired = false
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Tipo de entrega",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                CatalogId = "TIPO_ENTREGA"
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Premio y recompensas",
                Type = FieldTypeEnum.String,
                IsRequired = true
            });

            data.Fields.Add(new FieldRequest
            {
                Name = "Lineamientos",
                Type = FieldTypeEnum.Archive,
                IsRequired = false
            });

            return data;
        }

        public CategoriesEnum GetCategory() => CategoriesEnum.Crafts;

        public string GetDescription() => "Concursos relacionados a la creación manual de piezas artesanales.";
    }
}

