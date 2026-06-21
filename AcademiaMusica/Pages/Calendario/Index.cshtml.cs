using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Calendario
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public IndexModel(DatabaseHelper db)
        {
            _db = db;
        }

        public string Rol { get; set; } = "";
        public int IdReferencia { get; set; }
        public string NombreUsuario { get; set; } = "";

        public IActionResult OnGet()
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (usuario == null)
                return RedirectToPage("/Login/Login");

            Rol = HttpContext.Session.GetString("rol") ?? "";
            IdReferencia = HttpContext.Session.GetInt32("idReferencia") ?? 0;
            NombreUsuario = usuario;

            return Page();
        }

        public async Task<JsonResult> OnGetEventosAsync()
        {
            var rol = HttpContext.Session.GetString("rol") ?? "";
            var idReferencia = HttpContext.Session.GetInt32("idReferencia") ?? 0;

            var eventos = new List<object>();

            if (rol == "Profesor")
            {
                var bloques = await _db.GetDisponibilidadPorProfesor(idReferencia);
                foreach (var b in bloques)
                {
                    eventos.Add(new
                    {
                        id = b.IdDisponibilidad,
                        title = b.Reservado ? "Reservado" : "Disponible",
                        start = b.FechaInicio.ToString("s"),
                        end = b.FechaFin.ToString("s"),
                        color = b.Reservado ? "#e53935" : "#2ecc71"
                    });
                }
            }
            else if (rol == "Alumno")
            {
                var bloques = await _db.GetDisponibilidadGeneral();
                foreach (var b in bloques)
                {
                    eventos.Add(new
                    {
                        id = b.IdDisponibilidad,
                        title = $"{b.NombreProfesor}",
                        start = b.FechaInicio.ToString("s"),
                        end = b.FechaFin.ToString("s"),
                        color = "#7c4dff",
                        extendedProps = new { idProfesor = b.IdProfesor }
                    });
                }
            }

            return new JsonResult(eventos);
        }

        public async Task<JsonResult> OnPostCrearBloqueAsync([FromBody] BloqueInput input)
        {
            var rol = HttpContext.Session.GetString("rol") ?? "";
            var idReferencia = HttpContext.Session.GetInt32("idReferencia") ?? 0;

            if (rol != "Profesor")
                return new JsonResult(new { ok = false, mensaje = "Solo los profesores pueden crear bloques." });

            await _db.InsertDisponibilidad(idReferencia, input.Inicio, input.Fin);
            return new JsonResult(new { ok = true });
        }

        public async Task<JsonResult> OnPostSolicitarAsync([FromBody] SolicitarInput input)
        {
            var rol = HttpContext.Session.GetString("rol") ?? "";
            var idAlumno = HttpContext.Session.GetInt32("idReferencia") ?? 0;

            if (rol != "Alumno")
                return new JsonResult(new { ok = false, mensaje = "Solo los alumnos pueden solicitar clases." });

            await _db.InsertSolicitud(input.IdDisponibilidad, idAlumno, input.IdProfesor);
            return new JsonResult(new { ok = true, mensaje = "Solicitud enviada. Espera la confirmación del profesor." });
        }

        public class BloqueInput
        {
            public DateTime Inicio { get; set; }
            public DateTime Fin { get; set; }
        }

        public class SolicitarInput
        {
            public int IdDisponibilidad { get; set; }
            public int IdProfesor { get; set; }
        }
    }
}