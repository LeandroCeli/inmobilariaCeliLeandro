using inmobilariaCeli.Data;
using inmobilariaCeli.Models;
using MySql.Data.MySqlClient;

namespace inmobilariaCeli.Repositories;

public class InmuebleRepository
{
    private readonly DbConnectionFactory _factory;
    public InmuebleRepository(DbConnectionFactory factory) => _factory = factory;

    // 🔹 Listar todos los inmuebles con nombre del propietario
    public async Task<List<Inmueble>> GetAllAsync()
    {
        var list = new List<Inmueble>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT i.*, CONCAT(p.Nombre, ' ', p.Apellido) AS PropietarioNombre
                            FROM propiedades i
                            JOIN Propietarios p ON i.IdPropietario = p.Id
                            ORDER BY i.Direccion";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new Inmueble
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                Direccion = rd.GetString(rd.GetOrdinal("Direccion")),
                Tipo = rd.GetString(rd.GetOrdinal("Tipo")),
                Uso = rd.GetString(rd.GetOrdinal("Uso")),
                Ambientes = rd.GetInt32(rd.GetOrdinal("Ambientes")),
                Precio = rd.GetDecimal(rd.GetOrdinal("Precio")),
                IdPropietario = rd.GetInt32(rd.GetOrdinal("IdPropietario")),
                PropietarioNombre = rd.IsDBNull(rd.GetOrdinal("PropietarioNombre")) ? "" : rd.GetString(rd.GetOrdinal("PropietarioNombre"))
            });
        }
        return list;
    }

    // 🔹 Obtener inmueble por Id
    public async Task<Inmueble?> GetByIdAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM propiedades WHERE Id = @Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        if (await rd.ReadAsync())
        {
            return new Inmueble
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                Direccion = rd.GetString(rd.GetOrdinal("Direccion")),
                Tipo = rd.GetString(rd.GetOrdinal("Tipo")),
                Uso = rd.GetString(rd.GetOrdinal("Uso")),
                Ambientes = rd.GetInt32(rd.GetOrdinal("Ambientes")),
                Precio = rd.GetDecimal(rd.GetOrdinal("Precio")),
                IdPropietario = rd.GetInt32(rd.GetOrdinal("IdPropietario"))
            };
        }
        return null;
    }

    // 🔹 Crear inmueble
    public async Task CreateAsync(Inmueble i)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO propiedades 
            (Direccion, Tipo, Uso, Ambientes, Precio, IdPropietario)
            VALUES (@Direccion, @Tipo, @Uso, @Ambientes, @Precio, @IdPropietario)";

        var c = (MySqlCommand)cmd;
        c.Parameters.AddWithValue("@Direccion", i.Direccion);
        c.Parameters.AddWithValue("@Tipo", i.Tipo);
        c.Parameters.AddWithValue("@Uso", i.Uso);
        c.Parameters.AddWithValue("@Ambientes", i.Ambientes);
        c.Parameters.AddWithValue("@Precio", i.Precio);
        c.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);

        await c.ExecuteNonQueryAsync();
    }

    // 🔹 Actualizar inmueble
    public async Task UpdateAsync(Inmueble i)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE propiedades SET 
            Direccion=@Direccion, Tipo=@Tipo, Uso=@Uso, Ambientes=@Ambientes,
            Precio=@Precio, IdPropietario=@IdPropietario
            WHERE Id=@Id";

        var c = (MySqlCommand)cmd;
        c.Parameters.AddWithValue("@Id", i.Id);
        c.Parameters.AddWithValue("@Direccion", i.Direccion);
        c.Parameters.AddWithValue("@Tipo", i.Tipo);
        c.Parameters.AddWithValue("@Uso", i.Uso);
        c.Parameters.AddWithValue("@Ambientes", i.Ambientes);
        c.Parameters.AddWithValue("@Precio", i.Precio);
        c.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);

        await c.ExecuteNonQueryAsync();
    }

    // 🔹 Eliminar inmueble
    public async Task DeleteAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM propiedades WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    // 🔹 NUEVO: Obtener inmuebles disponibles por Tipo y Uso
    public async Task<List<Inmueble>> GetDisponiblesAsync(string tipo, string uso)
    {
        var list = new List<Inmueble>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        SELECT i.*, CONCAT(p.Nombre, ' ', p.Apellido) AS PropietarioNombre
        FROM Propiedades i
        JOIN Propietarios p ON i.IdPropietario = p.Id
        WHERE i.Tipo = @Tipo
          AND i.Uso = @Uso
          AND i.Id NOT IN (
              SELECT c.IdPropiedad
              FROM Contratos c
              WHERE CURDATE() BETWEEN c.FechaInicio AND c.FechaFin
          )
        ORDER BY i.Direccion";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Tipo", tipo);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Uso", uso);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new Inmueble
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                Direccion = rd.GetString(rd.GetOrdinal("Direccion")),
                Tipo = rd.GetString(rd.GetOrdinal("Tipo")),
                Uso = rd.GetString(rd.GetOrdinal("Uso")),
                Ambientes = rd.GetInt32(rd.GetOrdinal("Ambientes")),
                Precio = rd.GetDecimal(rd.GetOrdinal("Precio")),
                IdPropietario = rd.GetInt32(rd.GetOrdinal("IdPropietario")),
                PropietarioNombre = rd.IsDBNull(rd.GetOrdinal("PropietarioNombre")) ? "" : rd.GetString(rd.GetOrdinal("PropietarioNombre"))
            });
        }

        return list;
    }

}
