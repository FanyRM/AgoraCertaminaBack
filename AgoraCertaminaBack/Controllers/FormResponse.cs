using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using AgoraCertaminaBack.Models.Response;
namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("responses")]
    public class FormResponsesController(FormResponsesUseCases _useCases) : Controller
    {
        [HttpGet("form/{formId}")]
        public async Task<ActionResult<GenericResponse<FormDTO>>> GetFormToResponse(string formId)
        {
            return await _useCases.GetFormToResponse.Execute(formId)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpPost("save")]
        public async Task<ActionResult<GenericResponse<string>>> SaveFormResponse(
            [FromBody] SaveFormResponseRequest request)
        {
            return await _useCases.SaveFormResponse.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpPost("submit")]
        public async Task<ActionResult<GenericResponse<string>>> SubmitFormResponse(
            [FromBody] SubmitFormResponseRequest request)
        {
            return await _useCases.SubmitFormResponse.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
