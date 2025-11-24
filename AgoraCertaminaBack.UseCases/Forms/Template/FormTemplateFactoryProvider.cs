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
    public class FormTemplateFactoryProvider
    {
        private readonly Dictionary<string, Func<IFormTemplateFactory>> _factoriesById;
        private readonly Dictionary<FormTemplateCategory, List<string>> _templatesByCategory;

        public FormTemplateFactoryProvider()
        {
            _factoriesById = new Dictionary<string, Func<IFormTemplateFactory>>();
            _templatesByCategory = new Dictionary<FormTemplateCategory, List<string>>();
            RegisterAllTemplates();
        }

        private void RegisterAllTemplates()
        {
            // Literary
            RegisterTemplate(() => new PoetryContestTemplate());
            RegisterTemplate(() => new ShortStoryContestTemplate());
            RegisterTemplate(() => new EssayContestTemplate());
            RegisterTemplate(() => new MicrostoryContestTemplate());

            // Art
            RegisterTemplate(() => new PaintingContestTemplate());
            RegisterTemplate(() => new PhotographyContestTemplate());
            RegisterTemplate(() => new DigitalArtContestTemplate());
            RegisterTemplate(() => new SculptureContestTemplate());

            // Academic
            RegisterTemplate(() => new MathCompetitionTemplate());
            RegisterTemplate(() => new ScienceCompetitionTemplate());
            RegisterTemplate(() => new DebateCompetitionTemplate());
            RegisterTemplate(() => new RoboticsCompetitionTemplate());

            // Talent
            RegisterTemplate(() => new SingingTalentTemplate());
            RegisterTemplate(() => new DanceTalentTemplate());
            RegisterTemplate(() => new MusicTalentTemplate());
            RegisterTemplate(() => new TheaterTalentTemplate());

            // Sports
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

            _factoriesById[templateId] = factoryFunc;

            if (!_templatesByCategory.ContainsKey(category))
            {
                _templatesByCategory[category] = new List<string>();
            }
            _templatesByCategory[category].Add(templateId);
        }
        //COMMENT: Obtiene factory por ID de plantilla
        public IFormTemplateFactory GetFactory(string templateId)
        {
            if (_factoriesById.TryGetValue(templateId, out var factoryFunc))
            {
                return factoryFunc();
            }

            throw new KeyNotFoundException($"Template with ID '{templateId}' not found");
        }

        //COMMENT: Obtiene factory por categoria (obsoleto)
        [Obsolete("Use GetFactory(string templateId) instead")]
        public IFormTemplateFactory GetFactory(FormTemplateCategory category)
        {
            if (_templatesByCategory.TryGetValue(category, out var templateIds) && templateIds.Any())
            {
                return GetFactory(templateIds.First());
            }
            throw new KeyNotFoundException($"No templates found for category '{category}'");
        }

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

        public Dictionary<FormTemplateCategory, int> GetCategorySummary()
        {
            return _templatesByCategory.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Count
            );
        }
    }

    public class FormTemplateInfo
    {
        public string TemplateId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public FormTemplateCategory Category { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}