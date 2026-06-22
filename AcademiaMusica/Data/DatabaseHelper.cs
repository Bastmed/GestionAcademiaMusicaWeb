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

        // ── USUARIOS ───────────────────────────────────────────

        public async Task<Usuario> GetUsuario(string nombreUsuario, string contrasena)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdUsuario, NombreUsuario, Rol, IdReferencia FROM Usuarios WHERE NombreUsuario = @Usuario AND Contrasena = @Contrasena", conn);
            cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);
            cmd.Parameters.AddWithValue("@Contrasena", contrasena);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    IdUsuario = reader.GetInt32(0),
                    NombreUsuario = reader.GetString(1),
                    Rol = reader.IsDBNull(2) ? null : reader.GetString(2),
                    IdReferencia = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                };
            }
            return null;
        }

        public async Task InsertUsuario(string nombreUsuario, string contrasena)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("INSERT INTO Usuarios (NombreUsuario, Contrasena) VALUES (@Usuario, @Contrasena)", conn);
            cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);
            cmd.Parameters.AddWithValue("@Contrasena", contrasena);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UsuarioExiste(string nombreUsuario)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @Usuario", conn);
            cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);
            var resultado = (int)await cmd.ExecuteScalarAsync();
            return resultado > 0;
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

        // ── DISPONIBILIDAD (calendario del profesor) ────────────

        public async Task InsertDisponibilidad(int idProfesor, DateTime inicio, DateTime fin)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("INSERT INTO Disponibilidad (IdProfesor, FechaInicio, FechaFin, Reservado) VALUES (@IdProfesor, @Inicio, @Fin, 0)", conn);
            cmd.Parameters.AddWithValue("@IdProfesor", idProfesor);
            cmd.Parameters.AddWithValue("@Inicio", inicio);
            cmd.Parameters.AddWithValue("@Fin", fin);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Disponibilidad>> GetDisponibilidadPorProfesor(int idProfesor)
        {
            var lista = new List<Disponibilidad>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdDisponibilidad, IdProfesor, FechaInicio, FechaFin, Reservado FROM Disponibilidad WHERE IdProfesor = @IdProfesor", conn);
            cmd.Parameters.AddWithValue("@IdProfesor", idProfesor);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Disponibilidad
                {
                    IdDisponibilidad = reader.GetInt32(0),
                    IdProfesor = reader.GetInt32(1),
                    FechaInicio = reader.GetDateTime(2),
                    FechaFin = reader.GetDateTime(3),
                    Reservado = reader.GetBoolean(4)
                });
            }
            return lista;
        }

        public async Task<List<Disponibilidad>> GetDisponibilidadGeneral()
        {
            var lista = new List<Disponibilidad>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand(@"
                SELECT d.IdDisponibilidad, d.IdProfesor, d.FechaInicio, d.FechaFin, d.Reservado,
                       p.NombreProfesor + ' ' + p.ApellidoProfesor AS NombreCompleto
                FROM Disponibilidad d
                INNER JOIN Profesores p ON p.IdProfesor = d.IdProfesor
                WHERE d.Reservado = 0 AND d.FechaInicio >= GETDATE()", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Disponibilidad
                {
                    IdDisponibilidad = reader.GetInt32(0),
                    IdProfesor = reader.GetInt32(1),
                    FechaInicio = reader.GetDateTime(2),
                    FechaFin = reader.GetDateTime(3),
                    Reservado = reader.GetBoolean(4),
                    NombreProfesor = reader.GetString(5)
                });
            }
            return lista;
        }

        public async Task DeleteDisponibilidad(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("DELETE FROM Disponibilidad WHERE IdDisponibilidad = @Id AND Reservado = 0", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        // ── SOLICITUDES DE CLASE ─────────────────────────────────

        public async Task InsertSolicitud(int idDisponibilidad, int idAlumno, int idProfesor)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                INSERT INTO SolicitudesClase (IdDisponibilidad, IdAlumno, IdProfesor, Estado, FechaSolicitud)
                VALUES (@IdDisp, @IdAlumno, @IdProfesor, 'Pendiente', GETDATE())", conn);
            cmd.Parameters.AddWithValue("@IdDisp", idDisponibilidad);
            cmd.Parameters.AddWithValue("@IdAlumno", idAlumno);
            cmd.Parameters.AddWithValue("@IdProfesor", idProfesor);
            await cmd.ExecuteNonQueryAsync();

            var cmdUpdate = new SqlCommand("UPDATE Disponibilidad SET Reservado = 1 WHERE IdDisponibilidad = @Id", conn);
            cmdUpdate.Parameters.AddWithValue("@Id", idDisponibilidad);
            await cmdUpdate.ExecuteNonQueryAsync();
        }

        public async Task<List<SolicitudClase>> GetSolicitudesPorProfesor(int idProfesor)
        {
            var lista = new List<SolicitudClase>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand(@"
                SELECT s.IdSolicitud, s.IdDisponibilidad, s.IdAlumno, s.IdProfesor, s.Estado, s.FechaSolicitud,
                       a.NombreAlumno + ' ' + a.ApellidoAlumno AS NombreAlumno,
                       d.FechaInicio, d.FechaFin
                FROM SolicitudesClase s
                INNER JOIN Alumnos a ON a.IdAlumno = s.IdAlumno
                INNER JOIN Disponibilidad d ON d.IdDisponibilidad = s.IdDisponibilidad
                WHERE s.IdProfesor = @IdProfesor
                ORDER BY s.FechaSolicitud DESC", conn);
            cmd.Parameters.AddWithValue("@IdProfesor", idProfesor);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new SolicitudClase
                {
                    IdSolicitud = reader.GetInt32(0),
                    IdDisponibilidad = reader.GetInt32(1),
                    IdAlumno = reader.GetInt32(2),
                    IdProfesor = reader.GetInt32(3),
                    Estado = reader.GetString(4),
                    FechaSolicitud = reader.GetDateTime(5),
                    NombreAlumno = reader.GetString(6),
                    FechaInicio = reader.GetDateTime(7),
                    FechaFin = reader.GetDateTime(8)
                });
            }
            return lista;
        }

        public async Task<List<SolicitudClase>> GetSolicitudesPorAlumno(int idAlumno)
        {
            var lista = new List<SolicitudClase>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand(@"
                SELECT s.IdSolicitud, s.IdDisponibilidad, s.IdAlumno, s.IdProfesor, s.Estado, s.FechaSolicitud,
                       p.NombreProfesor + ' ' + p.ApellidoProfesor AS NombreProfesor,
                       d.FechaInicio, d.FechaFin
                FROM SolicitudesClase s
                INNER JOIN Profesores p ON p.IdProfesor = s.IdProfesor
                INNER JOIN Disponibilidad d ON d.IdDisponibilidad = s.IdDisponibilidad
                WHERE s.IdAlumno = @IdAlumno
                ORDER BY s.FechaSolicitud DESC", conn);
            cmd.Parameters.AddWithValue("@IdAlumno", idAlumno);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new SolicitudClase
                {
                    IdSolicitud = reader.GetInt32(0),
                    IdDisponibilidad = reader.GetInt32(1),
                    IdAlumno = reader.GetInt32(2),
                    IdProfesor = reader.GetInt32(3),
                    Estado = reader.GetString(4),
                    FechaSolicitud = reader.GetDateTime(5),
                    NombreProfesor = reader.GetString(6),
                    FechaInicio = reader.GetDateTime(7),
                    FechaFin = reader.GetDateTime(8)
                });
            }
            return lista;
        }
        public async Task<SolicitudClase> GetSolicitudById(int idSolicitud)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand(@"
        SELECT s.IdSolicitud, s.IdDisponibilidad, s.IdAlumno, s.IdProfesor, s.Estado, s.FechaSolicitud,
               a.NombreAlumno + ' ' + a.ApellidoAlumno AS NombreAlumno,
               p.NombreProfesor + ' ' + p.ApellidoProfesor AS NombreProfesor,
               d.FechaInicio, d.FechaFin,
               a.EmailAlumno,
               p.EmailProfesor
        FROM SolicitudesClase s
        INNER JOIN Alumnos a ON a.IdAlumno = s.IdAlumno
        INNER JOIN Profesores p ON p.IdProfesor = s.IdProfesor
        INNER JOIN Disponibilidad d ON d.IdDisponibilidad = s.IdDisponibilidad
        WHERE s.IdSolicitud = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", idSolicitud);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new SolicitudClase
                {
                    IdSolicitud = reader.GetInt32(0),
                    IdDisponibilidad = reader.GetInt32(1),
                    IdAlumno = reader.GetInt32(2),
                    IdProfesor = reader.GetInt32(3),
                    Estado = reader.GetString(4),
                    FechaSolicitud = reader.GetDateTime(5),
                    NombreAlumno = reader.GetString(6),
                    NombreProfesor = reader.GetString(7),
                    FechaInicio = reader.GetDateTime(8),
                    FechaFin = reader.GetDateTime(9),
                    EmailAlumno = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    EmailProfesor = reader.IsDBNull(11) ? "" : reader.GetString(11)
                };
            }
            return null;
        }

        // Versión de InsertSolicitud que devuelve los datos completos para el correo al profesor
        public async Task<SolicitudClase> InsertSolicitudYObtenerDatos(int idDisponibilidad, int idAlumno, int idProfesor)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            // Insertar solicitud
            var cmdInsert = new SqlCommand(@"
        INSERT INTO SolicitudesClase (IdDisponibilidad, IdAlumno, IdProfesor, Estado, FechaSolicitud)
        VALUES (@IdDisp, @IdAlumno, @IdProfesor, 'Pendiente', GETDATE());
        SELECT SCOPE_IDENTITY();", conn);
            cmdInsert.Parameters.AddWithValue("@IdDisp", idDisponibilidad);
            cmdInsert.Parameters.AddWithValue("@IdAlumno", idAlumno);
            cmdInsert.Parameters.AddWithValue("@IdProfesor", idProfesor);
            var nuevoId = Convert.ToInt32(await cmdInsert.ExecuteScalarAsync());

            // Marcar bloque como reservado
            var cmdUpdate = new SqlCommand("UPDATE Disponibilidad SET Reservado = 1 WHERE IdDisponibilidad = @Id", conn);
            cmdUpdate.Parameters.AddWithValue("@Id", idDisponibilidad);
            await cmdUpdate.ExecuteNonQueryAsync();

            // Obtener datos completos para el correo
            var cmdGet = new SqlCommand(@"
        SELECT a.NombreAlumno + ' ' + a.ApellidoAlumno, p.NombreProfesor + ' ' + p.ApellidoProfesor,
               d.FechaInicio, d.FechaFin, a.EmailAlumno, p.EmailProfesor
        FROM SolicitudesClase s
        INNER JOIN Alumnos a ON a.IdAlumno = s.IdAlumno
        INNER JOIN Profesores p ON p.IdProfesor = s.IdProfesor
        INNER JOIN Disponibilidad d ON d.IdDisponibilidad = s.IdDisponibilidad
        WHERE s.IdSolicitud = @Id", conn);
            cmdGet.Parameters.AddWithValue("@Id", nuevoId);
            using var reader = await cmdGet.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new SolicitudClase
                {
                    IdSolicitud = nuevoId,
                    NombreAlumno = reader.GetString(0),
                    NombreProfesor = reader.GetString(1),
                    FechaInicio = reader.GetDateTime(2),
                    FechaFin = reader.GetDateTime(3),
                    EmailAlumno = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    EmailProfesor = reader.IsDBNull(5) ? "" : reader.GetString(5)
                };
            }
            return null;
        }

        public async Task ResponderSolicitud(int idSolicitud, bool aceptar)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string nuevoEstado = aceptar ? "Aceptada" : "Rechazada";

            var cmd = new SqlCommand("UPDATE SolicitudesClase SET Estado = @Estado WHERE IdSolicitud = @Id", conn);
            cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
            cmd.Parameters.AddWithValue("@Id", idSolicitud);
            await cmd.ExecuteNonQueryAsync();

            if (!aceptar)
            {
                var cmdGetDisp = new SqlCommand("SELECT IdDisponibilidad FROM SolicitudesClase WHERE IdSolicitud = @Id", conn);
                cmdGetDisp.Parameters.AddWithValue("@Id", idSolicitud);
                var idDisp = (int)await cmdGetDisp.ExecuteScalarAsync();

                var cmdLiberar = new SqlCommand("UPDATE Disponibilidad SET Reservado = 0 WHERE IdDisponibilidad = @IdDisp", conn);
                cmdLiberar.Parameters.AddWithValue("@IdDisp", idDisp);
                await cmdLiberar.ExecuteNonQueryAsync();
            }
        }

        // ── ADMIN: obtener todos los usuarios ─────────────────────────────────

        public async Task<List<Usuario>> GetUsuarios()
        {
            var lista = new List<Usuario>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("SELECT IdUsuario, NombreUsuario, Rol, IdReferencia FROM Usuarios", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = reader.GetInt32(0),
                    NombreUsuario = reader.GetString(1),
                    Rol = reader.IsDBNull(2) ? null : reader.GetString(2),
                    IdReferencia = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                });
            }
            return lista;
        }

        // ── ADMIN: asignar rol e IdReferencia a un usuario ────────────────────

        public async Task AsignarRol(int idUsuario, string rol, int? idReferencia)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var cmd = new SqlCommand("UPDATE Usuarios SET Rol = @Rol, IdReferencia = @IdReferencia WHERE IdUsuario = @Id", conn);
            cmd.Parameters.AddWithValue("@Rol", (object?)rol ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdReferencia", (object?)idReferencia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", idUsuario);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}