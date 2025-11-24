using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Forms.Templates;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Academic
{
    /// Plantilla para competencias de MATEMÁTICAS
    public class MathCompetitionTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "academic-math";
        public string GetName() => "Competencia de Matemáticas";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.AcademicCompetition;
        public string GetDescription() => "Plantilla para olimpiadas y competencias de matemáticas";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Matemáticas",
                Color = "blue",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Nivel Académico",
                Values = new List<string>
                {
                    "Primaria",
                    "Secundaria",
                    "Preparatoria",
                    "Universidad"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Área de Especialización",
                Values = new List<string>
                {
                    "Álgebra",
                    "Geometría",
                    "Cálculo",
                    "Trigonometría",
                    "Estadística",
                    "Matemáticas Discretas"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Participante",
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
                Name = "Institución Educativa",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nivel Académico",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "NIVEL_ACADÉMICO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Área de Interés",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = false,
                Order = 4,
                StaticValue = "",
                CatalogId = "ÁREA_DE_ESPECIALIZACIÓN_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Experiencia Previa en Competencias",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 5,
                StaticValue = ""
            });

            return data;
        }
    }

    //COMMENT: Plantilla para competencias de CIENCIAS
    public class ScienceCompetitionTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "academic-science";
        public string GetName() => "Competencia de Ciencias";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.AcademicCompetition;
        public string GetDescription() => "Plantilla para ferias y competencias de ciencias";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Ciencias",
                Color = "green",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Área Científica",
                Values = new List<string>
                {
                    "Física",
                    "Química",
                    "Biología",
                    "Ciencias de la Tierra",
                    "Astronomía",
                    "Ciencias Ambientales"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Tipo de Proyecto",
                Values = new List<string>
                {
                    "Experimental",
                    "Investigación Teórica",
                    "Aplicación Tecnológica",
                    "Estudio de Campo"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Investigador",
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
                Name = "Título del Proyecto",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Área Científica",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "ÁREA_CIENTÍFICA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Tipo de Proyecto",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "TIPO_DE_PROYECTO_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Resumen del Proyecto",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Hipótesis",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }

    //COMMENT: Plantilla para competencias de DEBATE
    public class DebateCompetitionTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "academic-debate";
        public string GetName() => "Competencia de Debate";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.AcademicCompetition;
        public string GetDescription() => "Plantilla para torneos de debate académico";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Debate",
                Color = "rose",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Formato de Debate",
                Values = new List<string>
                {
                    "Debate Parlamentario",
                    "Debate Lincoln-Douglas",
                    "Debate Karl Popper",
                    "Debate Público",
                    "Debate Académico"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Experiencia en Debate",
                Values = new List<string>
                {
                    "Principiante",
                    "Intermedio",
                    "Avanzado",
                    "Profesional"
                }
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nombre del Debatiente",
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
                Name = "Institución que Representa",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 2,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Formato Preferido",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 3,
                StaticValue = "",
                CatalogId = "FORMATO_DE_DEBATE_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Nivel de Experiencia",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "EXPERIENCIA_EN_DEBATE_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Temas de Interés",
                Type = FieldTypeEnum.String,
                IsRequired = false,
                Order = 5,
                StaticValue = ""
            });

            return data;
        }
    }


    //COMMENT: Plantilla para competencias de ROBÓTICA
    public class RoboticsCompetitionTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "academic-robotics";
        public string GetName() => "Competencia de Robótica";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.AcademicCompetition;
        public string GetDescription() => "Plantilla para competencias de robótica y tecnología";

        public FormTemplateData GetTemplateData()
        {
            var data = new FormTemplateData();

            data.Tags.Add(new CustomTagRequest
            {
                Name = "Robótica",
                Color = "purple",
                Category = TagCategory.Form
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Categoría de Robot",
                Values = new List<string>
                {
                    "Robot Sumo",
                    "Robot Seguidor de Línea",
                    "Robot Humanoides",
                    "Drones",
                    "Robótica Educativa",
                    "Robot de Competencia"
                }
            });

            data.Catalogs.Add(new CreateCustomCatalogRequest
            {
                Name = "Plataforma Utilizada",
                Values = new List<string>
                {
                    "Arduino",
                    "Raspberry Pi",
                    "LEGO Mindstorms",
                    "VEX Robotics",
                    "Plataforma Propia"
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
                Name = "Nombre del Robot",
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
                CatalogId = "CATEGORÍA_DE_ROBOT_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Plataforma",
                Type = FieldTypeEnum.CustomCatalog,
                IsRequired = true,
                Order = 4,
                StaticValue = "",
                CatalogId = "PLATAFORMA_UTILIZADA_PLACEHOLDER"
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Descripción del Diseño",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 5,
                StaticValue = ""
            });

            data.Fields.Add(new CustomFieldRequest
            {
                Name = "Integrantes del Equipo",
                Type = FieldTypeEnum.String,
                IsRequired = true,
                Order = 6,
                StaticValue = ""
            });

            return data;
        }
    }
}