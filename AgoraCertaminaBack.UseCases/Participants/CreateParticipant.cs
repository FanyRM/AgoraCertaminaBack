using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Participant;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.UseCases.Tenants;
using System.Net;

namespace AgoraCertaminaBack.UseCases.Participants
{
    public class CreateParticipant(IMongoRepository<Participant> _mongoRepository, GetByIdTenant _getByIdCustomer, UserRequestContext _requestContext)
    {
        public async Task<Result<string>> Execute(CreateParticipantRequest request, string? tenandId = null)
        {
            return await _getByIdCustomer.Execute(tenandId ?? _requestContext.OrganizationId)
                .Bind(tenant => ValidateUniqueName(tenant, request))
                .Bind(tenant => RegistryParticipant(tenant, request));
        }

        public async Task<Result<Tenant>> ValidateUniqueName(Tenant tenant, CreateParticipantRequest request)
        {
            bool alreadyExist = await _mongoRepository.ExistsAsync(e =>
                e.FirstName.Equals(request.FirstName, StringComparison.CurrentCultureIgnoreCase) &&
                e.LastName.Equals(request.LastName, StringComparison.CurrentCultureIgnoreCase) &&
                e.OrganizationId == tenant.Id &&
                e.IsActive
            );

            if (alreadyExist)
                return Result.Failure<Tenant>("There is already a participant with the same data");

            return tenant.Success();
        }

        private async Task<Result<string>> RegistryParticipant(Tenant tenant, CreateParticipantRequest request)
        {
            var participant = request.ToParticipant(tenant.Id, tenant.TenantName);

            await _mongoRepository.InsertOneAsync(participant);

            return participant.Id.Success(HttpStatusCode.OK);
        }
    }
}
