using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;

namespace AgoraCertaminaBack.UseCases.Participants
{
    public class GetByIdParticipant(IMongoRepository<Participant> repository)
    {
        public async Task<Result<Participant>> Execute(string id)
        {
            var participant = await repository.FindByIdAsync(id);

            if (participant == null)
                return Result.NotFound<Participant>("Participant not found");

            return participant.Success();
        }
    }
}