using MongoDB.Driver;
using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.Mappers;

namespace AgoraCertaminaBack.UseCases.Forms
{
    public class UpdateForm(IMongoRepository<Form> _mongoRepository)
    {
        public async Task<Result<Form>> Execute(string formId, UpdateFormRequest request)
        {
            return await ValidateSchemaExist(formId)
                .Bind(form => ValidateUniquename(form, request))
                .Bind(form => SetUpdateForm(form, request));
        }

        private async Task<Result<Form>> ValidateSchemaExist(string formId)
        {
            var form = await _mongoRepository.FindByIdAsync(formId);

            if (form == null || !form.IsActive)
                return Result.Failure<Form>("Form not found or inactive");

            return form.Success();
        }

        private async Task<Result<Form>> ValidateUniquename(Form form, UpdateFormRequest request)
        {
            // Valida solo si el nombre cambió
            if (request.FormName == form.FormName)
                return form.Success();

            bool schemaExists = await _mongoRepository.ExistsAsync(s =>
            s.IsActive &&
            s.OrganizationId == form.OrganizationId &&
            s.FormName == request.FormName &&
            s.Id != form.Id); // para excluir el actual

            if (schemaExists)
                return Result.Failure<Form>("A form with this name already exist for the tenant");

            return form.Success();
        }

        private async Task<Result<Form>> SetUpdateForm(Form form, UpdateFormRequest request)
        {
            form.FormName = request.FormName;
            form.Tags = request.Tags.Select(t => t.ToCustomTag()).ToList();

            await _mongoRepository.ReplaceOneAsync(form);

            return form.Success();
        }
    }
}

