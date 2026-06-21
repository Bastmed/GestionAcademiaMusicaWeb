using System;

namespace AcademiaMusica.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ContraseñaUsuario { get; set; }

        public string? Rol { get; set; }
        public int? IdReferencia { get; set; }
    }
}
