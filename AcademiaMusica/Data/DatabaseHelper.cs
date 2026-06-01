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

        public async Task InsertAlumnos(string nombreAlumno, string apellidoAlumno, DateTime? fechaNacimiento, string telefonoAlumno, string emailAlumno, bool activoAlumno)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO Alumnos (NombreAlumno, ApellidoAlumno, EmailAlumno, FechaNacimiento, TelefonoAlumno, ActivoAlumno) VALUES (@NombreAlumno, @ApellidoAlumno, @EmailAlumno, @FechaNacimiento, @TelefonoAlumno, @ActivoAlumno)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreAlumno", nombreAlumno);
                    cmd.Parameters.AddWithValue("@ApellidoAlumno", apellidoAlumno);
                    cmd.Parameters.AddWithValue("@EmailAlumno", emailAlumno);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento.HasValue ? (object)fechaNacimiento.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@TelefonoAlumno", telefonoAlumno);
                    cmd.Parameters.AddWithValue("@ActivoAlumno", activoAlumno);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task <List<Alumno>> GetAlumnos()
        {
            List<Alumno> alumnos = new List<Alumno>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT IdAlumno, NombreAlumno, ApellidoAlumno, EmailAlumno, FechaNacimiento, TelefonoAlumno, ActivoAlumno FROM Alumnos";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            alumnos.Add(new Alumno
                            {
                                IdAlumno = reader.GetInt32(0),
                                NombreAlumno = reader.GetString(1),
                                ApellidoAlumno = reader.GetString(2),
                                EmailAlumno = reader.GetString(3),
                                FechaNacimiento = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                TelefonoAlumno = reader.GetString(5),
                                ActivoAlumno = reader.GetBoolean(6)
                            });
                        }
                    }
                }
            }
            return alumnos;

        }

    }
}
