using Microsoft.AspNetCore.Mvc;
using ROP;
using AgoraCertaminaBack.Authorization.AttributeHandler;
using AgoraCertaminaBack.Models.DTOs.Form.FormTag;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("form-tags")]
    public class FormTagsController(FormTagsUseCases _formTags) : Controller
    {
        //[HasPermissionOnAction(Constants.Actions.AddTenantTags)]
        //[HttpPost("{formId}")]
        //public async Task<ActionResult<GenericResponse<CustomTag>>> AssignFormTag(string formId, [FromBody] ActionFormTagRequest request)
        //{
        //    return await _formTags.AssignFormTag.Execute(formId, request)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.ReadTenants)]
        //[HttpGet("")]
        //public async Task<ActionResult<GenericResponse<List<CustomTag>>>> GetAllFormTags(string formId)
        //{
        //    return await _formTags.GetAllFormTags.Execute(formId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        [HasPermissionOnAction(Constants.Actions.DeleteTenantTags)]
        [HttpDelete("{formId}/{schemaTagId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteFormTag(string formId, string schemaTagId)
        {
            return await _formTags.DeleteFormTag.Execute(formId, schemaTagId)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
