
using MySql.Data.MySqlClient;
using inmobilariaCeli.Data;
using inmobilariaCeli.Models;

namespace inmobilariaCeli.Repositories;

public class PropietarioRepository
{
    private readonly DbConnectionFactory _factory;
    public PropietarioRepository(DbConnectionFactory factory) => _factory = factory;

    // Listar todos los propietarios
    public async Task<List<Propietario>> GetAll()
    {
        var list = new List<Propietario>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Propietarios ORDER BY Apellido, Nombre";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new Propietario
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                DNI = rd.GetString(rd.GetOrdinal("DNI")),
                Apellido = rd.GetString(rd.GetOrdinal("Apellido")),
                Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
                Email = rd.IsDBNull(rd.GetOrdinal("Email")) ? null : rd.GetString(rd.GetOrdinal("Email")),
                Telefono = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? null : rd.GetString(rd.GetOrdinal("Telefono")),
                Direccion = rd.IsDBNull(rd.GetOrdinal("Direccion")) ? null : rd.GetString(rd.GetOrdinal("Direccion")),
                FechaAlta = rd.GetDateTime(rd.GetOrdinal("FechaAlta"))
            });
        }
        return list;
    }
    public async Task<List<Propietario>> GetAllConEstadoEliminacionAsync()
    {
        var list = new List<Propietario>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT p.*, 
        (SELECT COUNT(*) FROM Contratos c 
         INNER JOIN Propiedades pr ON c.IdPropiedad = pr.Id 
         WHERE pr.IdPropietario = p.Id) AS ContratosActivos
        FROM Propietarios p
        ORDER BY p.Apellido, p.Nombre";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            var contratos = rd.GetInt32(rd.GetOrdinal("ContratosActivos"));

            list.Add(new Propietario
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                DNI = rd.GetString(rd.GetOrdinal("DNI")),
                Apellido = rd.GetString(rd.GetOrdinal("Apellido")),
                Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
                Email = rd.IsDBNull(rd.GetOrdinal("Email")) ? null : rd.GetString(rd.GetOrdinal("Email")),
                Telefono = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? null : rd.GetString(rd.GetOrdinal("Telefono")),
                Direccion = rd.IsDBNull(rd.GetOrdinal("Direccion")) ? null : rd.GetString(rd.GetOrdinal("Direccion")),
                FechaAlta = rd.GetDateTime(rd.GetOrdinal("FechaAlta")),
                PuedeEliminar = contratos == 0
            });
        }
        return list;
    }






    // Buscar por Id
    public async Task<Propietario?> GetById(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Propietarios WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        if (await rd.ReadAsync())
        {
            return new Propietario
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                DNI = rd.GetString(rd.GetOrdinal("DNI")),
                Apellido = rd.GetString(rd.GetOrdinal("Apellido")),
                Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
                Email = rd.IsDBNull(rd.GetOrdinal("Email")) ? null : rd.GetString(rd.GetOrdinal("Email")),
                Telefono = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? null : rd.GetString(rd.GetOrdinal("Telefono")),
                Direccion = rd.IsDBNull(rd.GetOrdinal("Direccion")) ? null : rd.GetString(rd.GetOrdinal("Direccion")),
                FechaAlta = rd.GetDateTime(rd.GetOrdinal("FechaAlta"))
            };
        }
        return null;
    }

    // alta Propietario
    public async Task<int> AltaPropietario(Propietario p)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Propietarios
            (DNI, Apellido, Nombre, Email, Telefono, Direccion, FechaAlta)
            VALUES (@DNI, @Apellido, @Nombre, @Email, @Telefono, @Direccion, @FechaAlta);
            SELECT LAST_INSERT_ID();";

        var c = (MySqlCommand)cmd;
        c.Parameters.AddWithValue("@DNI", p.DNI);
        c.Parameters.AddWithValue("@Apellido", p.Apellido);
        c.Parameters.AddWithValue("@Nombre", p.Nombre);
        c.Parameters.AddWithValue("@Email", (object?)p.Email ?? DBNull.Value);
        c.Parameters.AddWithValue("@Telefono", (object?)p.Telefono ?? DBNull.Value);
        c.Parameters.AddWithValue("@Direccion", (object?)p.Direccion ?? DBNull.Value);
        c.Parameters.AddWithValue("@FechaAlta", p.FechaAlta);

        var id = Convert.ToInt32(await c.ExecuteScalarAsync());
        return id;
    }

    // Actualizar propietario
    public async Task ActualizarPropietario(Propietario p)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Propietarios SET
            DNI=@DNI, Apellido=@Apellido, Nombre=@Nombre,
            Email=@Email, Telefono=@Telefono, Direccion=@Direccion
            WHERE Id=@Id";

        var c = (MySqlCommand)cmd;
        c.Parameters.AddWithValue("@Id", p.Id);
        c.Parameters.AddWithValue("@DNI", p.DNI);
        c.Parameters.AddWithValue("@Apellido", p.Apellido);
        c.Parameters.AddWithValue("@Nombre", p.Nombre);
        c.Parameters.AddWithValue("@Email", (object?)p.Email ?? DBNull.Value);
        c.Parameters.AddWithValue("@Telefono", (object?)p.Telefono ?? DBNull.Value);
        c.Parameters.AddWithValue("@Direccion", (object?)p.Direccion ?? DBNull.Value);

        await c.ExecuteNonQueryAsync();
    }

    // Eliminar propietario
    public async Task DeletePropietario(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Propietarios WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }


}
