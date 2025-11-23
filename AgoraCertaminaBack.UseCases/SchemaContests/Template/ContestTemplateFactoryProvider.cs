using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template
{
    public class ContestTemplateFactoryProvider
    {
        private readonly Dictionary<CategoriesEnum, Func<IContestTemplateFactory>> _factories;

        public ContestTemplateFactoryProvider()
        {
            _factories = new Dictionary<CategoriesEnum, Func<IContestTemplateFactory>>
            {

            };
        }
    }
}
