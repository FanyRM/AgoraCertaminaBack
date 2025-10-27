using AgoraCertaminaBack.Authorization.AttributeHandler;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;
using AgoraCertaminaBack.UseCases.Shared;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("contests")]
    public class ContestsController(ContestsUseCases _contestsUseCases) : Controller
    {
        [HttpPost("create-contest")]
        public async Task<ActionResult<GenericResponse<string>>> CreateContest([FromBody] CreateContestRequest request)
        {
            return await _contestsUseCases.CreateContest.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<List<ContestDTO>>>> GetContests()
        {
            return await _contestsUseCases.GetContests.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet("get-all-contests")]
        public async Task<ActionResult<GenericResponse<List<ContestDTO>>>> GetAllContestsSystem()
        {
            return await _contestsUseCases.GetAllContestsSystem.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet("disabled")]
        public async Task<ActionResult<GenericResponse<List<ContestDTO>>>> GetDisabledContests()
        {
            return await _contestsUseCases.GetDisabledContests.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet("{contestId}")]
        public async Task<ActionResult<GenericResponse<ContestDTO>>> GetContestById(string contestId)
        {
            return await _contestsUseCases.GetEntityByIdContest.Execute(contestId)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpPut("{contestId}")]
        public async Task<ActionResult<GenericResponse<string>>> UpdateContest(string contestId, [FromBody] ContestUpdateRequest request)
        {
            return await _contestsUseCases.UpdateContest.Execute(contestId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpPut("status/{contestId}")]
        public async Task<ActionResult<GenericResponse<string>>> UpdateStatusContest(string contestId, [FromBody] ContestStatusUpdateRequest request)
        {
            return await _contestsUseCases.UpdateStatusContest.Execute(contestId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpDelete("{contestId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteByIdCustomer(string contestId)
        {
            return await _contestsUseCases.DeleteByIdContest.Execute(contestId)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpPost("get-file")]
        public async Task<ActionResult<GenericResponse<string>>> GetS3Files([FromBody] FileRequest keyPath)
        {
            return await _contestsUseCases.GetS3Files.Execute(keyPath)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}

