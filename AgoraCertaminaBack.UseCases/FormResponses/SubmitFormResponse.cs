using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.DTOs;
using AgoraCertaminaBack.Services.Interfaces;
using Microsoft.Extensions.Logging;
using ROP;

public class SubmitFormResponse
{
    private readonly IMongoRepository<FormResponse> _formResponseRepository;
    private readonly IMongoRepository<Form> _formRepository;
    private readonly IMongoRepository<Participant> _participantRepository;
    private readonly IMongoRepository<Contest> _contestRepository; // ⬅️ AGREGAR
    private readonly IEmailService _emailService; // ⬅️ AGREGAR
    private readonly ILogger<SubmitFormResponse> _logger; // ⬅️ AGREGAR

    public SubmitFormResponse(
        IMongoRepository<FormResponse> formResponseRepository,
        IMongoRepository<Form> formRepository,
        IMongoRepository<Participant> participantRepository,
        IMongoRepository<Contest> contestRepository, // ⬅️ AGREGAR
        IEmailService emailService, // ⬅️ AGREGAR
        ILogger<SubmitFormResponse> logger) // ⬅️ AGREGAR
    {
        _formResponseRepository = formResponseRepository;
        _formRepository = formRepository;
        _participantRepository = participantRepository;
        _contestRepository = contestRepository; // ⬅️ AGREGAR
        _emailService = emailService; // ⬅️ AGREGAR
        _logger = logger; // ⬅️ AGREGAR
    }

    public async Task<Result<string>> Execute(SubmitFormResponseRequest request)
    {
        try
        {
            _logger.LogInformation("📝 Procesando envío de formulario...");

            // 1. VALIDACIONES EXISTENTES
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

            // 2. CREAR O BUSCAR PARTICIPANTE
            var participant = await _participantRepository.FindOneAsync(p =>
                p.FirstName == request.ParticipantInfo.FirstName &&
                p.LastName == request.ParticipantInfo.LastName &&
                p.OrganizationId == response.TenantId);

            if (participant == null)
            {
                participant = new Participant
                {
                    OrganizationId = response.TenantId,
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

            // 3. MARCAR COMO ENVIADO
            response.ParticipantId = participant.Id;
            response.ParticipantName = $"{participant.FirstName} {participant.LastName}";
            response.SubmittedAt = DateTime.UtcNow;
            response.UpdatedAt = DateTime.UtcNow;
            await _formResponseRepository.ReplaceOneAsync(response);

            _logger.LogInformation("✅ Formulario enviado correctamente");

            // 4. ENVIAR EMAIL DE CONFIRMACIÓN
            try
            {
                _logger.LogInformation("📧 Enviando email de confirmación...");

                // Obtener datos del contest
                var contest = await _contestRepository.FindOneAsync(c => c.FormId == form.Id);

                if (contest != null)
                {
                    await _emailService.SendInscripcionConfirmadaAsync(
                        // Datos de la convocatoria
                        new ConvocatoriaEmailDTO
                        {
                            Id = contest.Id,
                            Titulo = contest.ContestName ?? "Convocatoria",
                            Descripcion = contest.DescriptionContest ?? "",
                            FechaInicio = contest.StartDate,
                            FechaFin = contest.EndDate,
                            Organizacion = new OrganizacionEmailDTO
                            {
                                Nombre = contest.OrganizationName ?? "Organización",
                                Email = null // Ajusta si tienes el email de la org
                            },
                            ImagenUrl = contest.ImageUrl,
                            EsGratuita = !contest.IsPay,
                            CostoInscripcion = contest.Price.HasValue ? (decimal?)contest.Price.Value : null
                        },
                        // Datos del participante
                        new ParticipantEmailDTO
                        {
                            FirstName = participant.FirstName,
                            LastName = participant.LastName,
                            Email = participant.Email,
                            Phone = participant.PhoneNumber
                        },
                        // Datos del formulario
                        new FormularioEmailDTO
                        {
                            FormularioId = form.Id,
                            ResponseId = response.Id,
                            Estado = "completo",
                            FechaEnvio = DateTime.UtcNow,
                            PorcentajeCompletado = 100
                        }
                    );

                    _logger.LogInformation("✅ Email de confirmación enviado correctamente");
                }
                else
                {
                    _logger.LogWarning("⚠️ No se encontró contest asociado al formulario, email no enviado");
                }
            }
            catch (Exception emailEx)
            {
                // NO fallar el submit si el email falla
                _logger.LogWarning(emailEx, "⚠️ Error al enviar email (no crítico, el formulario se guardó correctamente)");
                // Continuar sin retornar error
            }

            // 5. RETORNAR ÉXITO
            return response.Id.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al procesar formulario");
            return Result.BadRequest<string>("Error al procesar formulario");
        }
    }
}