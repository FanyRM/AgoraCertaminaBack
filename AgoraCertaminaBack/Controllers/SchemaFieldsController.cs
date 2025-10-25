using AgoraCertaminaBack.Models.DTOs.Field;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("schema-fields")]
    public class SchemaFieldsController(SchemaFieldsUseCases _schemaFields) : Controller
    {
        // Crear customer field
        [HttpPost("create-field/{schemaId}")]
        public async Task<ActionResult<GenericResponse<string>>> AddSchemaField(string schemaId, [FromBody] FieldRequest request)
        {
            return await _schemaFields.CreateSchemaField.Execute(schemaId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet("{schemaId}")]
        public async Task<ActionResult<GenericResponse<List<CustomField>>>> GetAllSchemaFields(string schemaId)
        {
            return await _schemaFields.GetAllSchemaFields.Execute(schemaId)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpPut("edit-field/{schemaId}")]
        public async Task<ActionResult<GenericResponse<CustomField>>> EditSchemaField(string schemaId, [FromBody] UpdateSchemaFieldRequest request)
        {

            return await _schemaFields.EditSchemaField.Execute(schemaId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpDelete("{schemaId}/{fieldId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteSchemaField(string schemaId, string fieldId)
        {
            return await _schemaFields.DeleteSchemaField.Execute(schemaId, fieldId)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
