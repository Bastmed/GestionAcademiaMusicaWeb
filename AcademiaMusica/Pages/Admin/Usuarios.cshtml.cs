using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Admin
{
    public class UsuariosModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public UsuariosModel(DatabaseHelper db)
        {
            _db = db;
        }

        public List<Usuario> Usuarios { get; set; } = new();
        public List<Profesor> Profesores { get; set; } = new();
        public List<Alumno> Alumnos { get; set; } = new();
        public string Mensaje { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            // Solo Admin puede entrar
            var rol = HttpContext.Session.GetString("rol");
            if (rol != "Admin")
                return RedirectToPage("/Index");

            await CargarDatos();
            return Page();
        }

        public async Task<IActionResult> OnPostAsignarRolAsync(int idUsuario, string rol, int? idReferencia)
        {
            var rolSesion = HttpContext.Session.GetString("rol");
            if (rolSesion != "Admin")
                return RedirectToPage("/Index");

            // Si el rol no requiere referencia, limpiarla
            if (rol == "Admin" || string.IsNullOrEmpty(rol))
                idReferencia = null;

            await _db.AsignarRol(idUsuario, rol, idReferencia);

            await CargarDatos();
            Mensaje = "Rol actualizado correctamente.";
            return Page();
        }

        private async Task CargarDatos()
        {
            Usuarios = await _db.GetUsuarios();
            Profesores = await _db.GetProfesores();
            Alumnos = await _db.GetAlumnos();
        }
    }
}
