using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Forms.Templates;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Sports
{
    /// <summary>
    /// Plantilla para DEPORTES INDIVIDUALES
    /// </summary>
    public class IndividualSportsTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "sports-individual";
        public string GetName() => "Deportes Individuales";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.SportsCompetition;
        public string GetDescription() => "Plantilla para competencias de deportes individuales";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Deportes Individuales",
                Color = "green",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Disciplina Deportiva",
                Values = new List<string>
                {
                    "Natación",
                    "Ciclismo",
                    "Tenis",
                    "Gimnasia",
                    "Esgrima",
                    "Golf",
                    "Ajedrez"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Categoría por Edad",
                Values = new List<string>
                {
                    "Infantil (8-12 años)",
                    "Juvenil (13-17 años)",
                    "Adulto (18-35 años)",
                    "Master (36+ años)"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Atleta",
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
                Name = "Fecha de Nacimiento",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Disciplina",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "DISCIPLINA_DEPORTIVA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Categoría",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "CATEGORÍA_POR_EDAD_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Club o Asociación",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Mejor Marca Personal",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    /// <summary>
    /// Plantilla para DEPORTES DE EQUIPO
    /// </summary>
    public class TeamSportsTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "sports-team";
        public string GetName() => "Deportes de Equipo";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.SportsCompetition;
        public string GetDescription() => "Plantilla para competencias de deportes en equipo";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Deportes de Equipo",
                Color = "teal",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Deporte de Equipo",
                Values = new List<string>
                {
                    "Fútbol",
                    "Básquetbol",
                    "Voleibol",
                    "Béisbol",
                    "Fútbol Americano",
                    "Hockey",
                    "Rugby"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "División",
                Values = new List<string>
                {
                    "Infantil",
                    "Juvenil",
                    "Amateur",
                    "Profesional"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Equipo",
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
                Name = "Nombre del Representante",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Deporte",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "DEPORTE_DE_EQUIPO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "División",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "DIVISIÓN_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Número de Jugadores",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Club o Institución",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    /// <summary>
    /// Plantilla para ATLETISMO
    /// </summary>
    public class AthleticsTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "sports-athletics";
        public string GetName() => "Atletismo";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.SportsCompetition;
        public string GetDescription() => "Plantilla para competencias de atletismo y pista y campo";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Atletismo",
                Color = "yellow",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Modalidad",
                Values = new List<string>
                {
                    "Velocidad (100m, 200m, 400m)",
                    "Medio Fondo (800m, 1500m)",
                    "Fondo (5000m, 10000m)",
                    "Vallas",
                    "Salto de Altura",
                    "Salto de Longitud",
                    "Lanzamiento de Disco",
                    "Lanzamiento de Jabalina",
                    "Maratón"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Categoría de Edad",
                Values = new List<string>
                {
                    "Sub-15",
                    "Sub-18",
                    "Sub-20",
                    "Senior",
                    "Master"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Atleta",
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
                Name = "Fecha de Nacimiento",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Modalidad Atlética",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "MODALIDAD_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Categoría de Edad",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "CATEGORÍA_DE_EDAD_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Mejor Marca",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Club Atlético",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    /// <summary>
    /// Plantilla para ARTES MARCIALES
    /// </summary>
    public class MartialArtsTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "sports-martial-arts";
        public string GetName() => "Artes Marciales";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.SportsCompetition;
        public string GetDescription() => "Plantilla para competencias de artes marciales y combate";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Artes Marciales",
                Color = "red",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Disciplina Marcial",
                Values = new List<string>
                {
                    "Karate",
                    "Taekwondo",
                    "Judo",
                    "Jiu-Jitsu",
                    "Kung Fu",
                    "Muay Thai",
                    "Boxeo",
                    "MMA"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Cinturón/Grado",
                Values = new List<string>
                {
                    "Blanco",
                    "Amarillo",
                    "Naranja",
                    "Verde",
                    "Azul",
                    "Marrón",
                    "Negro (1er Dan)",
                    "Negro (2do Dan o superior)"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Categoría de Peso",
                Values = new List<string>
                {
                    "Mosca (-50 kg)",
                    "Gallo (-54 kg)",
                    "Pluma (-58 kg)",
                    "Ligero (-63 kg)",
                    "Welter (-69 kg)",
                    "Medio (-75 kg)",
                    "Semipesado (-81 kg)",
                    "Pesado (+81 kg)"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Competidor",
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
                Name = "Disciplina",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 2,
                StaticValue = "",
                CatalogId = "DISCIPLINA_MARCIAL_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Cinturón/Grado",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "CINTURÓN/GRADO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Peso (kg)",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 4,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Categoría de Peso",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 5,
                StaticValue = "",
                CatalogId = "CATEGORÍA_DE_PESO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Dojo/Gimnasio",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 6,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Años de Práctica",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 7,
                StaticValue = ""
            });

            return data;
        }
    }
}