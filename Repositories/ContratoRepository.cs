using inmobilariaCeli.Data;
using inmobilariaCeli.Models;
using MySql.Data.MySqlClient;

namespace inmobilariaCeli.Repositories;

public class ContratoRepository
{
    private readonly DbConnectionFactory _factory;
    public ContratoRepository(DbConnectionFactory factory) => _factory = factory;

    // Listar todos los contratos con datos auxiliares
    public async Task<List<Contrato>> GetAllAsync()
    {
        var list = new List<Contrato>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT c.*, 
                            CONCAT(i.Nombre, ' ', i.Apellido) AS InquilinoNombre,
                            p.Direccion AS PropiedadDireccion
                            FROM Contratos c
                            JOIN Inquilinos i ON c.IdInquilino = i.Id
                            JOIN propiedades p ON c.IdPropiedad = p.Id
                            ORDER BY c.FechaInicio DESC";

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new Contrato
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                IdPropiedad = rd.GetInt32(rd.GetOrdinal("IdPropiedad")),
                IdInquilino = rd.GetInt32(rd.GetOrdinal("IdInquilino")),
                FechaInicio = rd.GetDateTime(rd.GetOrdinal("FechaInicio")),
                FechaFin = rd.GetDateTime(rd.GetOrdinal("FechaFin")),
                // TipoOcupacion = (TipoOcupacion)rd.GetInt32(rd.GetOrdinal("TipoOcupacion")),
                MontoMensual = rd.GetDecimal(rd.GetOrdinal("MontoMensual")),
                Deposito = rd.GetDecimal(rd.GetOrdinal("Deposito")),
                InquilinoNombre = rd.IsDBNull(rd.GetOrdinal("InquilinoNombre")) ? "Sin nombre" : rd.GetString(rd.GetOrdinal("InquilinoNombre")),
                PropiedadDireccion = rd.IsDBNull(rd.GetOrdinal("PropiedadDireccion")) ? "Sin dirección" : rd.GetString(rd.GetOrdinal("PropiedadDireccion"))
            });
        }
        return list;
    }




    // Obtener contrato por Id
    public async Task<Contrato?> GetByIdAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Contratos WHERE Id = @Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        if (await rd.ReadAsync())
        {
            return new Contrato
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                IdPropiedad = rd.GetInt32(rd.GetOrdinal("IdPropiedad")),
                IdInquilino = rd.GetInt32(rd.GetOrdinal("IdInquilino")),
                FechaInicio = rd.GetDateTime(rd.GetOrdinal("FechaInicio")),
                FechaFin = rd.GetDateTime(rd.GetOrdinal("FechaFin")),
                MontoMensual = rd.GetDecimal(rd.GetOrdinal("MontoMensual")),
                Deposito = rd.GetDecimal(rd.GetOrdinal("Deposito"))
            };
        }
        return null;
    }

    // Crear contrato
    public async Task CreateAsync(Contrato c)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Contratos 
            (IdPropiedad, IdInquilino, FechaInicio, FechaFin, MontoMensual, Deposito)
            VALUES (@IdPropiedad, @IdInquilino, @FechaInicio, @FechaFin, @MontoMensual, @Deposito)";

        var sql = (MySqlCommand)cmd;
        sql.Parameters.AddWithValue("@IdPropiedad", c.IdPropiedad);
        sql.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
        sql.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
        sql.Parameters.AddWithValue("@FechaFin", c.FechaFin);
       // sql.Parameters.AddWithValue("@TipoOcupacion", (int)c.TipoOcupacion);
        sql.Parameters.AddWithValue("@MontoMensual", c.MontoMensual);
        sql.Parameters.AddWithValue("@Deposito", c.Deposito);

        await sql.ExecuteNonQueryAsync();
    }

    // Actualizar contrato
    public async Task UpdateAsync(Contrato c)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Contratos SET 
            IdPropiedad=@IdPropiedad, IdInquilino=@IdInquilino,
            FechaInicio=@FechaInicio, FechaFin=@FechaFin,
            MontoMensual=@MontoMensual, Deposito=@Deposito
            WHERE Id=@Id";

        var sql = (MySqlCommand)cmd;
        sql.Parameters.AddWithValue("@Id", c.Id);
        sql.Parameters.AddWithValue("@IdPropiedad", c.IdPropiedad);
        sql.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
        sql.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
        sql.Parameters.AddWithValue("@FechaFin", c.FechaFin);
        sql.Parameters.AddWithValue("@TipoOcupacion", (int)c.TipoOcupacion);
       // sql.Parameters.AddWithValue("@MontoMensual", c.MontoMensual);
        sql.Parameters.AddWithValue("@Deposito", c.Deposito);

        await sql.ExecuteNonQueryAsync();
    }

    // Eliminar contrato
    public async Task DeleteAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Contratos WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    // Obtener contratos por IdPropiedad (para validar superposición de fechas)
    public async Task<List<Contrato>> GetByInmuebleIdAsync(int idPropiedad)
    {
        var list = new List<Contrato>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT Id, FechaInicio, FechaFin 
                            FROM Contratos 
                            WHERE IdPropiedad = @IdPropiedad";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@IdPropiedad", idPropiedad);

        using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new Contrato
            {
                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                FechaInicio = rd.GetDateTime(rd.GetOrdinal("FechaInicio")),
                FechaFin = rd.GetDateTime(rd.GetOrdinal("FechaFin"))
            });
        }
        return list;
    }
    
        // ✅ Verificar si existen contratos superpuestos en las fechas dadas
    public async Task<bool> HaySuperposicionAsync(int idPropiedad, DateTime fechaInicio, DateTime fechaFin)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT COUNT(*) 
            FROM Contratos
            WHERE IdPropiedad = @IdPropiedad
            AND (
                (@FechaInicio BETWEEN FechaInicio AND FechaFin)
                OR (@FechaFin BETWEEN FechaInicio AND FechaFin)
                OR (FechaInicio BETWEEN @FechaInicio AND @FechaFin)
                OR (FechaFin BETWEEN @FechaInicio AND @FechaFin)
            );";

        var sql = (MySqlCommand)cmd;
        sql.Parameters.AddWithValue("@IdPropiedad", idPropiedad);
        sql.Parameters.AddWithValue("@FechaInicio", fechaInicio);
        sql.Parameters.AddWithValue("@FechaFin", fechaFin);

        var result = Convert.ToInt32(await sql.ExecuteScalarAsync());
        return result > 0; // ✅ Devuelve true si hay superposición
    }

}