//using ROP;
//using AgoraCertaminaBack.Models.DTOs.CustomCatalog;

//namespace AgoraCertaminaBack.UseCases.Tenants.TenantCatalogs
//{
//    public class GetByIdFormProgrammedCatalog(GetByIdFormProgrammed _getByIdFormProgrammed ,GetByIdTenantCatalog _getByIdTenantCatalog)
//    {
//        public async Task<Result<CustomCatalogDTO>> Execute(string formProgrammedId, string catalogId)
//        {
//            return await _getByIdFormProgrammed.Execute(formProgrammedId, validateTenant: false)
//                .Bind(formProgrammed => _getByIdTenantCatalog.Execute(catalogId, formProgrammed.TenantId));
//        }
//    }
//}