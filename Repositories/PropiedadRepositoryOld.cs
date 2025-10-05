using MySql.Data.MySqlClient;
using inmobilariaCeli.Data;
using inmobilariaCeli.Models;

namespace inmobilariaCeli.Repositories;

public class PropiedadRepository
{
    private readonly DbConnectionFactory _factory;
    public PropiedadRepository(DbConnectionFactory factory) => _factory = factory;

    // Listar todas las propiedades
    public async Task<List<Propiedad>> GetAll()
    {
        var list = new List<Propiedad>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Propiedades ORDER BY Direccion";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new Propiedad
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                Direccion =  rd.GetString(rd.GetOrdinal("Direccion")),
                Tipo = rd.GetString(rd.GetOrdinal("Tipo")),
                Uso = rd.GetString(rd.GetOrdinal("Uso")),
                Ambientes = rd.GetInt32(rd.GetOrdinal("Ambientes")),
                Precio =  rd.GetDecimal(rd.GetOrdinal("Precio")),
                IdPropietario = rd.GetInt32(rd.GetOrdinal("IdPropietario"))
            });
        }
        return list;
    }

    // Buscar por Id
    public async Task<Propiedad?> GetById(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Propiedades WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        if (await rd.ReadAsync())
        {
            return new Propiedad
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                Direccion =  rd.GetString(rd.GetOrdinal("Direccion")),
                Tipo = rd.GetString(rd.GetOrdinal("Tipo")),
                Uso = rd.GetString(rd.GetOrdinal("Uso")),
                Ambientes = rd.GetInt32(rd.GetOrdinal("Ambientes")),
                Precio =rd.GetDecimal(rd.GetOrdinal("Precio")),
                IdPropietario = rd.GetInt32(rd.GetOrdinal("IdPropietario"))
            };
        }
        return null;
    }

    // Alta de propiedad
    public async Task<int> AltaPropiedad(Propiedad p)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Propiedades 
            (Direccion, Tipo, Uso, Ambientes, Precio, IdPropietario)
            VALUES (@Direccion, @Tipo, @Uso, @Ambientes, @Precio, @IdPropietario);
            SELECT LAST_INSERT_ID();";

        var c = (MySqlCommand)cmd;
        c.Parameters.AddWithValue("@Direccion", p.Direccion);
        c.Parameters.AddWithValue("@Tipo", p.Tipo);
        c.Parameters.AddWithValue("@Uso", p.Uso);
        c.Parameters.AddWithValue("@Ambientes", p.Ambientes);
        c.Parameters.AddWithValue("@Precio", p.Precio);
        c.Parameters.AddWithValue("@IdPropietario", p.IdPropietario);

        var id = Convert.ToInt32(await c.ExecuteScalarAsync());
        return id;
    }

    // Actualizar propiedad
    public async Task ActualizarPropiedad(Propiedad p)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Propiedades SET
            Direccion=@Direccion, Tipo=@Tipo, Uso=@Uso,
            Ambientes=@Ambientes, Precio=@Precio, IdPropietario=@IdPropietario
            WHERE Id=@Id";

        var c = (MySqlCommand)cmd;
        c.Parameters.AddWithValue("@Id", p.Id);
        c.Parameters.AddWithValue("@Direccion", p.Direccion);
        c.Parameters.AddWithValue("@Tipo", p.Tipo);
        c.Parameters.AddWithValue("@Uso", p.Uso);
        c.Parameters.AddWithValue("@Ambientes", p.Ambientes);
        c.Parameters.AddWithValue("@Precio", p.Precio);
        c.Parameters.AddWithValue("@IdPropietario", p.IdPropietario);

        await c.ExecuteNonQueryAsync();
    }

    // Eliminar propiedad
    public async Task DeletePropiedad(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Propiedades WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }
}
