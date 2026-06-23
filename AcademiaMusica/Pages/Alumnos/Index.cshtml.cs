using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Alumnos
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public IndexModel(DatabaseHelper db)
        {
            _db = db;
        }

        public IList<Alumno> Alumnos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol == "Alumno")
                return RedirectToPage("/Index");

            Alumnos = await _db.GetAlumnos();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol == "Alumno")
                return RedirectToPage("/Index");

            await _db.DeleteAlumno(id);
            return RedirectToPage();
        }
    }
}