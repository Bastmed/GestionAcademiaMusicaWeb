using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Instrumentos
{
    public class EditarModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public EditarModel(DatabaseHelper db)
        {
            _db = db;
        }

        [BindProperty]
        public Instrumento Instrumento { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Instrumento = await _db.GetInstrumentoById(id);
            if (Instrumento == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _db.UpdateInstrumento(Instrumento);
            return RedirectToPage("./Index");
        }
    }
}