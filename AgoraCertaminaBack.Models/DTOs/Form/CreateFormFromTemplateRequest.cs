using AgoraCertaminaBack.Models.DTOs.Form.Templates;

namespace AgoraCertaminaBack.Models.DTOs.Form
{
    public class CreateFormFromTemplateRequest
    {
        public string FormName { get; set; } = string.Empty;

        // NUEVO: Ahora usamos TemplateId en lugar de Category directamente
        public string TemplateId { get; set; } = string.Empty;

        // OPCIONAL: Mantener por compatibilidad hacia atrás (deprecated)
        [Obsolete("Use TemplateId instead")]
        public FormTemplateCategory? TemplateCategory { get; set; }
    }
}