using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.Forms
{
    public class GetByIdForm(IMongoRepository<Form> _mongoRepository, UserRequestContext _requestContext)
    {
        public async Task<Result<Form>> Execute(string formId, bool validateTenant = true)
        {
            var form = await _mongoRepository.FindByIdAsync(formId);

            if (validateTenant && form.OrganizationId != _requestContext.OrganizationId)
                return Result.BadRequest<Form>("This request is forbidden");

            if (form == null)
                return Result.NotFound<Form>("Form not found");

            return form;
        }
    }
}
