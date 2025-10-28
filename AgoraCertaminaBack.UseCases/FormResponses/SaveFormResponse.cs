using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using ROP;
using System.Net;

public class SaveFormResponse(
    IMongoRepository<FormResponse> _formResponseRepository,
    IMongoRepository<Form> _formRepository)
{
    public async Task<Result<string>> Execute(SaveFormResponseRequest request)
    {
        // Si ya existe una respuesta, actualizar
        if (!string.IsNullOrEmpty(request.ResponseId))
        {
            var existing = await _formResponseRepository.FindByIdAsync(request.ResponseId);
            if (existing == null)
                return Result.NotFound<string>("Response not found");

            existing.FieldResponses = request.FieldResponses.Select(fr => new FieldResponse
            {
                FieldId = fr.FieldId,
                Value = fr.Value,
                UpdatedAt = DateTime.UtcNow
            }).ToList();

            existing.UpdatedAt = DateTime.UtcNow;
            await _formResponseRepository.ReplaceOneAsync(existing);
            return existing.Id.Success();
        }

        // Si no existe, crear nueva
        var form = await _formRepository.FindByIdAsync(request.FormId);
        if (form == null)
            return Result.NotFound<string>("Form not found");

        var response = new FormResponse
        {
            FormId = request.FormId,
            FormName = form.FormName,
            TenantId = form.OrganizationId,
            TenantName = form.TenantName,
            //Status = FormResponseStatus.Draft,
            FieldResponses = request.FieldResponses.Select(fr => new FieldResponse
            {
                FieldId = fr.FieldId,
                Value = fr.Value,
                CreatedAt = DateTime.UtcNow
            }).ToList(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _formResponseRepository.InsertOneAsync(response);
        return response.Id.Success(HttpStatusCode.Created);
    }
}