using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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
            // Protege la página: solo usuarios logeados
            var usuario = HttpContext.Session.GetString("usuario");
            if (usuario == null)
                return RedirectToPage("/Login/Login");

            Rol = HttpContext.Session.GetString("rol") ?? "";
            IdReferencia = HttpContext.Session.GetInt32("idReferencia") ?? 0;
            NombreUsuario = usuario;

            return Page();
        }

        // ── ENDPOINT: trae los eventos para FullCalendar (formato JSON) ──
        // Si es Profesor: trae SU PROPIA disponibilidad (incluye reservados)
        // Si es Alumno: trae la disponibilidad GENERAL libre de todos los profesores
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

        // ── ENDPOINT: el PROFESOR crea un nuevo bloque de disponibilidad ──
        public async Task<JsonResult> OnPostCrearBloqueAsync([FromBody] BloqueInput input)
        {
            var rol = HttpContext.Session.GetString("rol") ?? "";
            var idReferencia = HttpContext.Session.GetInt32("idReferencia") ?? 0;

            if (rol != "Profesor")
                return new JsonResult(new { ok = false, mensaje = "Solo los profesores pueden crear bloques." });

            await _db.InsertDisponibilidad(idReferencia, input.Inicio, input.Fin);
            return new JsonResult(new { ok = true });
        }

        // ── ENDPOINT: el ALUMNO solicita una clase en un bloque ──
        public async Task<JsonResult> OnPostSolicitarAsync([FromBody] SolicitarInput input)
        {
            var rol = HttpContext.Session.GetString("rol") ?? "";
            var idAlumno = HttpContext.Session.GetInt32("idReferencia") ?? 0;

            if (rol != "Alumno")
                return new JsonResult(new { ok = false, mensaje = "Solo los alumnos pueden solicitar clases." });

            // Insertar solicitud y obtener datos para el correo
            var solicitud = await _db.InsertSolicitudYObtenerDatos(input.IdDisponibilidad, idAlumno, input.IdProfesor);

            // Enviar correo al profesor
            if (solicitud != null && !string.IsNullOrEmpty(solicitud.EmailProfesor) &&
                solicitud.FechaInicio.HasValue && solicitud.FechaFin.HasValue)
            {
                try
                {
                    var emailSvc = new AcademiaMusica.Services.EmailService();
                    await emailSvc.NotificarProfesorSolicitud(
                        solicitud.EmailProfesor,
                        solicitud.NombreProfesor ?? "",
                        solicitud.NombreAlumno ?? "",
                        solicitud.FechaInicio.Value,
                        solicitud.FechaFin.Value
                    );
                }
                catch { /* No interrumpir si el correo falla */ }
            }

            return new JsonResult(new { ok = true, mensaje = "Solicitud enviada. Espera la confirmación del profesor." });
        }

        // ── ENDPOINT: genera y descarga el comprobante PDF ──
        public async Task<IActionResult> OnPostComprobantePdfAsync([FromBody] SolicitarInput input)
        {
            var rol = HttpContext.Session.GetString("rol") ?? "";
            var idAlumno = HttpContext.Session.GetInt32("idReferencia") ?? 0;

            if (rol != "Alumno")
                return BadRequest();

            // Obtener datos completos de la solicitud
            var solicitud = await _db.InsertSolicitudYObtenerDatos(input.IdDisponibilidad, idAlumno, input.IdProfesor);
            if (solicitud == null) return BadRequest();

            // Enviar correo al profesor
            if (!string.IsNullOrEmpty(solicitud.EmailProfesor) &&
                solicitud.FechaInicio.HasValue && solicitud.FechaFin.HasValue)
            {
                try
                {
                    var emailSvc = new AcademiaMusica.Services.EmailService();
                    await emailSvc.NotificarProfesorSolicitud(
                        solicitud.EmailProfesor,
                        solicitud.NombreProfesor ?? "",
                        solicitud.NombreAlumno ?? "",
                        solicitud.FechaInicio.Value,
                        solicitud.FechaFin.Value
                    );
                }
                catch { }
            }

            // Configurar licencia QuestPDF (community es gratis)
            QuestPDF.Settings.License = LicenseType.Community;

            // Generar PDF
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(content => ComposeContent(content, solicitud));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Academia Konge Egg — Sistema de Gestión Musical | ");
                        x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    });
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", $"Comprobante_Solicitud_{solicitud.IdSolicitud}.pdf");
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("ACADEMIA KONGE EGG")
                        .FontSize(22).Bold().FontColor("#7c4dff");
                    col.Item().Text("Sistema de Gestión Musical")
                        .FontSize(11).FontColor("#888888");
                });

                row.ConstantItem(100).Height(60).Placeholder();
            });

            container.PaddingTop(8).LineHorizontal(2).LineColor("#7c4dff");
        }

        private void ComposeContent(IContainer container, SolicitudClase solicitud)
        {
            container.PaddingTop(24).Column(col =>
            {
                // Título
                col.Item().Text("COMPROBANTE DE SOLICITUD DE CLASE")
                    .FontSize(16).Bold().FontColor("#333333");

                col.Item().PaddingTop(4).Text($"N° de Solicitud: #{solicitud.IdSolicitud}")
                    .FontSize(11).FontColor("#888888");

                col.Item().PaddingTop(24).Text("Estado de la Solicitud")
                    .FontSize(13).Bold();

                col.Item().PaddingTop(6).Background("#fff3cd").Padding(12).Text("⏳  PENDIENTE — Esperando confirmación del profesor")
                    .FontSize(12).FontColor("#856404");

                col.Item().PaddingTop(28).Text("Datos de la Clase").FontSize(13).Bold();
                col.Item().PaddingTop(6).LineHorizontal(1).LineColor("#dddddd");

                col.Item().PaddingTop(12).Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(160);
                        cols.RelativeColumn();
                    });

                    void Fila(string label, string valor)
                    {
                        table.Cell().PaddingVertical(8).PaddingRight(12)
                            .Text(label).Bold().FontColor("#555555");
                        table.Cell().PaddingVertical(8)
                            .Text(valor).FontColor("#222222");
                    }

                    Fila("Alumno:", solicitud.NombreAlumno ?? "—");
                    Fila("Profesor:", solicitud.NombreProfesor ?? "—");
                    Fila("Fecha:", solicitud.FechaInicio?.ToString("dddd, dd 'de' MMMM 'de' yyyy",
                        new System.Globalization.CultureInfo("es-CL")) ?? "—");
                    Fila("Horario:", $"{solicitud.FechaInicio?.ToString("HH:mm")} — {solicitud.FechaFin?.ToString("HH:mm")}");
                    Fila("Solicitado el:", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                });

                col.Item().PaddingTop(32).Background("#f0f0f0").Padding(16).Column(nota =>
                {
                    nota.Item().Text("Nota importante").Bold().FontSize(11);
                    nota.Item().PaddingTop(4).Text(
                        "Este comprobante confirma que tu solicitud fue enviada al profesor. " +
                        "El profesor debe aceptar o rechazar la solicitud. " +
                        "Revisa el estado en la sección 'Mis Solicitudes' del sistema.")
                        .FontSize(11).FontColor("#555555");
                });
            });
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
