using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;
using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;

namespace AgoraCertaminaBack.UseCases.Forms
{
    public class GetAllForms(IMongoRepository<Form> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<List<FormDTO>>> Execute()
        {
            var schemas = await _mongoRepository.FilterByAsync(filter => filter.OrganizationId == _userRequest.OrganizationId && filter.IsActive);

            var result = schemas.Select(form => form.ToFormDTO()).ToList();

            return result;
        }
    }
}