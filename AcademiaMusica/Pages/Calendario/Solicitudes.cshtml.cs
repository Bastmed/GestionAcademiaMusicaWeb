using AcademiaMusica.Data;
using AcademiaMusica.Models;
using AcademiaMusica.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Calendario
{
    public class SolicitudesModel : PageModel
    {
        private readonly DatabaseHelper _db;
        private readonly EmailService _email;

        public SolicitudesModel(DatabaseHelper db)
        {
            _db = db;
            _email = new EmailService();
        }

        public string Rol { get; set; } = "";
        public List<SolicitudClase> Solicitudes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (usuario == null)
                return RedirectToPage("/Login/Login");

            Rol = HttpContext.Session.GetString("rol") ?? "";
            var idReferencia = HttpContext.Session.GetInt32("idReferencia") ?? 0;

            if (Rol == "Profesor")
                Solicitudes = await _db.GetSolicitudesPorProfesor(idReferencia);
            else if (Rol == "Alumno")
                Solicitudes = await _db.GetSolicitudesPorAlumno(idReferencia);

            return Page();
        }

        public async Task<IActionResult> OnPostResponderAsync(int idSolicitud, bool aceptar)
        {
            var rol = HttpContext.Session.GetString("rol") ?? "";
            if (rol != "Profesor")
                return RedirectToPage();

            // Obtener datos ANTES de responder para el correo
            var solicitud = await _db.GetSolicitudById(idSolicitud);

            // Responder en la BD
            await _db.ResponderSolicitud(idSolicitud, aceptar);

            // Enviar correo al alumno
            if (solicitud != null &&
                !string.IsNullOrEmpty(solicitud.EmailAlumno) &&
                solicitud.FechaInicio.HasValue &&
                solicitud.FechaFin.HasValue)
            {
                try
                {
                    await _email.NotificarAlumnoRespuesta(
                        solicitud.EmailAlumno,
                        solicitud.NombreAlumno ?? "",
                        solicitud.NombreProfesor ?? "",
                        solicitud.FechaInicio.Value,
                        solicitud.FechaFin.Value,
                        aceptar
                    );
                }
                catch { }
            }

            return RedirectToPage();
        }
    }
}