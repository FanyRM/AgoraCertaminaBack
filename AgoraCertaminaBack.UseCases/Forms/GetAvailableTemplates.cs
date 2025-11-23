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

        public Task<Result<List<FormTemplateInfo>>> Execute()
        {
            var templates = _templateProvider.GetAvailableTemplates();
            return Task.FromResult(templates.Success());
        }
    }
}