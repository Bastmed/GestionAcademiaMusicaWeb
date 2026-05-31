namespace AcademiaMusica.Models
    {
    public class Instrumento
    {
        public int IdInstrumento { get; set; }
        public string NombreInstrumento { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public string Estado { get; set; }

        public Instrumento()
        {
            Estado = "Disponible";
        }
    }
}