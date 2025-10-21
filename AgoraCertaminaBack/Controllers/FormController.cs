using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using SensusAPI.Models.Response;
namespace SensusAPI.Controllers
{
    [ApiController]
    [Route("form")]
    public class FormController(FormsUseCases _forms) : Controller
    {
        #region Administration Forms
        //[HasPermissionOnAction(Constants.Actions.AddForms)]
        [HttpPost("")]
        public async Task<ActionResult<GenericResponse<string>>> CreateForm([FromBody] CreateFormRequest request)
        {
            return await _forms.CreateForm.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.ReadForms)]
        [HttpGet("")]
        public async Task<ActionResult<GenericResponse<List<FormDTO>>>> GetAllForms()
        {
            return await _forms.GetAllForms.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.ReadForms)]
        //[AllowAnonymous]
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

        //[HasPermissionOnAction(Constants.Actions.DeleteForms)]
        [HttpDelete("{formId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteFormById(string formId)
        {
            return await _forms.DeleteFormById.Execute(formId)
                .ToGenericResponse()
                .ToActionResult();
        }
        #endregion
        
        //#region Get Forms Public or Private
        ////COMMENT: ABIERTO PARA RESPONDER LOS FORMULARIOS
        //[HttpGet("public/preview/{formProgrammedId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<GenericResponse<FormPreviewDTO>>> GetPublicFormPreviewDTO(string formProgrammedId)
        //{
        //    return await _formProgrammed.GetPublicFormPreview.Execute(formProgrammedId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        ////COMMENT: ABIERTO PARA RESPONDER LOS FORMULARIOS
        //[HttpPost("public/access/{formProgrammedId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<GenericResponse<FormDTO>>> ValidatePublicFormAccess(string formProgrammedId, [FromBody] AccessKeyRequest request)
        //{
        //    return await _formProgrammed.ValidatePublicFormAccess.Execute(formProgrammedId, request)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        ////COMMENT: ABIERTO PARA RESPONDER LOS FORMULARIOS
        //[HttpGet("public/{formProgrammedId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<GenericResponse<FormDTO>>> GetPublicFormProgrammed(string formProgrammedId)
        //{
        //    return await _formProgrammed.GetPublicFormProgrammed.Execute(formProgrammedId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        ////COMMENT: ABIERTO PARA RESPONDER LOS FORMULARIOS
        //[HttpGet("private/preview/{assignmentId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<GenericResponse<FormPreviewDTO>>> GetPrivateFormPreviewDTO(string assignmentId)
        //{
        //    return await _formAssigned.GetPrivateFormPreview.Execute(assignmentId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        ////COMMENT: ABIERTO PARA RESPONDER LOS FORMULARIOS
        //[HttpPost("private/access/{assignmentId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<GenericResponse<bool>>> ValidatePrivateFormAccess(string assignmentId, [FromBody] AccessKeyRequest request)
        //{
        //    return await _formAssigned.ValidatePrivateFormAccess.Execute(assignmentId, request)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        ////COMMENT: ABIERTO PARA RESPONDER LOS FORMULARIOS
        //[HttpGet("private/{assignmentId}")]
        //[AllowAnonymous]
        //public async Task<ActionResult<GenericResponse<FormDTO>>> GetPrivateFormProgrammed(string assignmentId)
        //{
        //    return await _formAssigned.GetPrivateFormProgrammed.Execute(assignmentId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}
        //#endregion

        //#region Form Programmed
        //[HasPermissionOnAction(Constants.Actions.AddFormProgrammed)]
        //[HttpPost("programmed")]
        //public async Task<ActionResult<GenericResponse<string>>> CreateFormProgrammed([FromBody] CreateFormProgrammedRequest request)
        //{
        //    return await _formProgrammed.CreateFormProgrammed.Execute(request)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.ReadFormProgrammed)]
        //[HttpGet("programmed/form/{formId}")]
        //public async Task<ActionResult<GenericResponse<FormProgrammedDTO>>> GetAllFormsProgrammed(string formId)
        //{
        //    return await _formProgrammed.GetAllFormsProgrammed.Execute(formId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.ReadFormProgrammed)]
        //[HttpGet("programmed/accesskey/{formProgrammedId}")]
        //public async Task<ActionResult<GenericResponse<string>>> GetAccessKeyFormProgrammed(string formProgrammedId)
        //{
        //    return await _formProgrammed.GetAccessKeyFormProgrammed.Execute(formProgrammedId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.ReadFormProgrammed)]
        //[HttpGet("programmed/{formProgrammedId}")]
        //public async Task<ActionResult<GenericResponse<FormProgrammedDTO>>> GetByIdFormProgrammedDTO(string formProgrammedId)
        //{
        //    return await _formProgrammed.GetByIdFormProgrammedDTO.Execute(formProgrammedId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.UpdateFormProgrammed)]
        //[HttpPut("programmed/{id}")]
        //public async Task<ActionResult<GenericResponse<UpdateFormProgrammedRequest>>> UpdateFormProgrammed([FromBody] UpdateFormProgrammedRequest request)
        //{
        //    return await _formProgrammed.UpdateFormProgrammed.Execute(request)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.DeleteFormProgrammed)]
        //[HttpDelete("programmed/{id}")]
        //public async Task<ActionResult<GenericResponse<FormProgrammed>>> DeleteFormProgrammed(string id)
        //{
        //    return await _formProgrammed.DeleteFormProgrammed.Execute(id)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //#endregion

        //#region Form Assigned
        //[HasPermissionOnAction(Constants.Actions.AddFormAssigned)]
        //[HttpPost("assigned")]
        //public async Task<ActionResult<GenericResponse<string>>> CreateFormAssigned([FromBody] CreateFormAssignmentRequest request)
        //{
        //    return await _formAssigned.CreateFormAssignment.Execute(request)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.ReadFormAssigned)]
        //[HttpGet("assignments/{formProgrammedId}")]
        //public async Task<ActionResult<GenericResponse<List<FormAssignedDTO>>>> GetFormAssigments(string formProgrammedId)
        //{
        //    return await _formAssigned.GetAllFormAssignments.Execute(formProgrammedId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.ReadFormAssigned)]
        //[HttpGet("assigned/accesskey/{formProgrammedId}")]
        //public async Task<ActionResult<GenericResponse<string>>> GetAccessKeyFormAssigned(string formProgrammedId)
        //{
        //        return await _formAssigned.GetAccessKeyFormAssigned.Execute(formProgrammedId)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.UpdateFormAssigned)]
        //[HttpPut("assigned/{formAssignedId}")]
        //public async Task<ActionResult<GenericResponse<string>>> UpdateFormAssigned(string formAssignedId, [FromBody] UpdateFormAssignedRequest request)
        //{
        //    return await _formAssigned.UpdateFormAssigned.Execute(request)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}

        //[HasPermissionOnAction(Constants.Actions.DeleteFormAssigned)]
        //[HttpDelete("assigned/{id}")]
        //public async Task<ActionResult<GenericResponse<string>>> DeleteFormAssigned(string id)
        //{
        //    return await _formAssigned.DeleteFormAssigned.Execute(id)
        //        .ToGenericResponse()
        //        .ToActionResult();
        //}
        //#endregion
    }
}
