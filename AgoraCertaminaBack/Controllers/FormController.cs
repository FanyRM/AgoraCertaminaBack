using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases.Forms.Templates;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("form")]
    public class FormController(FormsUseCases _forms) : Controller
    {
        #region Templates (Method Factory Pattern)
        
        /// <summary>
        /// Obtiene todas las plantillas de formularios disponibles
        /// </summary>
        [HttpGet("templates")]
        public async Task<ActionResult<GenericResponse<List<FormTemplateInfo>>>> GetAvailableTemplates()
        {
            return await _forms.GetAvailableTemplates.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        /// <summary>
        /// Crea un formulario basado en una plantilla predefinida.
        /// La plantilla incluye tags, catálogos y campos pre-configurados.
        /// </summary>
        [HttpPost("from-template")]
        public async Task<ActionResult<GenericResponse<string>>> CreateFormFromTemplate(
            [FromBody] CreateFormFromTemplateRequest request)
        {
            return await _forms.CreateFormFromTemplate.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        #endregion

        #region Administration Forms
        
        /// <summary>
        /// Crea un formulario vacío (sin campos predefinidos)
        /// </summary>
        [HttpPost("")]
        public async Task<ActionResult<GenericResponse<string>>> CreateForm([FromBody] CreateFormRequest request)
        {
            return await _forms.CreateForm.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet("")]
        public async Task<ActionResult<GenericResponse<List<FormDTO>>>> GetAllForms()
        {
            return await _forms.GetAllForms.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet("{formId}")]
        public async Task<ActionResult<GenericResponse<FormDTO>>> GetByIdFormDTO(string formId)
        {
            return await _forms.GetByIdFormDTO.Execute(formId)
                .ToGenericResponse()
                .ToActionResult();
        }

        [NonAction]
        [HttpPut("{formId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> UpdateForm(string formId, [FromBody] UpdateFormRequest request)
        {
            return await _forms.UpdateForm.Execute(formId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpDelete("{formId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteFormById(string formId)
        {
            return await _forms.DeleteFormById.Execute(formId)
                .ToGenericResponse()
                .ToActionResult();
        }
        
        #endregion
    }
}