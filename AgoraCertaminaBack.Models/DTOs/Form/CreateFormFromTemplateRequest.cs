using AgoraCertaminaBack.Models.DTOs.Form.Templates;

namespace AgoraCertaminaBack.Models.DTOs.Form
{
    /// <summary>
    /// Request para crear un formulario desde una plantilla predefinida
    /// </summary>
    public class CreateFormFromTemplateRequest
    {
        public required string FormName { get; set; }
        public required FormTemplateCategory TemplateCategory { get; set; }
    }
}