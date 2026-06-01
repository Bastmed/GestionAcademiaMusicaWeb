using AcademiaMusica.Models;
using Microsoft.Data.SqlClient;

namespace AcademiaMusica.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conexion");
        }

        // ── ALUMNOS ────────────────────────────────────────────

        public async Task<List<Alumno>> GetAlumnos()
        {
            var lista = new List<Alumno>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdAlumno, NombreAlumno, ApellidoAlumno, EmailAlumno, FechaNacimiento, TelefonoAlumno, ActivoAlumno, IdInstrumento FROM Alumnos", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Alumno
                {
                    IdAlumno = reader.GetInt32(0),
                    NombreAlumno = reader.GetString(1),
                    ApellidoAlumno = reader.GetString(2),
                    EmailAlumno = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    FechaNacimiento = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    TelefonoAlumno = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    ActivoAlumno = reader.GetBoolean(6),
                    IdInstrumento = reader.IsDBNull(7) ? null : reader.GetInt32(7)
                });
            }
            return lista;
        }

        public async Task<Alumno> GetAlumnoById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdAlumno, NombreAlumno, ApellidoAlumno, EmailAlumno, FechaNacimiento, TelefonoAlumno, ActivoAlumno, IdInstrumento FROM Alumnos WHERE IdAlumno = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Alumno
                {
                    IdAlumno = reader.GetInt32(0),
                    NombreAlumno = reader.GetString(1),
                    ApellidoAlumno = reader.GetString(2),
                    EmailAlumno = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    FechaNacimiento = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    TelefonoAlumno = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    ActivoAlumno = reader.GetBoolean(6),
                    IdInstrumento = reader.IsDBNull(7) ? null : reader.GetInt32(7)
                };
            }
            return null;
        }

        public async Task InsertAlumnos(string nombre, string apellido, DateTime? fecha, string telefono, string email, bool activo, int? idInstrumento)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("INSERT INTO Alumnos (NombreAlumno, ApellidoAlumno, FechaNacimiento, TelefonoAlumno, EmailAlumno, ActivoAlumno, IdInstrumento) VALUES (@Nombre, @Apellido, @Fecha, @Telefono, @Email, @Activo, @IdInstrumento)", conn);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Apellido", apellido);
            cmd.Parameters.AddWithValue("@Fecha", (object?)fecha ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", (object?)telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Activo", activo);
            cmd.Parameters.AddWithValue("@IdInstrumento", (object?)idInstrumento ?? DBNull.Value);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAlumno(Alumno a)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("UPDATE Alumnos SET NombreAlumno=@Nombre, ApellidoAlumno=@Apellido, FechaNacimiento=@Fecha, TelefonoAlumno=@Telefono, EmailAlumno=@Email, ActivoAlumno=@Activo, IdInstrumento=@IdInstrumento WHERE IdAlumno=@Id", conn);
            cmd.Parameters.AddWithValue("@Nombre", a.NombreAlumno);
            cmd.Parameters.AddWithValue("@Apellido", a.ApellidoAlumno);
            cmd.Parameters.AddWithValue("@Fecha", (object?)a.FechaNacimiento ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", (object?)a.TelefonoAlumno ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)a.EmailAlumno ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Activo", a.ActivoAlumno);
            cmd.Parameters.AddWithValue("@IdInstrumento", (object?)a.IdInstrumento ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", a.IdAlumno);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAlumno(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("DELETE FROM Alumnos WHERE IdAlumno = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ── INSTRUMENTOS ───────────────────────────────────────

        public async Task<List<Instrumento>> GetInstrumentos()
        {
            var lista = new List<Instrumento>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdInstrumento, NombreInstrumento, Categoria, Marca, Estado FROM Instrumentos", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Instrumento
                {
                    IdInstrumento = reader.GetInt32(0),
                    NombreInstrumento = reader.GetString(1),
                    Categoria = reader.GetString(2),
                    Marca = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Estado = reader.GetString(4)
                });
            }
            return lista;
        }

        public async Task<Instrumento> GetInstrumentoById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdInstrumento, NombreInstrumento, Categoria, Marca, Estado FROM Instrumentos WHERE IdInstrumento = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Instrumento
                {
                    IdInstrumento = reader.GetInt32(0),
                    NombreInstrumento = reader.GetString(1),
                    Categoria = reader.GetString(2),
                    Marca = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Estado = reader.GetString(4)
                };
            }
            return null;
        }

        public async Task InsertInstrumento(Instrumento i)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("INSERT INTO Instrumentos (NombreInstrumento, Categoria, Marca, Estado) VALUES (@Nombre, @Categoria, @Marca, @Estado)", conn);
            cmd.Parameters.AddWithValue("@Nombre", i.NombreInstrumento);
            cmd.Parameters.AddWithValue("@Categoria", i.Categoria);
            cmd.Parameters.AddWithValue("@Marca", (object?)i.Marca ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado", i.Estado);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateInstrumento(Instrumento i)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("UPDATE Instrumentos SET NombreInstrumento=@Nombre, Categoria=@Categoria, Marca=@Marca, Estado=@Estado WHERE IdInstrumento=@Id", conn);
            cmd.Parameters.AddWithValue("@Nombre", i.NombreInstrumento);
            cmd.Parameters.AddWithValue("@Categoria", i.Categoria);
            cmd.Parameters.AddWithValue("@Marca", (object?)i.Marca ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado", i.Estado);
            cmd.Parameters.AddWithValue("@Id", i.IdInstrumento);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteInstrumento(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("DELETE FROM Instrumentos WHERE IdInstrumento = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ── PROFESORES ─────────────────────────────────────────

        public async Task<List<Profesor>> GetProfesores()
        {
            var lista = new List<Profesor>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdProfesor, NombreProfesor, ApellidoProfesor, TelefonoProfesor, EmailProfesor, Especialidad, TarifaHora, ActivoProfesor FROM Profesores", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Profesor
                {
                    IdProfesor = reader.GetInt32(0),
                    NombreProfesor = reader.GetString(1),
                    ApellidoProfesor = reader.GetString(2),
                    TelefonoProfesor = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    EmailProfesor = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Especialidad = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    TarifaHora = reader.GetDecimal(6),
                    ActivoProfesor = reader.GetBoolean(7)
                });
            }
            return lista;
        }

        public async Task<Profesor> GetProfesorById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdProfesor, NombreProfesor, ApellidoProfesor, TelefonoProfesor, EmailProfesor, Especialidad, TarifaHora, ActivoProfesor FROM Profesores WHERE IdProfesor = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Profesor
                {
                    IdProfesor = reader.GetInt32(0),
                    NombreProfesor = reader.GetString(1),
                    ApellidoProfesor = reader.GetString(2),
                    TelefonoProfesor = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    EmailProfesor = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Especialidad = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    TarifaHora = reader.GetDecimal(6),
                    ActivoProfesor = reader.GetBoolean(7)
                };
            }
            return null;
        }

        public async Task InsertProfesor(Profesor p)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("INSERT INTO Profesores (NombreProfesor, ApellidoProfesor, TelefonoProfesor, EmailProfesor, Especialidad, TarifaHora, ActivoProfesor) VALUES (@Nombre, @Apellido, @Telefono, @Email, @Especialidad, @Tarifa, @Activo)", conn);
            cmd.Parameters.AddWithValue("@Nombre", p.NombreProfesor);
            cmd.Parameters.AddWithValue("@Apellido", p.ApellidoProfesor);
            cmd.Parameters.AddWithValue("@Telefono", (object?)p.TelefonoProfesor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)p.EmailProfesor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Especialidad", (object?)p.Especialidad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Tarifa", p.TarifaHora);
            cmd.Parameters.AddWithValue("@Activo", p.ActivoProfesor);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateProfesor(Profesor p)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("UPDATE Profesores SET NombreProfesor=@Nombre, ApellidoProfesor=@Apellido, TelefonoProfesor=@Telefono, EmailProfesor=@Email, Especialidad=@Especialidad, TarifaHora=@Tarifa, ActivoProfesor=@Activo WHERE IdProfesor=@Id", conn);
            cmd.Parameters.AddWithValue("@Nombre", p.NombreProfesor);
            cmd.Parameters.AddWithValue("@Apellido", p.ApellidoProfesor);
            cmd.Parameters.AddWithValue("@Telefono", (object?)p.TelefonoProfesor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object?)p.EmailProfesor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Especialidad", (object?)p.Especialidad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Tarifa", p.TarifaHora);
            cmd.Parameters.AddWithValue("@Activo", p.ActivoProfesor);
            cmd.Parameters.AddWithValue("@Id", p.IdProfesor);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteProfesor(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("DELETE FROM Profesores WHERE IdProfesor = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}