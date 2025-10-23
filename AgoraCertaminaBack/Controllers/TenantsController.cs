using AgoraCertaminaBack.Models.DTOs.Tenant;
using AgoraCertaminaBack.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;
using AgoraCertaminaBack.Models.Response;


namespace AgoraCertaminaBack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("tenant")]
    public class TenantsController(TenantsUseCases _customers) : Controller
    {

        //[HasPermissionOnAction(Constants.Actions.ReadTenants)]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<GenericResponse<List<TenantDTO>>>> GetAllTenants()
        {
            return await _customers.GetAllTenants.Execute()
                .ToGenericResponse()
                .ToActionResult();
        }

        //[HasPermissionOnAction(Constants.Actions.DeleteTenants)]
        [AllowAnonymous]
        [HttpDelete("{tenantId}")]
        public async Task<ActionResult<GenericResponse<Unit>>> DeleteByIdTenant(string tenantId)
        {
            return await _customers.DeleteByIdTenant.Execute(tenantId)
                .ToGenericResponse()
                .ToActionResult();
        }

    }
}