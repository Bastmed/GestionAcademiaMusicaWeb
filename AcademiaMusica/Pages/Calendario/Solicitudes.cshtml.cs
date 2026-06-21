using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Calendario
{
    public class SolicitudesModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public SolicitudesModel(DatabaseHelper db)
        {
            _db = db;
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
            {
                Solicitudes = await _db.GetSolicitudesPorProfesor(idReferencia);
            }
            else if (Rol == "Alumno")
            {
                Solicitudes = await _db.GetSolicitudesPorAlumno(idReferencia);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostResponderAsync(int idSolicitud, bool aceptar)
        {
            var rol = HttpContext.Session.GetString("rol") ?? "";
            if (rol == "Profesor")
            {
                await _db.ResponderSolicitud(idSolicitud, aceptar);
            }
            return RedirectToPage();
        }
    }
}