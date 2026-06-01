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

        public async Task OnGetAsync()
        {
            Profesores = await _db.GetProfesores();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            await _db.DeleteProfesor(id);
            return RedirectToPage();
        }
    }
}
