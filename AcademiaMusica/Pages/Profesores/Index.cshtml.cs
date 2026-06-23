using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Profesores
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public IndexModel(DatabaseHelper db)
        {
            _db = db;
        }

        public IList<Profesor> Profesores { get; set; }
        public bool PuedeModificar { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol == "Alumno")
                return RedirectToPage("/Index");

            PuedeModificar = rol == "Admin";
            Profesores = await _db.GetProfesores();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol != "Admin")
                return RedirectToPage("/Index");

            await _db.DeleteProfesor(id);
            return RedirectToPage();
        }
    }
}