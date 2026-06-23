using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Instrumentos
{
    public class CrearModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public CrearModel(DatabaseHelper db)
        {
            _db = db;
        }

        [BindProperty]
        public Instrumento Instrumento { get; set; }

        public IActionResult OnGet()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (string.IsNullOrEmpty(rol))
                return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var rol = HttpContext.Session.GetString("rol");
            if (string.IsNullOrEmpty(rol))
                return RedirectToPage("/Index");

            if (!ModelState.IsValid) return Page();
            await _db.InsertInstrumento(Instrumento);
            return RedirectToPage("./Index");
        }
    }
}