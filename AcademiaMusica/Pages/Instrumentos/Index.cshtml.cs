using AcademiaMusica.Data;
using AcademiaMusica.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace AcademiaMusica.Pages.Instrumentos
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseHelper _db;
        public List<Instrumento> Instrumentos { get; set; } = new();

        public IndexModel(DatabaseHelper db)
        {
            _db = db;
        }

        public void OnGet()
        {
            using var con = _db.GetConnection();
            con.Open();
            var cmd = new SqlCommand("SELECT IdInstrumento, NombreInstrumento, Categoria, Marca, Estado FROM Instrumentos", con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Instrumentos.Add(new Instrumento
                {
                    IdInstrumento = (int)reader["IdInstrumento"],
                    NombreInstrumento = reader["NombreInstrumento"].ToString(),
                    Categoria = reader["Categoria"].ToString(),
                    Marca = reader["Marca"].ToString(),
                    Estado = reader["Estado"].ToString()
                });
            }
        }
    }
}