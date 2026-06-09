using AcademiaMusica.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademiaMusica.Pages.Login
{
    public class RegistroModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public RegistroModel(DatabaseHelper db)
        {
            _db = db;
        }

        [BindProperty]
        public string NombreUsuario { get; set; }

        [BindProperty]
        public string Contrasena { get; set; }

        [BindProperty]
        public string ConfirmarContrasena { get; set; }

        public string MensajeError { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("usuario") != null)
                return RedirectToPage("/Index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Contrasena))
            {
                MensajeError = "Todos los campos son obligatorios.";
                return Page();
            }

            if (Contrasena != ConfirmarContrasena)
            {
                MensajeError = "Las contraseñas no coinciden.";
                return Page();
            }

            if (await _db.UsuarioExiste(NombreUsuario))
            {
                MensajeError = "Ese nombre de usuario ya está en uso.";
                return Page();
            }

            await _db.InsertUsuario(NombreUsuario, Contrasena);

            HttpContext.Session.SetString("usuario", NombreUsuario);
            return RedirectToPage("/Index");
        }
    }
}
