using System;

namespace AcademiaMusica.Models
{
    public class Alumno
    {
        public int IdAlumno { get; set; }
        public string NombreAlumno { get; set; }
        public string ApellidoAlumno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string TelefonoAlumno { get; set; }
        public string EmailAlumno { get; set; }
        public bool ActivoAlumno { get; set; }
        public int? IdInstrumento { get; set; }

        public Alumno()
        {
            ActivoAlumno = true;
        }
    }
}
