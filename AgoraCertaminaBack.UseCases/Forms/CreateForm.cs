using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;
using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.UseCases.Tenants;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Forms
{
    public class CreateForm(IMongoRepository<Form> _mongoRepository, GetByIdTenant _getByIdCustomer, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(CreateFormRequest request)
        {
            return await _getByIdCustomer.Execute(_userRequest.OrganizationId)
                .Bind(tenant => ValidateUniqueName(tenant, request))
                .Bind(tenant => CreateNewForm(tenant, request));
        }

        private async Task<Result<Tenant>> ValidateUniqueName(Tenant tenant, CreateFormRequest request)
        {
            bool schemaExists = await _mongoRepository.ExistsAsync(s =>
                s.IsActive &&
                s.OrganizationId == tenant.Id &&
                s.FormName == request.FormName
            );

            if (schemaExists)
                return Result.Failure<Tenant>("A form with this name already exists");

            return tenant.Success();
        }

        private async Task<Result<string>> CreateNewForm(Tenant tenant, CreateFormRequest request)
        {
            var tags = request.Tags.Select(t => t.ToCustomTag()).ToList();

            var newSchema = new Form
            {
                OrganizationId = tenant.Id,
                TenantName = tenant.TenantName,
                FormName = request.FormName,
                Tags = tags,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _mongoRepository.InsertOneAsync(newSchema);

            return newSchema.Id.Success(HttpStatusCode.Created);
        }
    }
}