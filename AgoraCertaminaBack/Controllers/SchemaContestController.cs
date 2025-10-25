using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("schema")]
    public class SchemaContestController(SchemaContestsUseCases _schemasContest) : Controller
    {
        [HttpPost("create-schema")]
        public async Task<ActionResult<GenericResponse<string>>> CreateSchema([FromBody] CreateSchemaContestRequest request)
        {
            return await _schemasContest.CreateSchema.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<List<SchemaContestDTOs>>>> GetAllSchemas()
        {
            return await _schemasContest.GetAllSchemas.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpPut("update-by/{schemaId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> UpdateSchema(string schemaId, UpdateSchemaRequest request)
        {
            return await _schemasContest.UpdateSchema.Execute(schemaId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet("{schemaId}")]
        public async Task<ActionResult<GenericResponse<SchemaContestDTOs>>> GetByIdSchema(string schemaId)
        {
            return await _schemasContest.GetEntityByIdSchema.Execute(schemaId)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet("schema-data/{schemaId}")]
        public async Task<ActionResult<GenericResponse<DataSchemaDTO>>> GetDataSchemaById(string schemaId)
        {
            return await _schemasContest.GetDataSchemaById.Execute(schemaId)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpDelete("{schemaId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteSchema(string schemaId)
        {
            return await _schemasContest.DeleteSchemaById.Execute(schemaId)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
