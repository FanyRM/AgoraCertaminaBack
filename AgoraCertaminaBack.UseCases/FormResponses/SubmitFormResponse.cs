using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;

public class SubmitFormResponse(
    IMongoRepository<FormResponse> _formResponseRepository,
    IMongoRepository<Form> _formRepository,
    IMongoRepository<Participant> _participantRepository)
{
    public async Task<Result<string>> Execute(SubmitFormResponseRequest request)
    {
        var response = await _formResponseRepository.FindByIdAsync(request.ResponseId);
        if (response == null)
            return Result.NotFound<string>("Response not found");

        // Validar campos requeridos
        var form = await _formRepository.FindByIdAsync(response.FormId);
        var requiredFields = form.FormFields.Where(f => f.IsRequired).ToList();

        foreach (var required in requiredFields)
        {
            var hasValue = response.FieldResponses.Any(fr =>
                fr.FieldId == required.Id &&
                !string.IsNullOrWhiteSpace(fr.Value));

            if (!hasValue)
                return Result.BadRequest<string>($"Required field '{required.Name}' is missing");
        }

        // Crear o buscar participante
        var participant = await _participantRepository.FindOneAsync(p =>
            p.FirstName == request.ParticipantInfo.FirstName &&
            p.LastName == request.ParticipantInfo.LastName &&
            p.OrganizationId == response.OrganizationId);

        if (participant == null)
        {
            participant = new Participant
            {
                OrganizationId = response.OrganizationId,
                TenantName = response.TenantName,
                FirstName = request.ParticipantInfo.FirstName,
                LastName = request.ParticipantInfo.LastName,
                Email = request.ParticipantInfo.Email,
                PhoneNumber = request.ParticipantInfo.Phone,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _participantRepository.InsertOneAsync(participant);
        }

        // Marcar como enviado
        response.ParticipantId = participant.Id;
        response.ParticipantName = $"{participant.FirstName} {participant.LastName}";
        //response.Status = FormResponseStatus.Submitted;
        response.SubmittedAt = DateTime.UtcNow;
        response.UpdatedAt = DateTime.UtcNow;

        await _formResponseRepository.ReplaceOneAsync(response);
        return response.Id.Success();
    }
}