using AgoraCertaminaBack.Models.DTOs.Form.Templates;

namespace AgoraCertaminaBack.UseCases.Forms.Templates
{
    /// Interfaz base para el patrón Method Factory de plantillas de formularios.
   
    public interface IFormTemplateFactory
    {
        string GetTemplateId();
        string GetName();
        FormTemplateData GetTemplateData();
        FormTemplateCategory GetCategory();
        string GetDescription();
    }
}