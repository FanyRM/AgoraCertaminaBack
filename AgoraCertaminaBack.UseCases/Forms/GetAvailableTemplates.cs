using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.UseCases.Forms.Templates;
using ROP;

namespace AgoraCertaminaBack.UseCases.Forms
{
    /// <summary>
    /// Caso de uso para obtener la lista de todas las plantillas disponibles
    /// </summary>
    public class GetAvailableTemplates
    {
        private readonly FormTemplateFactoryProvider _templateProvider;

        public GetAvailableTemplates(FormTemplateFactoryProvider templateProvider)
        {
            _templateProvider = templateProvider;
        }

        /// <summary>
        /// Obtiene todas las plantillas
        /// </summary>
        public Task<Result<List<FormTemplateInfo>>> Execute()
        {
            var templates = _templateProvider.GetAvailableTemplates();
            return Task.FromResult(templates.Success());
        }

        /// <summary>
        /// Obtiene plantillas filtradas por categoría
        /// </summary>
        public Task<Result<List<FormTemplateInfo>>> ExecuteByCategory(FormTemplateCategory category)
        {
            var templates = _templateProvider.GetTemplatesByCategory(category);
            return Task.FromResult(templates.Success());
        }

        /// <summary>
        /// Obtiene resumen de categorías
        /// </summary>
        public Task<Result<Dictionary<FormTemplateCategory, int>>> ExecuteCategorySummary()
        {
            var summary = _templateProvider.GetCategorySummary();
            return Task.FromResult(summary.Success());
        }
    }
}