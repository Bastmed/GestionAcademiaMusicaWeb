using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Profesores
{
    public class EditarModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public EditarModel(DatabaseHelper db)
        {
            _db = db;
        }

        [BindProperty]
        public Profesor Profesor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Profesor = await _db.GetProfesorById(id);
            if (Profesor == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _db.UpdateProfesor(Profesor);
            return RedirectToPage("./Index");
        }
    }
}