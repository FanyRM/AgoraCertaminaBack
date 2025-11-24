using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Forms.Templates;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Talent
{
    /// <summary>
    /// Plantilla para concursos de CANTO
    /// </summary>
    public class SingingTalentTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "talent-singing";
        public string GetName() => "Concurso de Canto";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.TalentShow;
        public string GetDescription() => "Plantilla para competencias de canto y voz";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Canto",
                Color = "rose",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Género Musical",
                Values = new List<string>
                {
                    "Pop",
                    "Rock",
                    "Balada",
                    "Ranchera",
                    "Regional Mexicano",
                    "Ópera",
                    "Jazz",
                    "Blues",
                    "Soul/R&B"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tipo de Voz",
                Values = new List<string>
                {
                    "Soprano",
                    "Mezzosoprano",
                    "Contralto",
                    "Tenor",
                    "Barítono",
                    "Bajo"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Cantante",
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
                Name = "Título de la Canción",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Artista Original",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 3,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Género Musical",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "GÉNERO_MUSICAL_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tipo de Voz",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = false,
                Order = 5,
                StaticValue = "",
                CatalogId = "TIPO_DE_VOZ_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Experiencia Musical",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    /// <summary>
    /// Plantilla para concursos de DANZA
    /// </summary>
    public class DanceTalentTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "talent-dance";
        public string GetName() => "Concurso de Danza";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.TalentShow;
        public string GetDescription() => "Plantilla para competencias de baile y danza";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Danza",
                Color = "orangeligh",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Estilo de Danza",
                Values = new List<string>
                {
                    "Ballet Clásico",
                    "Danza Contemporánea",
                    "Jazz",
                    "Hip Hop",
                    "Breakdance",
                    "Salsa",
                    "Bachata",
                    "Folklórica",
                    "Tango"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Categoría de Participación",
                Values = new List<string>
                {
                    "Solo",
                    "Dueto",
                    "Grupo Pequeño (3-7)",
                    "Grupo Grande (8+)"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Participante/Grupo",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 0,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Email de Contacto",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 1,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Título de la Coreografía",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Estilo de Danza",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "ESTILO_DE_DANZA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Categoría",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "CATEGORÍA_DE_PARTICIPACIÓN_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Música Seleccionada",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Número de Integrantes",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    /// <summary>
    /// Plantilla para concursos de MÚSICA INSTRUMENTAL
    /// </summary>
    public class MusicTalentTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "talent-music";
        public string GetName() => "Concurso de Música Instrumental";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.TalentShow;
        public string GetDescription() => "Plantilla para competencias de interpretación musical";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Música",
                Color = "indigo",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Instrumento Principal",
                Values = new List<string>
                {
                    "Piano",
                    "Guitarra",
                    "Violín",
                    "Violonchelo",
                    "Flauta",
                    "Saxofón",
                    "Trompeta",
                    "Batería",
                    "Otro"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Género de la Pieza",
                Values = new List<string>
                {
                    "Clásico",
                    "Jazz",
                    "Blues",
                    "Rock",
                    "Pop",
                    "Música Latina",
                    "Contemporáneo"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Músico",
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
                Name = "Instrumento Principal",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 2,
                StaticValue = "",
                CatalogId = "INSTRUMENTO_PRINCIPAL_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Título de la Pieza",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 3,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Compositor",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 4,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Género Musical",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 5,
                StaticValue = "",
                CatalogId = "GÉNERO_DE_LA_PIEZA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Años de Experiencia",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    /// <summary>
    /// Plantilla para concursos de TEATRO
    /// </summary>
    public class TheaterTalentTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "talent-theater";
        public string GetName() => "Concurso de Teatro";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.TalentShow;
        public string GetDescription() => "Plantilla para competencias de actuación y teatro";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Teatro",
                Color = "red",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Género Teatral",
                Values = new List<string>
                {
                    "Drama",
                    "Comedia",
                    "Tragedia",
                    "Musical",
                    "Monólogo",
                    "Teatro Experimental",
                    "Teatro Infantil"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Formato de Presentación",
                Values = new List<string>
                {
                    "Monólogo",
                    "Escena en Dueto",
                    "Obra Corta",
                    "Improvisación"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Actor/Grupo",
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
                Name = "Título de la Obra/Escena",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Autor",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 3,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Género Teatral",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "GÉNERO_TEATRAL_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Formato",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 5,
                StaticValue = "",
                CatalogId = "FORMATO_DE_PRESENTACIÓN_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Sinopsis",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Duración Aproximada (minutos)",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 7,
                StaticValue = ""
            });

            return data;
        }
    }
}