namespace AgoraCertaminaBack.Models.DTOs
{
    // ============================================
    // ENUMS
    // ============================================
    public enum EmailType
    {
        InscripcionConfirmada,
        PagoConfirmado,
        RecordatorioCierre,
        NuevaInscripcionOrganizacion
    }

    public enum RecipientRole
    {
        Participant,
        Organization
    }

    // ============================================
    // DTOs
    // ============================================
    public class EmailRecipientDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public RecipientRole Role { get; set; }
    }

    public class ConvocatoriaEmailDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public OrganizacionEmailDTO Organizacion { get; set; } = new();
        public string? ImagenUrl { get; set; }
        public bool EsGratuita { get; set; }
        public decimal? CostoInscripcion { get; set; }
    }

    public class OrganizacionEmailDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Email { get; set; }
    }

    public class ParticipantEmailDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }

    public class PagoEmailDTO
    {
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string TransaccionId { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty; // "exitoso", "pendiente", "fallido"
    }

    public class FormularioEmailDTO
    {
        public string FormularioId { get; set; } = string.Empty;
        public string ResponseId { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty; // "completo", "parcial", "pendiente"
        public DateTime? FechaEnvio { get; set; }
        public int? PorcentajeCompletado { get; set; }
    }

    // ============================================
    // REQUEST
    // ============================================
    public class SendEmailRequestDTO
    {
        public EmailType Tipo { get; set; }
        public List<EmailRecipientDTO> Destinatarios { get; set; } = new();
        public ConvocatoriaEmailDTO Convocatoria { get; set; } = new();
        public ParticipantEmailDTO Participante { get; set; } = new();
        public PagoEmailDTO? Pago { get; set; }
        public FormularioEmailDTO? Formulario { get; set; }
    }

    // ============================================
    // RESPONSE
    // ============================================
    public class SendEmailResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int EmailsSent { get; set; }
        public List<string>? Errors { get; set; }
    }
}