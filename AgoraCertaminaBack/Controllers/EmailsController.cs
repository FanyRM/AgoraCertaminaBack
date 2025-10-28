using AgoraCertaminaBack.Models.DTOs;
using AgoraCertaminaBack.Models.Response;
using AgoraCertaminaBack.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailsController> _logger;

        public EmailsController(IEmailService emailService, ILogger<EmailsController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Envía un email genérico
        /// </summary>
        [HttpPost("send")]
        [AllowAnonymous]
        public async Task<ActionResult<GenericResponse<SendEmailResponseDTO>>> SendEmail([FromBody] SendEmailRequestDTO request)
        {
            try
            {
                _logger.LogInformation("📧 Solicitud de envío de email recibida");

                var result = await _emailService.SendEmailAsync(request);

                if (result.Success)
                {
                    return Ok(new GenericResponse<SendEmailResponseDTO>
                    {
                        HttpStatusCode = HttpStatusCode.OK,
                        ResponseType = "Success",
                        Response = result
                    });
                }

                return BadRequest(new GenericResponse<SendEmailResponseDTO>
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    ResponseType = "Error",
                    Response = result,
                    Errors = result.Errors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al procesar solicitud de email");

                return StatusCode(500, new GenericResponse<SendEmailResponseDTO>
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ResponseType = "Error",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Envía email de confirmación de pago
        /// </summary>
        [HttpPost("send/pago-confirmado")]
        public async Task<ActionResult<GenericResponse<bool>>> SendPagoConfirmado(
            [FromBody] SendPagoConfirmadoRequestDTO request)
        {
            try
            {
                var result = await _emailService.SendPagoConfirmadoAsync(
                    request.Convocatoria,
                    request.Participante,
                    request.Pago
                );

                if (result)
                {
                    return Ok(new GenericResponse<bool>
                    {
                        HttpStatusCode = HttpStatusCode.OK,
                        ResponseType = "Success",
                        Response = true
                    });
                }
                else
                {
                    return BadRequest(new GenericResponse<bool>
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        ResponseType = "Error",
                        Response = false,
                        Errors = new List<string> { "Error al enviar email de pago" }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al enviar email de pago");

                return StatusCode(500, new GenericResponse<bool>
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ResponseType = "Error",
                    Response = false,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Envía email de recordatorio de cierre
        /// </summary>
        [HttpPost("send/recordatorio-cierre")]
        public async Task<ActionResult<GenericResponse<bool>>> SendRecordatorioCierre(
            [FromBody] SendRecordatorioCierreRequestDTO request)
        {
            try
            {
                var result = await _emailService.SendRecordatorioCierreAsync(
                    request.Convocatoria,
                    request.Participante,
                    request.DiasRestantes
                );

                if (result)
                {
                    return Ok(new GenericResponse<bool>
                    {
                        HttpStatusCode = HttpStatusCode.OK,
                        ResponseType = "Success",
                        Response = true
                    });
                }
                else
                {
                    return BadRequest(new GenericResponse<bool>
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        ResponseType = "Error",
                        Response = false,
                        Errors = new List<string> { "Error al enviar email de recordatorio" }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al enviar email de recordatorio");

                return StatusCode(500, new GenericResponse<bool>
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ResponseType = "Error",
                    Response = false,
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }

    // ============================================
    // REQUEST DTOs SIMPLIFICADOS
    // ============================================
    public class SendInscripcionConfirmadaRequestDTO
    {
        public ConvocatoriaEmailDTO Convocatoria { get; set; } = new();
        public ParticipantEmailDTO Participante { get; set; } = new();
        public FormularioEmailDTO Formulario { get; set; } = new();
    }

    public class SendPagoConfirmadoRequestDTO
    {
        public ConvocatoriaEmailDTO Convocatoria { get; set; } = new();
        public ParticipantEmailDTO Participante { get; set; } = new();
        public PagoEmailDTO Pago { get; set; } = new();
    }

    public class SendRecordatorioCierreRequestDTO
    {
        public ConvocatoriaEmailDTO Convocatoria { get; set; } = new();
        public ParticipantEmailDTO Participante { get; set; } = new();
        public int DiasRestantes { get; set; }
    }
}
