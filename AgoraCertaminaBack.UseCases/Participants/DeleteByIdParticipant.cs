using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Tenants;

namespace AgoraCertaminaBack.UseCases.Participants
{
    public class DeleteByIdParticipant(IMongoRepository<Participant> _mongoRepository, GetByIdTenant _getById, UserRequestContext _userRequest)
    {
        public async Task<Result<Unit>> Execute(string employeeId)
        {
            return await _getById.Execute(_userRequest.OrganizationId)
                .Bind(tenant => DeleteParticipant(_userRequest.OrganizationId, employeeId));
        }

        public async Task<Result<Unit>> DeleteParticipant(string customerId, string employeeId)
        {
            var participant = await _mongoRepository.FindByIdAsync(employeeId);
            if (participant == null)
            {
                return Result.NotFound<Unit>("The participant was not found");
            }

            if (participant.OrganizationId != customerId)
            {
                return Result.NotFound<Unit>("The participant does not belong to the specified tenant");
            }

            participant.IsActive = false;
            await _mongoRepository.ReplaceOneAsync(participant);

            return Result.Success();
        }
    }
}