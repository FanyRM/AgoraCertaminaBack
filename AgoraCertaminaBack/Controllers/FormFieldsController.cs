using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using AgoraCertaminaBack.Authorization.AttributeHandler;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.DTOs.Form.FormFields;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("form-fields")]
    public class FormFieldsController(FormFieldsUseCases _formFields) : Controller
    {
        //[HasPermissionOnAction(Constants.Actions.AddFormFields)]
        [AllowAnonymous]
        [HttpPost("{formId}")]
        public async Task<ActionResult<GenericResponse<string>>> CreateFormField(string formId, [FromBody] CustomFieldRequest request)
        {
            return await _formFields.CreateFormField.Execute(formId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.ReadFormFields)]
        [AllowAnonymous]
        [HttpGet("{formId}")]
        public async Task<ActionResult<GenericResponse<List<CustomField>>>> GetAllFormFieldsById(string formId)
        {
            return await _formFields.GetAllFormFields.Execute(formId)
                .ToGenericResponse()
                .ToActionResult();
        }
        //[AllowAnonymous]
        //[HttpGet("variations/{fieldType}")]
        //public ActionResult<GenericResponse<List<FieldTypeEnum>>> GetByIdFormVariations(FieldTypeEnum fieldType)
        //{
        //    return _formFields.GetByIdFormVariations.Execute(fieldType)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}


        //[HasPermissionOnAction(Constants.Actions.UpdateFormFields)]
        [AllowAnonymous]
        [HttpPut("{formId}")]
        public async Task<ActionResult<GenericResponse<CustomField>>> UpdateFormField(string formId, [FromBody] UpdateFormFieldRequest request)
        {
            return await _formFields.UpdateFormField.Execute(formId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.UpdateFormFields)]
        [AllowAnonymous]
        [HttpPut("order/{formId}")]
        public async Task<ActionResult<GenericResponse<bool>>> UpdateFormFieldsOrder(string formId, [FromBody] List<ReorderFormFieldsRequest> request)
        {
            return await _formFields.UpdateFormFieldsOrder.Execute(formId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.DeleteFormFields)]
        [AllowAnonymous]
        [HttpDelete("{formId}/{fieldId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteFormField(string formId, string fieldId)
        {
            return await _formFields.DeleteFormField.Execute(formId, fieldId)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
