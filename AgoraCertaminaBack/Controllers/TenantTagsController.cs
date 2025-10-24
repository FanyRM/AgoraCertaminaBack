using Microsoft.AspNetCore.Mvc;
using ROP;
using AgoraCertaminaBack.Authorization.AttributeHandler;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Authorization;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("tenant-tags")]
    public class TenantTagsController(TenantTagUseCases _customTag) : Controller
    {
        //[HasPermissionOnAction(Constants.Actions.AddTenantCatalogs)]
        [AllowAnonymous]
        [HttpPost("")]
        public async Task<ActionResult<GenericResponse<string>>> CreateTenantTag([FromBody] CustomTagRequest request)
        {
            return await _customTag.CreateTenantTag.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.ReadTenantTags)]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<GenericResponse<List<Tag>>>> GetAllTenantTags([FromQuery] List<TagCategory>? categories)
        {
            return await _customTag.GetAllTenantTags.Execute(categories)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.DeleteTenantTags)]
        [AllowAnonymous]
        [HttpDelete("{tenantTagId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteTenantTag(string tenantTagId)
        {
            return await _customTag.DeleteTenantTag.Execute(tenantTagId)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
