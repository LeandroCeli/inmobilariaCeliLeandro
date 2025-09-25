using MySql.Data.MySqlClient;
using inmobilariaCeli.Data;
using inmobilariaCeli.Models;

namespace inmobilariaCeli.Repositories;

public class ContratoRepository
{
    private readonly DbConnectionFactory _factory;
    public ContratoRepository(DbConnectionFactory factory) => _factory = factory;

    // Listar todos los contratos con datos auxiliares
    public async Task<List<Contrato>> GetAll()
    {
        var list = new List<Contrato>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT c.Id, c.IdPropiedad, c.IdInquilino, c.FechaInicio, c.FechaFin,
                   c.MontoMensual, c.Deposito,
                   p.Direccion AS PropiedadDireccion,
                   CONCAT(q.Nombre, ' ', q.Apellido) AS InquilinoNombre
            FROM Contratos c
            JOIN Propiedades p ON c.IdPropiedad = p.Id
            JOIN Inquilinos q ON c.IdInquilino = q.Id
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
                MontoMensual = rd.GetDecimal(rd.GetOrdinal("MontoMensual")),
                Deposito = rd.GetDecimal(rd.GetOrdinal("Deposito")),
                PropiedadDireccion = rd.GetString(rd.GetOrdinal("PropiedadDireccion")),
                InquilinoNombre = rd.GetString(rd.GetOrdinal("InquilinoNombre"))
            });
        }
        return list;
    }

    // Buscar contrato por Id con datos auxiliares
    public async Task<Contrato?> GetById(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT c.Id, c.IdPropiedad, c.IdInquilino, c.FechaInicio, c.FechaFin,
                   c.MontoMensual, c.Deposito,
                   p.Direccion AS PropiedadDireccion,
                   CONCAT(q.Nombre, ' ', q.Apellido) AS InquilinoNombre
            FROM Contratos c
            JOIN Propiedades p ON c.IdPropiedad = p.Id
            JOIN Inquilinos q ON c.IdInquilino = q.Id
            WHERE c.Id = @Id";

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
                Deposito = rd.GetDecimal(rd.GetOrdinal("Deposito")),
                PropiedadDireccion = rd.GetString(rd.GetOrdinal("PropiedadDireccion")),
                InquilinoNombre = rd.GetString(rd.GetOrdinal("InquilinoNombre"))
            };
        }
        return null;
    }

    // Alta contrato
    public async Task<int> AltaContrato(Contrato cto)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Contratos 
            (IdPropiedad, IdInquilino, FechaInicio, FechaFin, MontoMensual, Deposito)
            VALUES (@IdPropiedad, @IdInquilino, @Inicio, @Fin, @Monto, @Deposito);
            SELECT LAST_INSERT_ID();";

        var c = (MySqlCommand)cmd;
        c.Parameters.AddWithValue("@IdPropiedad", cto.IdPropiedad);
        c.Parameters.AddWithValue("@IdInquilino", cto.IdInquilino);
        c.Parameters.AddWithValue("@Inicio", cto.FechaInicio);
        c.Parameters.AddWithValue("@Fin", cto.FechaFin);
        c.Parameters.AddWithValue("@Monto", cto.MontoMensual);
        c.Parameters.AddWithValue("@Deposito", cto.Deposito);

        var id = Convert.ToInt32(await c.ExecuteScalarAsync());
        return id;
    }

    // Actualizar contrato
    public async Task ActualizarContrato(Contrato cto)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Contratos SET
            IdPropiedad=@IdPropiedad, IdInquilino=@IdInquilino,
            FechaInicio=@Inicio, FechaFin=@Fin,
            MontoMensual=@Monto, Deposito=@Deposito
            WHERE Id=@Id";

        var c = (MySqlCommand)cmd;
        c.Parameters.AddWithValue("@Id", cto.Id);
        c.Parameters.AddWithValue("@IdPropiedad", cto.IdPropiedad);
        c.Parameters.AddWithValue("@IdInquilino", cto.IdInquilino);
        c.Parameters.AddWithValue("@Inicio", cto.FechaInicio);
        c.Parameters.AddWithValue("@Fin", cto.FechaFin);
        c.Parameters.AddWithValue("@Monto", cto.MontoMensual);
        c.Parameters.AddWithValue("@Deposito", cto.Deposito);

        await c.ExecuteNonQueryAsync();
    }

    // Eliminar contrato
    public async Task DeleteContrato(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Contratos WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }
}