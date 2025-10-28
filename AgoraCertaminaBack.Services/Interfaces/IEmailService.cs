using AgoraCertaminaBack.Models.DTOs;

namespace AgoraCertaminaBack.Services.Interfaces
{
    public interface IEmailService
    {
        Task<SendEmailResponseDTO> SendEmailAsync(SendEmailRequestDTO request);
        Task<bool> SendInscripcionConfirmadaAsync(
            ConvocatoriaEmailDTO convocatoria,
            ParticipantEmailDTO participante,
            FormularioEmailDTO formulario
        );
        Task<bool> SendPagoConfirmadoAsync(
            ConvocatoriaEmailDTO convocatoria,
            ParticipantEmailDTO participante,
            PagoEmailDTO pago
        );
        Task<bool> SendRecordatorioCierreAsync(
            ConvocatoriaEmailDTO convocatoria,
            ParticipantEmailDTO participante,
            int diasRestantes
        );
    }
}