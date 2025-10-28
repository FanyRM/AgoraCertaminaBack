using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;
using ROP;
using System.Net;

public class GetFormToResponse(
    IMongoRepository<Form> _formRepository)
{
    public async Task<Result<FormDTO>> Execute(string formId)
    {
        var form = await _formRepository.FindByIdAsync(formId);

        if (form == null || !form.IsActive)
            return Result.NotFound<FormDTO>("Form not found");

        return form.ToFormDTO();
    }
}

