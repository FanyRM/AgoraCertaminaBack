using AgoraCertaminaBack.Models.DTOs;
using AgoraCertaminaBack.Services.Interfaces;
using AgoraCertaminaBack.Services.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using System.Text;

namespace AgoraCertaminaBack.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(
            ILogger<EmailService> logger,
            IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public async Task<SendEmailResponseDTO> SendEmailAsync(SendEmailRequestDTO request)
        {
            _logger.LogInformation("📧 Enviando email...");
            _logger.LogInformation($"   - Tipo: {request.Tipo}");
            _logger.LogInformation($"   - Destinatarios: {request.Destinatarios.Count}");
            _logger.LogInformation($"   - Convocatoria: {request.Convocatoria.Titulo}");
            _logger.LogInformation($"   - Participante: {request.Participante.FirstName} {request.Participante.LastName}");

            try
            {
                // Generar contenido del email
                var emailContent = GenerateEmailTemplate(request);
                var subject = GetEmailSubject(request.Tipo);

                if (_emailSettings.UseMockService)
                {
                    // ⬇️ MODO SIMULADO
                    _logger.LogInformation("🧪 Modo simulado - Email NO enviado");
                    _logger.LogInformation("📨 Contenido del email:");
                    _logger.LogInformation(emailContent);
                    await Task.Delay(500);

                    return new SendEmailResponseDTO
                    {
                        Success = true,
                        Message = "Email enviado correctamente (simulado)",
                        EmailsSent = request.Destinatarios.Count
                    };
                }
                else
                {
                    // ⬇️ MODO REAL - ENVÍO POR SMTP
                    _logger.LogInformation("📮 Enviando email REAL por SMTP...");

                    int emailsEnviados = 0;
                    var errores = new List<string>();

                    foreach (var destinatario in request.Destinatarios)
                    {
                        try
                        {
                            await SendEmailViaSMTP(
                                destinatario.Email,
                                destinatario.Name,
                                subject,
                                emailContent
                            );

                            _logger.LogInformation($"✅ Email enviado a: {destinatario.Email}");
                            emailsEnviados++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"❌ Error al enviar a {destinatario.Email}");
                            errores.Add($"Error al enviar a {destinatario.Email}: {ex.Message}");
                        }
                    }

                    _logger.LogInformation($"✅ Emails enviados: {emailsEnviados}/{request.Destinatarios.Count}");

                    return new SendEmailResponseDTO
                    {
                        Success = emailsEnviados > 0,
                        Message = emailsEnviados == request.Destinatarios.Count
                            ? "Emails enviados correctamente"
                            : $"Emails enviados parcialmente: {emailsEnviados}/{request.Destinatarios.Count}",
                        EmailsSent = emailsEnviados,
                        Errors = errores.Count > 0 ? errores : null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al enviar email");

                return new SendEmailResponseDTO
                {
                    Success = false,
                    Message = "Error al enviar email",
                    EmailsSent = 0,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Envía un email usando SMTP (Gmail, Outlook, etc.)
        /// </summary>
        private async Task SendEmailViaSMTP(
    string toEmail,
    string toName,
    string subject,
    string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _emailSettings.SenderName,
                _emailSettings.SenderEmail
            ));

            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;

            // ⬇️ CAMBIAR DE TextBody a HtmlBody
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body,  // ⬅️ CAMBIO AQUÍ
                TextBody = StripHtml(body) // Versión texto plano como fallback
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await client.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.SmtpPort,
                    _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None
                );

                await client.AuthenticateAsync(
                    _emailSettings.Username,
                    _emailSettings.Password
                );

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"📧 Email enviado exitosamente a {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al enviar email a {toEmail}");
                throw;
            }
        }

        // Método auxiliar para remover HTML (fallback texto plano)
        private string StripHtml(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Obtiene el asunto del email según el tipo
        /// </summary>
        private string GetEmailSubject(EmailType tipo)
        {
            return tipo switch
            {
                EmailType.InscripcionConfirmada => "✅ Inscripción Confirmada - ÁgoraCertāmina",
                EmailType.PagoConfirmado => "💳 Pago Confirmado - ÁgoraCertāmina",
                EmailType.RecordatorioCierre => "⏰ Recordatorio: Convocatoria próxima a cerrar",
                EmailType.NuevaInscripcionOrganizacion => "🎉 Nueva inscripción recibida",
                _ => "Notificación - ÁgoraCertāmina"
            };
        }

        public async Task<bool> SendInscripcionConfirmadaAsync(
    ConvocatoriaEmailDTO convocatoria,
    ParticipantEmailDTO participante,
    FormularioEmailDTO formulario)
        {
            // ⬇️ CREAR LISTA DE DESTINATARIOS
            var destinatarios = new List<EmailRecipientDTO>
    {
        // Siempre enviar al participante
        new()
        {
            Email = participante.Email,
            Name = $"{participante.FirstName} {participante.LastName}",
            Role = RecipientRole.Participant
        }
    };

            // ⬇️ SOLO AGREGAR ORGANIZACIÓN SI TIENE EMAIL VÁLIDO
            if (!string.IsNullOrWhiteSpace(convocatoria.Organizacion.Email) &&
                convocatoria.Organizacion.Email != "organizacion@ejemplo.com")
            {
                destinatarios.Add(new EmailRecipientDTO
                {
                    Email = convocatoria.Organizacion.Email,
                    Name = convocatoria.Organizacion.Nombre,
                    Role = RecipientRole.Organization
                });
            }

            var request = new SendEmailRequestDTO
            {
                Tipo = EmailType.InscripcionConfirmada,
                Destinatarios = destinatarios, // ⬅️ USAR LA LISTA FILTRADA
                Convocatoria = convocatoria,
                Participante = participante,
                Formulario = formulario
            };

            var result = await SendEmailAsync(request);
            return result.Success;
        }

        public async Task<bool> SendPagoConfirmadoAsync(
            ConvocatoriaEmailDTO convocatoria,
            ParticipantEmailDTO participante,
            PagoEmailDTO pago)
        {
            var request = new SendEmailRequestDTO
            {
                Tipo = EmailType.PagoConfirmado,
                Destinatarios = new List<EmailRecipientDTO>
                {
                    new() { Email = participante.Email, Name = $"{participante.FirstName} {participante.LastName}", Role = RecipientRole.Participant },
                    new() { Email = convocatoria.Organizacion.Email ?? "organizacion@ejemplo.com", Name = convocatoria.Organizacion.Nombre, Role = RecipientRole.Organization }
                },
                Convocatoria = convocatoria,
                Participante = participante,
                Pago = pago
            };

            var result = await SendEmailAsync(request);
            return result.Success;
        }

        public async Task<bool> SendRecordatorioCierreAsync(
            ConvocatoriaEmailDTO convocatoria,
            ParticipantEmailDTO participante,
            int diasRestantes)
        {
            var request = new SendEmailRequestDTO
            {
                Tipo = EmailType.RecordatorioCierre,
                Destinatarios = new List<EmailRecipientDTO>
                {
                    new() { Email = participante.Email, Name = $"{participante.FirstName} {participante.LastName}", Role = RecipientRole.Participant }
                },
                Convocatoria = convocatoria,
                Participante = participante
            };

            var result = await SendEmailAsync(request);
            return result.Success;
        }

        // ============================================
        // GENERACIÓN DE PLANTILLAS (mismo código anterior)
        // ============================================
        private string GenerateEmailTemplate(SendEmailRequestDTO request)
        {
            return request.Tipo switch
            {
                EmailType.InscripcionConfirmada => GetInscripcionTemplate(request),
                EmailType.PagoConfirmado => GetPagoTemplate(request),
                EmailType.RecordatorioCierre => GetRecordatorioTemplate(request),
                EmailType.NuevaInscripcionOrganizacion => GetNuevaInscripcionOrgTemplate(request),
                _ => "Email sin plantilla"
            };
        }

        private string GetInscripcionTemplate(SendEmailRequestDTO request)
        {
            return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Inscripción Confirmada</title>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f4f4f4; padding: 20px 0;"">
        <tr>
            <td align=""center"">
                <!-- Container principal -->
                <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
                    
                    <!-- Header con degradado -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 40px 20px; text-align: center;"">
                            <h1 style=""color: #ffffff; margin: 0; font-size: 28px; font-weight: 600;"">
                                ✅ Inscripción Confirmada
                            </h1>
                            <p style=""color: #f0f0ff; margin: 10px 0 0 0; font-size: 16px;"">
                                ÁgoraCertāmina
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Contenido -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            
                            <!-- Saludo -->
                            <p style=""color: #333333; font-size: 16px; line-height: 1.6; margin: 0 0 20px 0;"">
                                Hola <strong>{request.Participante.FirstName} {request.Participante.LastName}</strong>,
                            </p>
                            
                            <p style=""color: #555555; font-size: 15px; line-height: 1.6; margin: 0 0 30px 0;"">
                                Tu inscripción a la convocatoria ha sido <strong style=""color: #667eea;"">confirmada exitosamente</strong>.
                            </p>
                            
                            <!-- Datos de la convocatoria -->
                            <div style=""background-color: #f8f9fa; border-left: 4px solid #667eea; padding: 20px; margin-bottom: 30px; border-radius: 4px;"">
                                <h2 style=""color: #667eea; font-size: 18px; margin: 0 0 15px 0;"">
                                    📋 Datos de la Convocatoria
                                </h2>
                                <table width=""100%"" cellpadding=""5"" cellspacing=""0"">
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Título:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            {request.Convocatoria.Titulo}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Organización:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            {request.Convocatoria.Organizacion.Nombre}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Fecha de inicio:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            {request.Convocatoria.FechaInicio:dd/MM/yyyy}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Fecha de cierre:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            {request.Convocatoria.FechaFin:dd/MM/yyyy}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Inscripción:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            {(request.Convocatoria.EsGratuita ? "<span style='color: #28a745; font-weight: bold;'>GRATUITA ✨</span>" : $"<span style='color: #667eea; font-weight: bold;'>${request.Convocatoria.CostoInscripcion:N2}</span>")}
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            
                            <!-- Estado del formulario -->
                            <div style=""background-color: #e8f5e9; border-left: 4px solid #28a745; padding: 20px; margin-bottom: 30px; border-radius: 4px;"">
                                <h2 style=""color: #28a745; font-size: 18px; margin: 0 0 15px 0;"">
                                    📝 Estado del Formulario
                                </h2>
                                <table width=""100%"" cellpadding=""5"" cellspacing=""0"">
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>ID de respuesta:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0; font-family: monospace;"">
                                            {request.Formulario?.ResponseId}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Estado:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            <span style=""background-color: #28a745; color: white; padding: 4px 12px; border-radius: 12px; font-size: 12px; font-weight: bold;"">
                                                {request.Formulario?.Estado.ToUpper()}
                                            </span>
                                        </td>
                                    </tr>
                                    {(request.Formulario?.FechaEnvio.HasValue == true ? $@"
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Fecha de envío:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            {request.Formulario.FechaEnvio:dd/MM/yyyy}
                                        </td>
                                    </tr>
                                    " : "")}
                                    {(request.Formulario?.PorcentajeCompletado.HasValue == true ? $@"
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Completado:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            <strong style=""color: #28a745;"">{request.Formulario.PorcentajeCompletado}%</strong>
                                        </td>
                                    </tr>
                                    " : "")}
                                </table>
                            </div>
                            
                            <!-- Mensaje de agradecimiento -->
                            <div style=""text-align: center; padding: 20px 0; border-top: 1px solid #e0e0e0; border-bottom: 1px solid #e0e0e0; margin: 20px 0;"">
                                <p style=""color: #667eea; font-size: 18px; font-weight: 600; margin: 0;"">
                                    ¡Gracias por tu participación! 🎉
                                </p>
                            </div>
                            
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f8f9fa; padding: 30px; text-align: center; border-top: 1px solid #e0e0e0;"">
                            <p style=""color: #666666; font-size: 14px; margin: 0 0 10px 0;"">
                                Equipo de <strong>{request.Convocatoria.Organizacion.Nombre}</strong>
                            </p>
                            <p style=""color: #667eea; font-size: 16px; font-weight: 600; margin: 0;"">
                                ÁgoraCertāmina
                            </p>
                            <p style=""color: #999999; font-size: 12px; margin: 15px 0 0 0;"">
                                Este es un correo automático, por favor no respondas a este mensaje.
                            </p>
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";
        }

        private string GetPagoTemplate(SendEmailRequestDTO request)
        {
            return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Pago Confirmado</title>
</head>
<body style=""margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f4f4f4; padding: 20px 0;"">
        <tr>
            <td align=""center"">
                <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
                    
                    <!-- Header -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #28a745 0%, #20c997 100%); padding: 40px 20px; text-align: center;"">
                            <h1 style=""color: #ffffff; margin: 0; font-size: 28px; font-weight: 600;"">
                                💳 Pago Confirmado
                            </h1>
                            <p style=""color: #f0fff4; margin: 10px 0 0 0; font-size: 16px;"">
                                ÁgoraCertāmina
                            </p>
                        </td>
                    </tr>
                    
                    <!-- Contenido -->
                    <tr>
                        <td style=""padding: 40px 30px;"">
                            
                            <p style=""color: #333333; font-size: 16px; line-height: 1.6; margin: 0 0 20px 0;"">
                                Hola <strong>{request.Participante.FirstName} {request.Participante.LastName}</strong>,
                            </p>
                            
                            <p style=""color: #555555; font-size: 15px; line-height: 1.6; margin: 0 0 30px 0;"">
                                Tu pago ha sido <strong style=""color: #28a745;"">procesado exitosamente</strong>.
                            </p>
                            
                            <!-- Detalles del pago -->
                            <div style=""background-color: #f8f9fa; border-left: 4px solid #28a745; padding: 20px; margin-bottom: 30px; border-radius: 4px;"">
                                <h2 style=""color: #28a745; font-size: 18px; margin: 0 0 15px 0;"">
                                    💰 Detalles del Pago
                                </h2>
                                <table width=""100%"" cellpadding=""5"" cellspacing=""0"">
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Monto:</strong>
                                        </td>
                                        <td style=""color: #28a745; font-size: 18px; font-weight: bold; padding: 5px 0;"">
                                            ${request.Pago?.Monto:N2}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Método de pago:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            {request.Pago?.MetodoPago}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>ID de transacción:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0; font-family: monospace;"">
                                            {request.Pago?.TransaccionId}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Fecha:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            {request.Pago?.Fecha:dd/MM/yyyy HH:mm}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""color: #666666; font-size: 14px; padding: 5px 0;"">
                                            <strong>Estado:</strong>
                                        </td>
                                        <td style=""color: #333333; font-size: 14px; padding: 5px 0;"">
                                            <span style=""background-color: #28a745; color: white; padding: 4px 12px; border-radius: 12px; font-size: 12px; font-weight: bold;"">
                                                {request.Pago?.Estado.ToUpper()} ✅
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            
                            <!-- Convocatoria -->
                            <div style=""background-color: #f0f0ff; border-left: 4px solid #667eea; padding: 20px; margin-bottom: 30px; border-radius: 4px;"">
                                <h2 style=""color: #667eea; font-size: 18px; margin: 0 0 10px 0;"">
                                    📋 Convocatoria
                                </h2>
                                <p style=""color: #333333; font-size: 15px; margin: 0;"">
                                    <strong>{request.Convocatoria.Titulo}</strong>
                                </p>
                                <p style=""color: #666666; font-size: 14px; margin: 5px 0 0 0;"">
                                    {request.Convocatoria.Organizacion.Nombre}
                                </p>
                            </div>
                            
                            <div style=""text-align: center; padding: 20px 0; border-top: 1px solid #e0e0e0; margin: 20px 0;"">
                                <p style=""color: #28a745; font-size: 18px; font-weight: 600; margin: 0;"">
                                    ¡Gracias por tu pago! 💚
                                </p>
                                <p style=""color: #666666; font-size: 14px; margin: 10px 0 0 0;"">
                                    Este es tu comprobante de pago. Consérvalo para tus registros.
                                </p>
                            </div>
                            
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td style=""background-color: #f8f9fa; padding: 30px; text-align: center; border-top: 1px solid #e0e0e0;"">
                            <p style=""color: #666666; font-size: 14px; margin: 0 0 10px 0;"">
                                Equipo de <strong>{request.Convocatoria.Organizacion.Nombre}</strong>
                            </p>
                            <p style=""color: #667eea; font-size: 16px; font-weight: 600; margin: 0;"">
                                ÁgoraCertāmina
                            </p>
                            <p style=""color: #999999; font-size: 12px; margin: 15px 0 0 0;"">
                                Este es un correo automático, por favor no respondas a este mensaje.
                            </p>
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
";
        }

        private string GetRecordatorioTemplate(SendEmailRequestDTO request)
        {
            var diasRestantes = (request.Convocatoria.FechaFin - DateTime.Now).Days;

            var sb = new StringBuilder();
            sb.AppendLine("╔════════════════════════════════════════════════════════════╗");
            sb.AppendLine("║          ⏰ RECORDATORIO: CIERRE PRÓXIMO                  ║");
            sb.AppendLine("╚════════════════════════════════════════════════════════════╝");
            sb.AppendLine();
            sb.AppendLine($"Hola {request.Participante.FirstName} {request.Participante.LastName},");
            sb.AppendLine();
            sb.AppendLine($"La convocatoria \"{request.Convocatoria.Titulo}\" está próxima a cerrar.");
            sb.AppendLine();
            sb.AppendLine($"⚠️ QUEDAN SOLO {diasRestantes} {(diasRestantes == 1 ? "DÍA" : "DÍAS")}");
            sb.AppendLine();
            sb.AppendLine($"📅 Fecha de cierre: {request.Convocatoria.FechaFin:dddd, dd 'de' MMMM 'de' yyyy}");
            sb.AppendLine();
            sb.AppendLine("🔔 No olvides completar tu inscripción antes de que termine el plazo.");
            sb.AppendLine();
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine();
            sb.AppendLine("Saludos,");
            sb.AppendLine();
            sb.AppendLine($"Equipo de {request.Convocatoria.Organizacion.Nombre}");
            sb.AppendLine("ÁgoraCertāmina");

            return sb.ToString();
        }

        private string GetNuevaInscripcionOrgTemplate(SendEmailRequestDTO request)
        {
            var sb = new StringBuilder();
            sb.AppendLine("╔════════════════════════════════════════════════════════════╗");
            sb.AppendLine("║          🎉 NUEVA INSCRIPCIÓN RECIBIDA                    ║");
            sb.AppendLine("╚════════════════════════════════════════════════════════════╝");
            sb.AppendLine();
            sb.AppendLine($"Hola {request.Convocatoria.Organizacion.Nombre},");
            sb.AppendLine();
            sb.AppendLine("Se ha recibido una nueva inscripción para tu convocatoria.");
            sb.AppendLine();
            sb.AppendLine("👤 DATOS DEL PARTICIPANTE:");
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine($"• Nombre: {request.Participante.FirstName} {request.Participante.LastName}");
            sb.AppendLine($"• Email: {request.Participante.Email}");
            if (!string.IsNullOrEmpty(request.Participante.Phone))
                sb.AppendLine($"• Teléfono: {request.Participante.Phone}");
            sb.AppendLine();
            sb.AppendLine("📋 CONVOCATORIA:");
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine($"• {request.Convocatoria.Titulo}");
            sb.AppendLine($"• Fecha de inscripción: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine();
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine();
            sb.AppendLine("Puedes revisar los detalles completos en tu panel de administración.");
            sb.AppendLine();
            sb.AppendLine("ÁgoraCertāmina");

            return sb.ToString();
        }
    }
}