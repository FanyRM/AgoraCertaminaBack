using Microsoft.AspNetCore.Mvc;
using ROP;
using AgoraCertaminaBack.Authorization.AttributeHandler;
using AgoraCertaminaBack.Models.DTOs.Participant;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Authorization;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("participants")]
    public class ParticipantsController(ParticipantUseCases _participants) : Controller
    {
        //[HasPermissionOnAction(Constants.Actions.AddParticipants)]\
        [AllowAnonymous]
        [HttpPost("")]
        public async Task<ActionResult<GenericResponse<string>>> CreateParticipant([FromBody] CreateParticipantRequest request)
        {
            return await _participants.CreateParticipant.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.ReadParticipants)]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<GenericResponse<List<ParticipantDTO>>>> GetAllParticipants()
        {
            return await _participants.GetAllParticipants.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.ReadParticipants)]
        [AllowAnonymous]
        [HttpGet("paginated")]
        public async Task<ActionResult<GenericResponse<PaginatedResult<ParticipantDTO>>>> GetAllParticipants([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            return await _participants.GetAllParticipants.ExecutePaginated(page, pageSize, search)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.UpdateParticipants)]
        [AllowAnonymous]
        [HttpPut("{participantId}")]
        public async Task<ActionResult<GenericResponse<string>>> UpdateParticipant(string participantId, [FromBody] UpdateParticipantRequest request)
        {
            return await _participants.UpdateParticipant.Execute(participantId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.DeleteParticipants)]
        [AllowAnonymous]
        [HttpDelete("{participantId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteByIdParticipant(string participantId)
        {
            return await _participants.DeleteByIdParticipant.Execute(participantId)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
