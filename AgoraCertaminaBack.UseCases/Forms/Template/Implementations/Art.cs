using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Forms.Templates;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Art
{
    //COMMENT: Plantilla para concursos de PINTURA
    public class PaintingContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "art-painting";
        public string GetName() => "Concurso de Pintura";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.ArtContest;
        public string GetDescription() => "Plantilla para concursos de pintura en diversas técnicas y estilos";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Pintura",
                Color = "orange",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Técnica de Pintura",
                Values = new List<string>
                {
                    "Óleo",
                    "Acrílico",
                    "Acuarela",
                    "Témpera",
                    "Pastel",
                    "Técnica Mixta"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tamaño de Obra",
                Values = new List<string>
                {
                    "Pequeño (menos de 30x40 cm)",
                    "Mediano (30x40 a 70x100 cm)",
                    "Grande (más de 70x100 cm)"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Artista",
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
                Name = "Título de la Obra",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Técnica Utilizada",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "TÉCNICA_DE_PINTURA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tamaño de la Obra",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "TAMAÑO_DE_OBRA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Descripción de la Obra",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Imagen de la Obra",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    //COMMENT: Plantilla para concursos de FOTOGRAFÍA
    public class PhotographyContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "art-photography";
        public string GetName() => "Concurso de Fotografía";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.ArtContest;
        public string GetDescription() => "Plantilla para concursos de fotografía artística y documental";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Fotografía",
                Color = "indigo",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Categoría Fotográfica",
                Values = new List<string>
                {
                    "Retrato",
                    "Paisaje",
                    "Naturaleza",
                    "Urbana",
                    "Documental",
                    "Abstracta",
                    "Macro"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tipo de Fotografía",
                Values = new List<string>
                {
                    "Color",
                    "Blanco y Negro",
                    "Sepia"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Fotógrafo",
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
                Name = "Título de la Fotografía",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Categoría",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "CATEGORÍA_FOTOGRÁFICA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tipo",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "TIPO_DE_FOTOGRAFÍA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Descripción o Historia",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Archivo de Fotografía",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    //COMMENT: Plantilla para concursos de ARTE DIGITAL
    public class DigitalArtContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "art-digital";
        public string GetName() => "Concurso de Arte Digital";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.ArtContest;
        public string GetDescription() => "Plantilla para concursos de ilustración digital, diseño gráfico y arte 3D";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Arte Digital",
                Color = "teal",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tipo de Arte Digital",
                Values = new List<string>
                {
                    "Ilustración Digital",
                    "Diseño Gráfico",
                    "Arte 3D",
                    "Pixel Art",
                    "Arte Vectorial",
                    "Concept Art"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Software Utilizado",
                Values = new List<string>
                {
                    "Photoshop",
                    "Illustrator",
                    "Procreate",
                    "Blender",
                    "Cinema 4D",
                    "Otro"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Artista Digital",
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
                Name = "Título del Trabajo",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tipo de Arte Digital",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "TIPO_DE_ARTE_DIGITAL_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Software Principal",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "SOFTWARE_UTILIZADO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Concepto o Inspiración",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Archivo de Obra Digital",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    //COMMENT: Plantilla para concursos de ESCULTURA
    public class SculptureContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "art-sculpture";
        public string GetName() => "Concurso de Escultura";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.ArtContest;
        public string GetDescription() => "Plantilla para concursos de escultura en diversos materiales y técnicas";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Escultura",
                Color = "yellow",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Material de Escultura",
                Values = new List<string>
                {
                    "Mármol",
                    "Bronce",
                    "Madera",
                    "Arcilla",
                    "Piedra",
                    "Metal",
                    "Materiales Reciclados"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Dimensiones",
                Values = new List<string>
                {
                    "Miniatura (menos de 30 cm)",
                    "Pequeña (30-60 cm)",
                    "Mediana (60-150 cm)",
                    "Grande (más de 150 cm)"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Escultor",
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
                Name = "Título de la Escultura",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Material Principal",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "MATERIAL_DE_ESCULTURA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Dimensiones de la Obra",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "DIMENSIONES_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Concepto Artístico",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Fotografías de la Escultura",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }
}