using System;

namespace AcademiaMusica.Models
{
    public class SolicitudClase
    {
        public int IdSolicitud { get; set; }
        public int IdDisponibilidad { get; set; }
        public int IdAlumno { get; set; }
        public int IdProfesor { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaSolicitud { get; set; }

        public string? NombreAlumno { get; set; }
        public string? NombreProfesor { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public string? EmailAlumno { get; set; }
        public string? EmailProfesor { get; set; }
    }
}
