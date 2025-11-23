using AgoraCertaminaBack.Models.DTOs.Form.Templates;

namespace AgoraCertaminaBack.UseCases.Forms.Templates
{
    /// <summary>
    /// Interfaz base para el patrón Method Factory de plantillas de formularios.
    /// Cada implementación define la estructura completa de un tipo de formulario.
    /// </summary>
    public interface IFormTemplateFactory
    {
        /// <summary>
        /// Obtiene los datos de la plantilla (tags, catálogos y campos)
        /// </summary>
        FormTemplateData GetTemplateData();

        /// <summary>
        /// Obtiene la categoría de la plantilla
        /// </summary>
        FormTemplateCategory GetCategory();

        /// <summary>
        /// Obtiene una descripción de la plantilla
        /// </summary>
        string GetDescription();
    }
}