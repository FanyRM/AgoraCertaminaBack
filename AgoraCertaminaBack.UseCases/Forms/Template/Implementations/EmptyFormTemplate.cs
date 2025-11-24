using AgoraCertaminaBack.Models.DTOs.Form.Templates;

namespace AgoraCertaminaBack.UseCases.Forms.Templates.Implementations
{
    /// <summary>
    /// Plantilla vacía - comportamiento por defecto (sin campos predefinidos)
    /// </summary>
    public class EmptyFormTemplate : IFormTemplateFactory
    {
        public FormTemplateData GetTemplateData()
        {
            return new FormTemplateData();
        }

        public FormTemplateCategory GetCategory()
        {
            return FormTemplateCategory.Empty;
        }

        public string GetDescription()
        {
            return "Formulario vacío - Sin campos predefinidos";
        }

        public string GetTemplateId()
        {
            return "EmptyFormTemplate";
        }

        public string GetName()
        {
            return "Empty Form Template";
        }
    }
}