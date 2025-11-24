using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.UseCases.Forms.Template.Implementations.Literary;
using AgoraCertaminaBack.UseCases.Forms.Templates.Implementations;
using AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Academic;
using AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Art;
using AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Literary;
using AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Sports;
using AgoraCertaminaBack.UseCases.Forms.Templates.Implementations.Talent;

namespace AgoraCertaminaBack.UseCases.Forms.Templates
{
    /// <summary>
    /// Proveedor central del patrón Method Factory.
    /// Ahora soporta múltiples plantillas por categoría.
    /// </summary>
    public class FormTemplateFactoryProvider
    {
        // Registro por ID único (principal)
        private readonly Dictionary<string, Func<IFormTemplateFactory>> _factoriesById;

        // Registro por categoría (para compatibilidad y búsquedas)
        private readonly Dictionary<FormTemplateCategory, List<string>> _templatesByCategory;

        public FormTemplateFactoryProvider()
        {
            _factoriesById = new Dictionary<string, Func<IFormTemplateFactory>>();
            _templatesByCategory = new Dictionary<FormTemplateCategory, List<string>>();

            RegisterAllTemplates();
        }

        private void RegisterAllTemplates()
        {
            // Empty
            //RegisterTemplate(() => new EmptyFormTemplate());

            // Contest Registration
            //RegisterTemplate(() => new ContestRegistrationTemplate());

            // LITERARY - Múltiples opciones
            RegisterTemplate(() => new PoetryContestTemplate());
            RegisterTemplate(() => new ShortStoryContestTemplate());
            RegisterTemplate(() => new EssayContestTemplate());
            RegisterTemplate(() => new MicrostoryContestTemplate());


            // ART - Múltiples opciones
            RegisterTemplate(() => new PaintingContestTemplate());
            RegisterTemplate(() => new PhotographyContestTemplate());
            RegisterTemplate(() => new DigitalArtContestTemplate());
            RegisterTemplate(() => new SculptureContestTemplate());

            // ACADEMIC - Múltiples opciones
            RegisterTemplate(() => new MathCompetitionTemplate());
            RegisterTemplate(() => new ScienceCompetitionTemplate());
            RegisterTemplate(() => new DebateCompetitionTemplate());
            RegisterTemplate(() => new RoboticsCompetitionTemplate());

            // TALENT - Múltiples opciones
            RegisterTemplate(() => new SingingTalentTemplate());
            RegisterTemplate(() => new DanceTalentTemplate());
            RegisterTemplate(() => new MusicTalentTemplate());
            RegisterTemplate(() => new TheaterTalentTemplate());

            // SPORTS - Múltiples opciones
            RegisterTemplate(() => new IndividualSportsTemplate());
            RegisterTemplate(() => new TeamSportsTemplate());
            RegisterTemplate(() => new AthleticsTemplate());
            RegisterTemplate(() => new MartialArtsTemplate());
        }

        private void RegisterTemplate(Func<IFormTemplateFactory> factoryFunc)
        {
            var instance = factoryFunc();
            var templateId = instance.GetTemplateId();
            var category = instance.GetCategory();

            // Registrar por ID
            _factoriesById[templateId] = factoryFunc;

            // Registrar en categoría
            if (!_templatesByCategory.ContainsKey(category))
            {
                _templatesByCategory[category] = new List<string>();
            }
            _templatesByCategory[category].Add(templateId);
        }

        /// <summary>
        /// Obtiene la fábrica por ID de plantilla (método principal)
        /// </summary>
        public IFormTemplateFactory GetFactory(string templateId)
        {
            if (_factoriesById.TryGetValue(templateId, out var factoryFunc))
            {
                return factoryFunc();
            }

            // Por defecto retorna plantilla vacía
            return new EmptyFormTemplate();
        }

        /// <summary>
        /// Obtiene la fábrica por categoría (retrocompatibilidad - retorna la primera de la categoría)
        /// </summary>
        [Obsolete("Use GetFactory(string templateId) instead")]
        public IFormTemplateFactory GetFactory(FormTemplateCategory category)
        {
            if (_templatesByCategory.TryGetValue(category, out var templateIds) && templateIds.Any())
            {
                return GetFactory(templateIds.First());
            }

            return new EmptyFormTemplate();
        }

        /// <summary>
        /// Obtiene todas las plantillas disponibles con metadata completa
        /// </summary>
        public List<FormTemplateInfo> GetAvailableTemplates()
        {
            return _factoriesById.Select(kvp =>
            {
                var factory = kvp.Value();
                return new FormTemplateInfo
                {
                    TemplateId = factory.GetTemplateId(),
                    Name = factory.GetName(),
                    Category = factory.GetCategory(),
                    Description = factory.GetDescription()
                };
            }).OrderBy(t => t.Category).ThenBy(t => t.Name).ToList();
        }

        /// <summary>
        /// Obtiene plantillas filtradas por categoría
        /// </summary>
        public List<FormTemplateInfo> GetTemplatesByCategory(FormTemplateCategory category)
        {
            if (!_templatesByCategory.TryGetValue(category, out var templateIds))
            {
                return new List<FormTemplateInfo>();
            }

            return templateIds.Select(id =>
            {
                var factory = GetFactory(id);
                return new FormTemplateInfo
                {
                    TemplateId = factory.GetTemplateId(),
                    Name = factory.GetName(),
                    Category = factory.GetCategory(),
                    Description = factory.GetDescription()
                };
            }).ToList();
        }

        /// <summary>
        /// Obtiene todas las categorías con su cantidad de plantillas
        /// </summary>
        public Dictionary<FormTemplateCategory, int> GetCategorySummary()
        {
            return _templatesByCategory.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Count
            );
        }
    }

    /// <summary>
    /// Información completa de una plantilla disponible
    /// </summary>
    public class FormTemplateInfo
    {
        public string TemplateId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public FormTemplateCategory Category { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}