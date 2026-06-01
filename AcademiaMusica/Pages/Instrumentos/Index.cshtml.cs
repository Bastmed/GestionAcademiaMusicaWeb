using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Instrumentos
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public IndexModel(DatabaseHelper db)
        {
            _db = db;
        }

        public IList<Instrumento> Instrumentos { get; set; }

        public async Task OnGetAsync()
        {
            Instrumentos = await _db.GetInstrumentos();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            await _db.DeleteInstrumento(id);
            return RedirectToPage();
        }
    }
}