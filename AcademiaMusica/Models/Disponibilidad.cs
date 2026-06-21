using System;

namespace AcademiaMusica.Models
{
    public class Disponibilidad
    {
        public int IdDisponibilidad { get; set; }
        public int IdProfesor { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Reservado { get; set; }
        public string? NombreProfesor { get; set; }
    }
}
