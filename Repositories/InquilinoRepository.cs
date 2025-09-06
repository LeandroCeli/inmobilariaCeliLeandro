using inmobilariaCeli.Models;
using inmobilariaCeli.Data;
using MySql.Data.MySqlClient;

namespace inmobilariaCeli.Repositories;

public class InquilinoRepository
{
    private readonly DbConnectionFactory _factory;
    public InquilinoRepository(DbConnectionFactory factory) => _factory = factory;

    // Obtener todos
    public async Task<List<Inquilino>> GetAllAsync()
    {
        var list = new List<Inquilino>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Inquilinos ORDER BY Apellido, Nombre";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new Inquilino
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

    // Obtener por Id
    public async Task<Inquilino?> GetByIdAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Inquilinos WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        if (await rd.ReadAsync())
        {
            return new Inquilino
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

    // Crear
    public async Task CreateAsync(Inquilino i)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Inquilinos 
            (DNI, Apellido, Nombre, Email, Telefono, Direccion, FechaAlta) 
            VALUES (@DNI, @Apellido, @Nombre, @Email, @Telefono, @Direccion, @FechaAlta)";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@DNI", i.DNI);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Apellido", i.Apellido);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Nombre", i.Nombre);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Email", (object?)i.Email ?? DBNull.Value);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Telefono", (object?)i.Telefono ?? DBNull.Value);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Direccion", (object?)i.Direccion ?? DBNull.Value);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@FechaAlta", i.FechaAlta);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    // Editar
    public async Task UpdateAsync(Inquilino i)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Inquilinos SET 
            DNI=@DNI, Apellido=@Apellido, Nombre=@Nombre, 
            Email=@Email, Telefono=@Telefono, Direccion=@Direccion 
            WHERE Id=@Id";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", i.Id);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@DNI", i.DNI);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Apellido", i.Apellido);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Nombre", i.Nombre);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Email", (object?)i.Email ?? DBNull.Value);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Telefono", (object?)i.Telefono ?? DBNull.Value);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Direccion", (object?)i.Direccion ?? DBNull.Value);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    // Eliminar
    public async Task DeleteAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Inquilinos WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }
}
