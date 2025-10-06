using inmobilariaCeli.Data;
using MySql.Data.MySqlClient;
using inmobilariaCeli.Models;

namespace inmobilariaCeli.Repositories
{
    public class UsuarioRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UsuarioRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand("SELECT * FROM Usuarios WHERE Email = @email", (MySqlConnection)connection);
            command.Parameters.AddWithValue("@email", email);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearUsuario(reader);
            }

            return null;
        }

        public Usuario? ObtenerPorId(int id)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand("SELECT * FROM Usuarios WHERE Id = @id", (MySqlConnection)connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapearUsuario(reader);
            }

            return null;
        }

        public List<Usuario> ListarTodos()
        {
            var lista = new List<Usuario>();
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand("SELECT * FROM Usuarios", (MySqlConnection)connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearUsuario(reader));
            }

            return lista;
        }

        public void CrearUsuario(Usuario usuario)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand(@"
                INSERT INTO Usuarios (Email, PasswordHash, NombreCompleto, Rol, FotoPerfil)
                VALUES (@Email, @PasswordHash, @NombreCompleto, @Rol, @FotoPerfil)", (MySqlConnection)connection);

            command.Parameters.AddWithValue("@Email", usuario.Email);
            command.Parameters.AddWithValue("@PasswordHash", usuario.Password);
            command.Parameters.AddWithValue("@NombreCompleto", usuario.NombreCompleto);
            command.Parameters.AddWithValue("@Rol", usuario.Rol);
            command.Parameters.AddWithValue("@FotoPerfil", (object?)usuario.FotoPerfil ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand(@"
                UPDATE Usuarios
                SET Email = @Email,
                    NombreCompleto = @NombreCompleto,
                    Rol = @Rol,
                    FotoPerfil = @FotoPerfil
                WHERE Id = @Id", (MySqlConnection)connection);

            command.Parameters.AddWithValue("@Email", usuario.Email);
            command.Parameters.AddWithValue("@NombreCompleto", usuario.NombreCompleto);
            command.Parameters.AddWithValue("@Rol", usuario.Rol);
            command.Parameters.AddWithValue("@FotoPerfil", (object?)usuario.FotoPerfil ?? DBNull.Value);
            command.Parameters.AddWithValue("@Id", usuario.Id);

            command.ExecuteNonQuery();
        }

        public void ActualizarPassword(int id, string nuevoHash)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand("UPDATE Usuarios SET PasswordHash = @hash WHERE Id = @id", (MySqlConnection)connection);
            command.Parameters.AddWithValue("@hash", nuevoHash);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void ActualizarFoto(int id, string ruta)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand("UPDATE Usuarios SET FotoPerfil = @ruta WHERE Id = @id", (MySqlConnection)connection);
            command.Parameters.AddWithValue("@ruta", ruta);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void QuitarFoto(int id)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand("UPDATE Usuarios SET FotoPerfil = NULL WHERE Id = @id", (MySqlConnection)connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = new MySqlCommand("DELETE FROM Usuarios WHERE Id = @id", (MySqlConnection)connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        private Usuario MapearUsuario(MySqlDataReader reader)
        {
            return new Usuario
            {
                Id = reader.GetInt32("Id"),
                Email = reader.GetString("Email"),
                Password = reader.GetString("PasswordHash"),
                NombreCompleto = reader.GetString("NombreCompleto"),
                Rol = Enum.TryParse<Usuario.RolUsuario>(reader.GetString(reader.GetOrdinal("Rol")), out var rolParseado)
    ? rolParseado
    : Usuario.RolUsuario.Empleado,

                FotoPerfil = reader.IsDBNull(reader.GetOrdinal("FotoPerfil")) ? null : reader.GetString("FotoPerfil")
            };
        }
    }
}