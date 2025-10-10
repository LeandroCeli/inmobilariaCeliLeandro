using inmobilariaCeli.Models;
using inmobilariaCeli.Data;
using MySql.Data.MySqlClient;

namespace inmobilariaCeli.Repositories;

public class InquilinoRepository
{
    private readonly DbConnectionFactory _factory;
    public InquilinoRepository(DbConnectionFactory factory) => _factory = factory;

    // Obtener todos los inquilinos
    public async Task<List<Inquilino>> GetAllAsync()
    {
        var list = new List<Inquilino>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT Id, Nombre, Apellido, DNI, Telefono, Email FROM Inquilinos ORDER BY Nombre";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            var nombre = rd.IsDBNull(rd.GetOrdinal("Nombre")) ? "" : rd.GetString(rd.GetOrdinal("Nombre"));
            var apellido = rd.IsDBNull(rd.GetOrdinal("Apellido")) ? "" : rd.GetString(rd.GetOrdinal("Apellido"));

            list.Add(new Inquilino
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                Nombre = nombre,
                Apellido = apellido,
                DNI = rd.IsDBNull(rd.GetOrdinal("DNI")) ? "" : rd.GetString(rd.GetOrdinal("DNI")),
                Telefono = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? "" : rd.GetString(rd.GetOrdinal("Telefono")),
                Email = rd.IsDBNull(rd.GetOrdinal("Email")) ? "" : rd.GetString(rd.GetOrdinal("Email"))

            });
        }
        return list;
    }

    public async Task<List<Inquilino>> GetAllConEstadoEliminacionAsync()
    {
        var list = new List<Inquilino>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT i.Id, i.Nombre, i.Apellido, i.DNI, i.Telefono, i.Email,
        (SELECT COUNT(*) FROM Contratos c WHERE c.IdInquilino = i.Id) AS ContratosActivos
        FROM Inquilinos i
        ORDER BY i.Nombre";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            var nombre = rd.IsDBNull(rd.GetOrdinal("Nombre")) ? "" : rd.GetString(rd.GetOrdinal("Nombre"));
            var apellido = rd.IsDBNull(rd.GetOrdinal("Apellido")) ? "" : rd.GetString(rd.GetOrdinal("Apellido"));
            var contratos = rd.GetInt32(rd.GetOrdinal("ContratosActivos"));

            list.Add(new Inquilino
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                Nombre = nombre,
                Apellido = apellido,
                DNI = rd.IsDBNull(rd.GetOrdinal("DNI")) ? "" : rd.GetString(rd.GetOrdinal("DNI")),
                Telefono = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? "" : rd.GetString(rd.GetOrdinal("Telefono")),
                Email = rd.IsDBNull(rd.GetOrdinal("Email")) ? "" : rd.GetString(rd.GetOrdinal("Email")),
                PuedeEliminar = contratos == 0
            });
        }
        return list;
    }







    // Obtener un inquilino por Id
    public async Task<Inquilino?> GetByIdAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT i.Id, i.Nombre, i.Apellido, i.DNI, i.Telefono, i.Email,
        (SELECT COUNT(*) FROM Contratos c WHERE c.IdInquilino = i.Id) AS ContratosActivos
        FROM Inquilinos i
        WHERE i.Id = @Id";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        if (await rd.ReadAsync())
        {
            var nombre = rd.IsDBNull(rd.GetOrdinal("Nombre")) ? "" : rd.GetString(rd.GetOrdinal("Nombre"));
            var apellido = rd.IsDBNull(rd.GetOrdinal("Apellido")) ? "" : rd.GetString(rd.GetOrdinal("Apellido"));
            var contratos = rd.GetInt32(rd.GetOrdinal("ContratosActivos"));

            return new Inquilino
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                Nombre = nombre,
                Apellido = apellido,
                DNI = rd.IsDBNull(rd.GetOrdinal("DNI")) ? "" : rd.GetString(rd.GetOrdinal("DNI")),
                Telefono = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? "" : rd.GetString(rd.GetOrdinal("Telefono")),
                Email = rd.IsDBNull(rd.GetOrdinal("Email")) ? "" : rd.GetString(rd.GetOrdinal("Email")),
                PuedeEliminar = contratos == 0
            };
        }

        return null;
    }

    // Crear nuevo inquilino
    public async Task CreateAsync(Inquilino i)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Inquilinos (Nombre, Apellido, DNI, Telefono, Email)
                            VALUES (@Nombre, @Apellido, @DNI, @Telefono, @Email)";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Nombre", i.Nombre);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Apellido", i.Apellido);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@DNI", i.DNI);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Telefono", i.Telefono);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Email", i.Email);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    // Actualizar inquilino existente
    public async Task UpdateAsync(Inquilino i)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Inquilinos SET
                            Nombre=@Nombre, Apellido=@Apellido, DNI=@DNI,
                            Telefono=@Telefono, Email=@Email
                            WHERE Id=@Id";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", i.Id);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Nombre", i.Nombre);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Apellido", i.Apellido);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@DNI", i.DNI);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Telefono", i.Telefono);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Email", i.Email);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    // Eliminar inquilino
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