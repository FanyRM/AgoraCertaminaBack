using AgoraCertaminaBack.UseCases.Participants;

namespace AgoraCertaminaBack.UseCases
{
    public record class ParticipantUseCases(
        CreateParticipant CreateParticipant,
        GetAllParticipants GetAllParticipants,
        UpdateParticipant UpdateParticipant,
        DeleteByIdParticipant DeleteByIdParticipant
    );
}
