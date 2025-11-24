using AgoraCertaminaBack.UseCases.Forms.Templates;
using AgoraCertaminaBack.UseCases.SchemaContests.Template;
using ROP;

namespace AgoraCertaminaBack.UseCases.SchemaContests
{
    public class GetAvailableContestTemplates (ContestTemplateFactoryProvider _templateProvider)
    {
        public Task<Result<List<ContestTemplateInfo>>> Execute()
        {
            var templates = _templateProvider.GetAvailableContestTemplates();
            return Task.FromResult(templates.Success());
        }
    }
}
