using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Alumnos
{
    public class EditarModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public EditarModel(DatabaseHelper db)
        {
            _db = db;
        }

        [BindProperty]
        public Alumno Alumno { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol == "Alumno")
                return RedirectToPage("/Index");

            Alumno = await _db.GetAlumnoById(id);
            if (Alumno == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol == "Alumno")
                return RedirectToPage("/Index");

            if (!ModelState.IsValid) return Page();
            await _db.UpdateAlumno(Alumno);
            return RedirectToPage("./Index");
        }
    }
}