using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Participant;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using System.Net;
namespace AgoraCertaminaBack.UseCases.Participants
{
    public class UpdateParticipant(IMongoRepository<Participant> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<string>> Execute(string employeeId, UpdateParticipantRequest request)
        {
            return await GetParticipantId(employeeId)
                .Bind(participant => ValidateUniqueReg(participant, request))
                .Bind(participant => SetUpdateParticipant(participant, request));
        }

        private async Task<Result<Participant>> GetParticipantId(string employeeId)
        {
            var participant = await _mongoRepository.FindByIdAsync(employeeId);
            if (participant == null || !participant.IsActive)
                return Result.Failure<Participant>("Participant not found or inactive");

            if (participant.OrganizationId != _userRequest.OrganizationId)
                return Result.Failure<Participant>("Unauthorized access to this participant");

            return participant.Success();
        }

        private async Task<Result<Participant>> ValidateUniqueReg(Participant participant, UpdateParticipantRequest request)
        {
            // Solo validar unicidad si se cambiaron email o teléfono
            if (participant.Email == request.Email && participant.PhoneNumber == request.PhoneNumber)
                return participant.Success();

            bool employeeExists = await _mongoRepository.ExistsAsync(e =>
                e.IsActive &&
                e.OrganizationId == participant.OrganizationId &&
                e.Id != participant.Id && // Para excluir el empleado actual
                (e.Email == request.Email || e.PhoneNumber == request.PhoneNumber)
            );

            if (employeeExists)
                return Result.Failure<Participant>("There is already another participant with the same email or phone number");

            return participant.Success();
        }

        private async Task<Result<string>> SetUpdateParticipant(Participant participant, UpdateParticipantRequest request)
        {
            participant.FirstName = request.FirstName;
            participant.LastName = request.LastName;
            participant.Email = request.Email;
            participant.PhoneNumber = request.PhoneNumber;

            // participant.Tags = request.Tags?.Select(t => t.ConvertToCustomTag()).ToList() ?? new List<CustomTag>();

            await _mongoRepository.ReplaceOneAsync(participant);
            return participant.Id.Success(HttpStatusCode.OK);
        }
    }
}