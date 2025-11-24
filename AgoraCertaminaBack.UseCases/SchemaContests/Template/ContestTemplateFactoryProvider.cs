using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template
{
    public class ContestTemplateFactoryProvider
    {
        private readonly Dictionary<CategoriesEnum, Func<IContestTemplateFactory>> _factories;

        public ContestTemplateFactoryProvider()
        {
            _factories = new Dictionary<CategoriesEnum, Func<IContestTemplateFactory>>
            {
                { CategoriesEnum.Crafts, () => new CraftsContestTemplate() },
                { CategoriesEnum.Competition, () => new CompetitionContestTemplate() },
                { CategoriesEnum.Projects, () => new ProjectsContestTemplate() },
                { CategoriesEnum.Literature, () => new LiteratureContestTemplate() },
                { CategoriesEnum.Knowledge, () => new KnowledgeContestTemplate() },
                { CategoriesEnum.Talent, () => new TalentContestTemplate() },
                { CategoriesEnum.Technology, () => new TechnologyContestTemplate() }

            };

        }

        public IContestTemplateFactory GetFactory(CategoriesEnum category)
        {
            if (_factories.TryGetValue(category, out var factoryFunc))
            {
                return factoryFunc();
            }

            return null;
        }

        public List<ContestTemplateInfo> GetAvailableContestTemplates()
        {
            return _factories.Select(kvp =>
            {
                var factory = kvp.Value();
                return new ContestTemplateInfo
                {
                    Category = kvp.Key,
                    Description = factory.GetDescription()
                };
            }).ToList();
        }
    }

    public class ContestTemplateInfo
    {
        public CategoriesEnum Category { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
