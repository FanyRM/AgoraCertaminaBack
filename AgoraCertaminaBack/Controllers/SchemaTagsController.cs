using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("schema-tags")]
    public class SchemaTagsController(SchemaTagsUseCases _schemaTags) : Controller
    {
        [HttpPost("create-tag/{schemaId}")]
        public async Task<ActionResult<GenericResponse<Tag>>> AddSchemaTag(string schemaId, [FromBody] SchemaTagRequest request)
        {
            return await _schemaTags.AssignSchemaTag.Execute(schemaId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<List<Tag>>>> GetAllSchemaTags(string schemaId)
        {
            return await _schemaTags.GetAllSchemaTags.Execute(schemaId)
                .ToGenericResponse()
                .ToActionResult();
        }

        [HttpDelete("{schemaId}/{schemaTagId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteSchemaTag(string schemaId, string schemaTagId)
        {
            return await _schemaTags.DeleteSchemaTag.Execute(schemaId, schemaTagId)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
