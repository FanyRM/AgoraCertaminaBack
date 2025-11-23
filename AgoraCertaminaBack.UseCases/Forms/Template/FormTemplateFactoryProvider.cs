using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.UseCases.Forms.Templates.Implementations;

namespace AgoraCertaminaBack.UseCases.Forms.Templates
{
    /// <summary>
    /// Proveedor central del patrón Method Factory.
    /// Responsable de crear instancias de las fábricas de plantillas según la categoría.
    /// </summary>
    public class FormTemplateFactoryProvider
    {
        private readonly Dictionary<FormTemplateCategory, Func<IFormTemplateFactory>> _factories;

        public FormTemplateFactoryProvider()
        {
            // Registro de todas las fábricas disponibles
            _factories = new Dictionary<FormTemplateCategory, Func<IFormTemplateFactory>>
            {
                { FormTemplateCategory.Empty, () => new EmptyFormTemplate() },
                { FormTemplateCategory.EmployeeRegistration, () => new EmployeeRegistrationTemplate() },
                { FormTemplateCategory.CustomerSurvey, () => new CustomerSurveyTemplate() },
                { FormTemplateCategory.EventRegistration, () => new EventRegistrationTemplate() },
                { FormTemplateCategory.JobApplication, () => new JobApplicationTemplate() },
                { FormTemplateCategory.FeedbackForm, () => new FeedbackFormTemplate() }
            };
        }

        /// <summary>
        /// Obtiene la fábrica apropiada según la categoría
        /// </summary>
        public IFormTemplateFactory GetFactory(FormTemplateCategory category)
        {
            if (_factories.TryGetValue(category, out var factoryFunc))
            {
                return factoryFunc();
            }

            // Por defecto retorna plantilla vacía
            return new EmptyFormTemplate();
        }

        /// <summary>
        /// Obtiene todas las plantillas disponibles con sus descripciones
        /// </summary>
        public List<FormTemplateInfo> GetAvailableTemplates()
        {
            return _factories.Select(kvp =>
            {
                var factory = kvp.Value();
                return new FormTemplateInfo
                {
                    Category = kvp.Key,
                    Description = factory.GetDescription()
                };
            }).ToList();
        }
    }

    /// <summary>
    /// Información de una plantilla disponible
    /// </summary>
    public class FormTemplateInfo
    {
        public FormTemplateCategory Category { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}