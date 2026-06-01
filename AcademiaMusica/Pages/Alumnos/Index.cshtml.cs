using AcademiaMusica.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcademiaMusica.Models;


namespace AcademiaMusica.Pages.Alumnos
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseHelper _databaseHelper;
        public IndexModel(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        public IList<Alumno> Alumnos { get; set; }
        public async Task OnGetAsync()
        {
            Alumnos = await _databaseHelper.GetAlumnos();
        }
    }
}
