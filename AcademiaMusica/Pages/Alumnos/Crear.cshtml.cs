using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcademiaMusica.Data;
using AcademiaMusica.Models;

namespace AcademiaMusica.Pages.Alumnos
{
    public class CrearModel : PageModel
    {
        private readonly DatabaseHelper _databaseHelper;

        public CrearModel(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [BindProperty]
        public Alumno Alumno { get; set; }

        public IActionResult OnGet()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol == "Alumno")
                return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol == "Alumno")
                return RedirectToPage("/Index");

            await _databaseHelper.InsertAlumnos(
                Alumno.NombreAlumno,
                Alumno.ApellidoAlumno,
                Alumno.FechaNacimiento,
                Alumno.TelefonoAlumno,
                Alumno.EmailAlumno,
                Alumno.ActivoAlumno,
                Alumno.IdInstrumento
            );

            return RedirectToPage("./Index");
        }
    }
}