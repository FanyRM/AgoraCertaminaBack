using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;

namespace AgoraCertaminaBack.UseCases.Forms
{
    public class GetByIdFormDTO(IMongoRepository<Form> _mongoRepository)
    {
        public async Task<Result<FormDTO>> Execute(string formId)
        {
            var form = await _mongoRepository.FindByIdAsync(formId);

            if (form == null)
                return Result.NotFound<FormDTO>("Form not found");

            return form.ToFormDTO();
        }
    }
}
