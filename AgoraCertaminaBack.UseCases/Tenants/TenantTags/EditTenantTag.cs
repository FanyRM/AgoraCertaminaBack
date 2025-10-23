using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Tenants.TenantTags
{
    public class UpdateTenantTag(IMongoRepository<Tenant> _mongoRepository, GetByIdTenant _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<Tag>> Execute(EditCustomTagRequest request)
        {
            return await _getById.Execute(_userRequest.OrganizationId)
                .Bind(tenant => EditCustomTag(tenant, request));
        }

        public async Task<Result<Tag>> EditCustomTag(Tenant tenant, EditCustomTagRequest request)
        {
            var tagToUpdate = tenant.Tags.Find(tag => tag.Id == request.Id);

            if (tagToUpdate == null)
                return Result.NotFound<Tag>("Tag not found");

            tagToUpdate.Name = request.Name;
            tagToUpdate.Color = request.Color;

            await _mongoRepository.ReplaceOneAsync(tenant);

            return tagToUpdate.Success(HttpStatusCode.OK);

        }
    }
}
