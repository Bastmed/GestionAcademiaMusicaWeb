using System.Net;
using System.Net.Mail;

namespace AcademiaMusica.Services
{
    public class EmailService
    {
        private readonly string _host = "sandbox.smtp.mailtrap.io";
        private readonly int _port = 587;
        private readonly string _username = "27c300cdb2e4fa";
        private readonly string _password = "21001acdb3541b";
        private readonly string _remitente = "academia@kongegg.com";

        private async Task EnviarCorreo(string destinatario, string asunto, string cuerpoHtml)
        {
            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = true
            };

            var mensaje = new MailMessage
            {
                From = new MailAddress(_remitente, "Academia Konge Egg"),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };

            mensaje.To.Add(destinatario);
            await client.SendMailAsync(mensaje);
        }

        public async Task NotificarProfesorSolicitud(
            string emailProfesor,
            string nombreProfesor,
            string nombreAlumno,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            var asunto = "📅 Nueva solicitud de clase — Academia Konge Egg";
            var cuerpo = $@"
                <div style='font-family:Arial,sans-serif;max-width:600px;margin:0 auto;'>
                    <div style='background:#1a1d2e;padding:24px;text-align:center;'>
                        <h1 style='color:#7c4dff;font-size:22px;margin:0;'>🎵 Academia Konge Egg</h1>
                    </div>
                    <div style='padding:32px;background:#f9f9f9;'>
                        <h2 style='color:#333;'>Hola, {nombreProfesor}</h2>
                        <p style='color:#555;font-size:15px;'>
                            El alumno <strong>{nombreAlumno}</strong> ha solicitado una clase contigo.
                        </p>
                        <div style='background:#fff;border:1px solid #e0e0e0;border-radius:8px;padding:20px;margin:20px 0;'>
                            <p style='margin:0;color:#333;'><strong>📅 Fecha:</strong> {fechaInicio:dd/MM/yyyy}</p>
                            <p style='margin:8px 0 0;color:#333;'><strong>🕐 Horario:</strong> {fechaInicio:HH:mm} — {fechaFin:HH:mm}</p>
                        </div>
                        <p style='color:#555;font-size:14px;'>
                            Ingresa al sistema para <strong>aceptar o rechazar</strong> esta solicitud.
                        </p>
                        <a href='https://localhost:7253/Calendario/Solicitudes'
                           style='display:inline-block;background:#7c4dff;color:#fff;padding:12px 28px;border-radius:6px;text-decoration:none;font-weight:bold;margin-top:8px;'>
                            Ver Solicitudes
                        </a>
                    </div>
                    <div style='background:#eee;padding:12px;text-align:center;font-size:12px;color:#999;'>
                        Academia Konge Egg — Sistema de Gestión Musical
                    </div>
                </div>";

            await EnviarCorreo(emailProfesor, asunto, cuerpo);
        }

        public async Task NotificarAlumnoRespuesta(
            string emailAlumno,
            string nombreAlumno,
            string nombreProfesor,
            DateTime fechaInicio,
            DateTime fechaFin,
            bool aceptada)
        {
            var estado = aceptada ? "✅ Aceptada" : "❌ Rechazada";
            var color = aceptada ? "#2ecc71" : "#e53935";
            var mensaje = aceptada
                ? "Tu solicitud fue <strong>aceptada</strong>. ¡Nos vemos en clase!"
                : "Tu solicitud fue <strong>rechazada</strong>. Puedes buscar otro horario disponible.";

            var asunto = $"Solicitud de clase {estado} — Academia Konge Egg";
            var cuerpo = $@"
                <div style='font-family:Arial,sans-serif;max-width:600px;margin:0 auto;'>
                    <div style='background:#1a1d2e;padding:24px;text-align:center;'>
                        <h1 style='color:#7c4dff;font-size:22px;margin:0;'>🎵 Academia Konge Egg</h1>
                    </div>
                    <div style='padding:32px;background:#f9f9f9;'>
                        <h2 style='color:#333;'>Hola, {nombreAlumno}</h2>
                        <div style='background:{color};border-radius:8px;padding:14px;margin-bottom:20px;text-align:center;'>
                            <span style='color:#fff;font-size:18px;font-weight:bold;'>{estado}</span>
                        </div>
                        <p style='color:#555;font-size:15px;'>{mensaje}</p>
                        <div style='background:#fff;border:1px solid #e0e0e0;border-radius:8px;padding:20px;margin:20px 0;'>
                            <p style='margin:0;color:#333;'><strong>👨‍🏫 Profesor:</strong> {nombreProfesor}</p>
                            <p style='margin:8px 0 0;color:#333;'><strong>📅 Fecha:</strong> {fechaInicio:dd/MM/yyyy}</p>
                            <p style='margin:8px 0 0;color:#333;'><strong>🕐 Horario:</strong> {fechaInicio:HH:mm} — {fechaFin:HH:mm}</p>
                        </div>
                        <a href='https://localhost:7253/Calendario/Solicitudes'
                           style='display:inline-block;background:#7c4dff;color:#fff;padding:12px 28px;border-radius:6px;text-decoration:none;font-weight:bold;margin-top:8px;'>
                            Ver Mis Solicitudes
                        </a>
                    </div>
                    <div style='background:#eee;padding:12px;text-align:center;font-size:12px;color:#999;'>
                        Academia Konge Egg — Sistema de Gestión Musical
                    </div>
                </div>";

            await EnviarCorreo(emailAlumno, asunto, cuerpo);
        }
    }
}
