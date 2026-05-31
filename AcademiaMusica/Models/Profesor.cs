using System;

namespace AcademiaMusica.Models
{
    public class Profesor
    {
        public int IdProfesor { get; set; }
        public string NombreProfesor { get; set; }
        public string ApellidoProfesor { get; set; }
        public string TelefonoProfesor { get; set; }
        public string EmailProfesor { get; set; }
        public string Especialidad { get; set; }
        public decimal TarifaHora { get; set; }
        public decimal TarifaConIVA => Math.Round(TarifaHora * 1.19m, 2);
        public bool ActivoProfesor { get; set; }

        public Profesor()
        {
            ActivoProfesor = true;
        }
    }
}
