using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantTags
{
    public class CreateTenantTag(IMongoRepository<Tenant> _mongoRepository, GetByIdTenant _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(CustomTagRequest request)
        {
            return await _getById.Execute(_userRequest.OrganizationId)
                .Bind(tenant => ValidateUniqueName(tenant, request.Name))
                .Bind(tenant => AddTag(tenant, request));
        }

        private static Result<Tenant> ValidateUniqueName(Tenant tenant, string tagName)
        {
            bool tagExists = tenant.Tags.Any(t => t.IsActive && t.Name.Equals(tagName, StringComparison.CurrentCultureIgnoreCase));

            if (tagExists)
                return Result.Failure<Tenant>("A tag with this name already exists");

            return tenant.Success();
        }

        public async Task<Result<string>> AddTag(Tenant tenant, CustomTagRequest request)
        {
            var newTag = request.ToCustomTag();

            tenant.Tags.Add(newTag);

            await _mongoRepository.ReplaceOneAsync(tenant);

            return newTag.Id.Success(HttpStatusCode.OK);
        }
    }
}