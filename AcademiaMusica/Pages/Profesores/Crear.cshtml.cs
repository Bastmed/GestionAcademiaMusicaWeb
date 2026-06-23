using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Profesores
{
    public class CrearModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public CrearModel(DatabaseHelper db)
        {
            _db = db;
        }

        [BindProperty]
        public Profesor Profesor { get; set; }

        public IActionResult OnGet()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol != "Admin")
                return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (rol != "Admin")
                return RedirectToPage("/Index");

            if (!ModelState.IsValid) return Page();
            await _db.InsertProfesor(Profesor);
            return RedirectToPage("./Index");
        }
    }
}