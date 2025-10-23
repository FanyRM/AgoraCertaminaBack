using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.CustomField;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Forms.FieldsForm
{
    public class CreateFormField(IMongoRepository<Form> _mongoRepository, GetByIdForm _getByIdSchema, IMongoRepository<Tenant> _customerRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(string formId, CustomFieldRequest request)
        {
            return await _getByIdSchema.Execute(formId)
                .Bind(form => ValidateUniquename(form, request))
                .Bind(form => ValidateCatalog(form, request))
                .Bind(form => AddSchemaField(form, request));
        }

        public static Result<Form> ValidateUniquename(Form form, CustomFieldRequest request)
        {
            bool fieldExists = form.FormFields.Any(f =>
                f.IsActive &&
                f.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)
            );
            if (fieldExists)
                return Result.NotFound<Form>("A field with the same name already exists in this form");
            return form.Success();
        }

        private async Task<Result<Form>> ValidateCatalog(Form form, CustomFieldRequest request)
        {
            // validacion para campos de tipo CustomCatalog
            if (request.Type != FieldTypeEnum.CustomCatalog)
                return form.Success();

            if (string.IsNullOrEmpty(request.CatalogId))
                return Result.Failure<Form>("A catalog must be provided");

            // se obtiene el cliente actual para verificar que el catalogo exista
            var tenant = await _customerRepository.FindByIdAsync(_userRequest.OrganizationId);
            if (tenant == null)
                return Result.NotFound<Form>("Tenant not found");

            var catalogExists = tenant.Catalogs.Any(c => c.Id == request.CatalogId && c.IsActive);
            if (!catalogExists)
                return Result.Failure<Form>("The catalog does not exist or is not active");

            return form.Success();
        }

        public async Task<Result<string>> AddSchemaField(Form form, CustomFieldRequest request)
        {
            var customField = request.ToCustomField();

            var nextOrder = form.FormFields
                .Where(f => f.IsActive)
                .Select(f => (int?)f.Order)
                .Max() ?? -1;

            customField.Order = nextOrder + 1;

            form.FormFields.Add(customField);

            await _mongoRepository.ReplaceOneAsync(form);

            return customField.Id.Success(HttpStatusCode.Created);
        }

    }
}