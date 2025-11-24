using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.UseCases.Forms.Templates;
using ROP;

namespace AgoraCertaminaBack.UseCases.Forms
{
    public class GetAvailableTemplates
    {
        private readonly FormTemplateFactoryProvider _templateProvider;

        public GetAvailableTemplates(FormTemplateFactoryProvider templateProvider)
        {
            _templateProvider = templateProvider;
        }

        ///COMMENT: Obtiene todas las plantillas
        public Task<Result<List<FormTemplateInfo>>> Execute()
        {
            var templates = _templateProvider.GetAvailableTemplates();
            return Task.FromResult(templates.Success());
        }

        ///COMMENT: Filtra categoria 
        public Task<Result<List<FormTemplateInfo>>> ExecuteByCategory(FormTemplateCategory category)
        {
            var templates = _templateProvider.GetTemplatesByCategory(category);
            return Task.FromResult(templates.Success());
        }

        public Task<Result<Dictionary<FormTemplateCategory, int>>> ExecuteCategorySummary()
        {
            var summary = _templateProvider.GetCategorySummary();
            return Task.FromResult(summary.Success());
        }
    }
}