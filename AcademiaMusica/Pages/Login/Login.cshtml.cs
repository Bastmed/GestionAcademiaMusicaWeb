using AcademiaMusica.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public LoginModel(DatabaseHelper db)
        {
            _db = db;
        }

        [BindProperty]
        public string NombreUsuario { get; set; }

        [BindProperty]
        public string Contrasena { get; set; }

        public string MensajeError { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("usuario") != null)
                return RedirectToPage("/Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var usuario = await _db.GetUsuario(NombreUsuario, Contrasena);

            if (usuario != null)
            {
                HttpContext.Session.SetString("usuario", usuario.NombreUsuario);
                return RedirectToPage("/Index");
            }

            MensajeError = "Usuario o contraseña incorrectos.";
            return Page();
        }
    }
}