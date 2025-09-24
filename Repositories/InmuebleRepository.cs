using inmobilariaCeli.Models;
using inmobilariaCeli.Data;
using MySql.Data.MySqlClient;

namespace inmobilariaCeli.Repositories;

public class InmuebleRepository
{
    private readonly DbConnectionFactory _factory;
    public InmuebleRepository(DbConnectionFactory factory) => _factory = factory;

    // Obtener todos
    public async Task<List<Inmueble>> GetAllAsync()
    {
        var list = new List<Inmueble>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT i.*, CONCAT(p.Nombre, ' ', p.Apellido) AS PropietarioNombre
                            FROM Propiedades  i
                            JOIN Propietarios p ON i.IdPropietario = p.Id
                            ORDER BY i.Direccion";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new Inmueble
            {   Id =rd.GetInt32(rd.GetOrdinal("Id")),
                Direccion = rd.GetString(rd.GetOrdinal("Direccion")),
                Tipo = rd.GetString(rd.GetOrdinal("Tipo")),
                Uso = rd.GetString (rd.GetOrdinal("Uso")),
                Ambientes = rd.GetInt32(rd.GetOrdinal("Ambientes")),
                Precio = rd.GetDecimal(rd.GetOrdinal("Precio")),
                IdPropietario = rd.GetInt32(rd.GetOrdinal("IdPropietario")),
                PropietarioNombre = rd.GetString(rd.GetOrdinal("PropietarioNombre"))
            });
        }
        return list;
    }

    // Obtener por Id
    public async Task<Inmueble?> GetByIdAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT i.*, CONCAT(p.Nombre, ' ', p.Apellido) AS PropietarioNombre
                            FROM Propiedades  i
                            JOIN Propietarios p ON i.IdPropietario = p.Id
                            WHERE i.Id = @Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        if (await rd.ReadAsync())
        {
            return new Inmueble
            {
                Id =rd.GetInt32(rd.GetOrdinal("Id")),
                Direccion = rd.GetString(rd.GetOrdinal("Direccion")),
                Tipo = rd.GetString(rd.GetOrdinal("Tipo")),
                Uso = rd.GetString (rd.GetOrdinal("Uso")),
                Ambientes = rd.GetInt32(rd.GetOrdinal("Ambientes")),
                Precio = rd.GetDecimal(rd.GetOrdinal("Precio")),
                IdPropietario = rd.GetInt32(rd.GetOrdinal("IdPropietario")),
                PropietarioNombre = rd.GetString(rd.GetOrdinal("PropietarioNombre"))
            };
        }
        return null;
    }

    // Crear
    public async Task CreateAsync(Inmueble i)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Propiedades  
            (Direccion, Tipo, Uso, Ambientes, Precio, IdPropietario) 
            VALUES (@Direccion, @Tipo, @Uso, @Ambientes, @Precio, @IdPropietario)";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Direccion", i.Direccion);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Tipo", i.Tipo);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Uso", i.Uso);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Ambientes", i.Ambientes);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Precio", i.Precio);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@IdPropietario", i.IdPropietario);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    // Editar
    public async Task UpdateAsync(Inmueble i)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Propiedades  SET 
            Direccion=@Direccion, Tipo=@Tipo, Uso=@Uso, 
            Ambientes=@Ambientes, Precio=@Precio, IdPropietario=@IdPropietario 
            WHERE Id=@Id";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", i.Id);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Direccion", i.Direccion);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Tipo", i.Tipo);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Uso", i.Uso);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Ambientes", i.Ambientes);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Precio", i.Precio);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@IdPropietario", i.IdPropietario);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    // Eliminar
    public async Task DeleteAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Propiedades  WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }
}