using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgoraCertaminaBack.Authorization.AttributeHandler;
using AgoraCertaminaBack.Models.DTOs.CustomCatalog;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.UseCases;

namespace AgoraCertaminaBack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("tenant-catalogs")]
    public class CustomerCatalogsController(TenantCatalogUseCases _tenantCatalogs) : Controller
    {
        // COMMENT: ABIERTO PARA RESPONDER LOS FORMULARIOS QUE USEN OPCIONES DE CATALOGOS
        [AllowAnonymous]
        [HttpGet("forms/{formId}/catalog/{catalogId}")]
        public async Task<ActionResult<GenericResponse<CustomCatalogDTO>>> GetCatalogByForm(
            string formId,
            string catalogId)
        {
            return await _tenantCatalogs.GetByIdFormCatalog.Execute(formId, catalogId)
                .ToGenericResponse()
                .ToActionResult();
        }
        //[HasPermissionOnAction(Constants.Actions.AddTenantCatalogs)]
        [AllowAnonymous]
        [HttpPost("")]
        public async Task<ActionResult<GenericResponse<string>>> CreateTenantCatalog(CreateCustomCatalogRequest request)
        {
            return await _tenantCatalogs.CreateTenantCatalog.Execute(request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.ReadTenantCatalogs)]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<GenericResponse<List<CustomCatalogDTO>>>> GetAllTenantCatalogs()
        {
            return await _tenantCatalogs.GetAllTenantCatalogs.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.ReadTenantCatalogs)]
        [AllowAnonymous]
        [HttpGet("{catalogId}")]
        public async Task<ActionResult<GenericResponse<List<CustomCatalogDTO>>>> GetByIdTenantCatalog(string catalogId)
        {
            return await _tenantCatalogs.GetByIdTenantCatalog.Execute(catalogId)
                .ToGenericResponse()
                .ToActionResult();
        }


        //[HasPermissionOnAction(Constants.Actions.UpdateTenantCatalogs)]
        [AllowAnonymous]
        [HttpPut("{catalogId}")]
        public async Task<ActionResult<GenericResponse<string>>> UpdateTenantCatalogValues(string catalogId, UpdateCustomCatalogValuesRequest request)
        {
            return await _tenantCatalogs.UpdateTenantCatalogValues.Execute(catalogId, request)
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.DeleteTenantCatalogs)]
        [AllowAnonymous]
        [HttpDelete("{catalogId}")]
        public async Task<ActionResult<GenericResponse<string>>> DeleteTenantCatalog(string catalogId)
        {
            return await _tenantCatalogs.DeleteTenantCatalog.Execute(catalogId)
                .ToGenericResponse()
                .ToActionResult();
        }
    }
}
