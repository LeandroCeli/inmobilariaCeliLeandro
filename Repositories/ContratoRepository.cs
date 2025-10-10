using inmobilariaCeli.Data;
using inmobilariaCeli.Models;
using MySql.Data.MySqlClient;

namespace inmobilariaCeli.Repositories;

public class ContratoRepository
{
    private readonly DbConnectionFactory _factory;
    public ContratoRepository(DbConnectionFactory factory) => _factory = factory;

    // ✅ Calcula la cantidad de cuotas (mensuales)
    private int CalcularCuotas(DateTime inicio, DateTime fin)
    {
        int meses = ((fin.Year - inicio.Year) * 12) + (fin.Month - inicio.Month);
        return meses <= 0 ? 1 : meses;
    }

    // ✅ Listar todos los contratos
    public async Task<List<Contrato>> GetAllAsync()
    {
        var list = new List<Contrato>();
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT c.*,  
                        CONCAT(i.Nombre, ' ', i.Apellido) AS InquilinoNombre,
                        p.Direccion AS PropiedadDireccion
                        FROM contratos c
                        JOIN inquilinos i ON c.IdInquilino = i.Id
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
                MontoMensual = rd.GetDecimal(rd.GetOrdinal("MontoMensual")),
                Deposito = rd.GetDecimal(rd.GetOrdinal("Deposito")),
                Cuotas = rd.IsDBNull(rd.GetOrdinal("Cuotas")) ? 0 : rd.GetInt32(rd.GetOrdinal("Cuotas")),
                InquilinoNombre = rd.IsDBNull(rd.GetOrdinal("InquilinoNombre")) ? "Sin nombre" : rd.GetString(rd.GetOrdinal("InquilinoNombre")),
                PropiedadDireccion = rd.IsDBNull(rd.GetOrdinal("PropiedadDireccion")) ? "Sin dirección" : rd.GetString(rd.GetOrdinal("PropiedadDireccion")),
                RegistradoPor = rd.IsDBNull(rd.GetOrdinal("RegistradoPor")) ? "Desconocido" : rd.GetString(rd.GetOrdinal("RegistradoPor")),

                // 🔑 Campos que faltaban
                DadoDeBaja = rd.GetInt32(rd.GetOrdinal("DadoDeBaja")) == 1,
                FechaBaja = rd.IsDBNull(rd.GetOrdinal("FechaBaja")) ? null : rd.GetDateTime(rd.GetOrdinal("FechaBaja")),
                UsuarioBaja = rd.IsDBNull(rd.GetOrdinal("UsuarioBaja")) ? null : rd.GetString(rd.GetOrdinal("UsuarioBaja"))
            });
        }
        return list;
    }
    // ✅ Obtener contrato por Id
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
                Deposito = rd.GetDecimal(rd.GetOrdinal("Deposito")),
                Cuotas = rd.IsDBNull(rd.GetOrdinal("Cuotas")) ? 0 : rd.GetInt32(rd.GetOrdinal("Cuotas")),
                RegistradoPor = rd.IsDBNull(rd.GetOrdinal("RegistradoPor")) ? null : rd.GetString(rd.GetOrdinal("RegistradoPor"))
            };
        }
        return null;
    }

    // ✅ Crear contrato (guarda quién lo registró)
    public async Task CreateAsync(Contrato c)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        c.Cuotas = CalcularCuotas(c.FechaInicio, c.FechaFin);

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Contratos 
            (IdPropiedad, IdInquilino, FechaInicio, FechaFin, MontoMensual, Deposito, Cuotas, RegistradoPor)
            VALUES (@IdPropiedad, @IdInquilino, @FechaInicio, @FechaFin, @MontoMensual, @Deposito, @Cuotas, @RegistradoPor)";

        var sql = (MySqlCommand)cmd;
        sql.Parameters.AddWithValue("@IdPropiedad", c.IdPropiedad);
        sql.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
        sql.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
        sql.Parameters.AddWithValue("@FechaFin", c.FechaFin);
        sql.Parameters.AddWithValue("@MontoMensual", c.MontoMensual);
        sql.Parameters.AddWithValue("@Deposito", c.Deposito);
        sql.Parameters.AddWithValue("@Cuotas", c.Cuotas);
        sql.Parameters.AddWithValue("@RegistradoPor", c.RegistradoPor ?? "Desconocido");

        await sql.ExecuteNonQueryAsync();
    }

    // ✅ Actualizar contrato
    public async Task UpdateAsync(Contrato c)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        c.Cuotas = CalcularCuotas(c.FechaInicio, c.FechaFin);

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Contratos SET 
            IdPropiedad=@IdPropiedad, IdInquilino=@IdInquilino,
            FechaInicio=@FechaInicio, FechaFin=@FechaFin,
            MontoMensual=@MontoMensual, Deposito=@Deposito, Cuotas=@Cuotas
            WHERE Id=@Id";

        var sql = (MySqlCommand)cmd;
        sql.Parameters.AddWithValue("@Id", c.Id);
        sql.Parameters.AddWithValue("@IdPropiedad", c.IdPropiedad);
        sql.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
        sql.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
        sql.Parameters.AddWithValue("@FechaFin", c.FechaFin);
        sql.Parameters.AddWithValue("@MontoMensual", c.MontoMensual);
        sql.Parameters.AddWithValue("@Deposito", c.Deposito);
        sql.Parameters.AddWithValue("@Cuotas", c.Cuotas);

        await sql.ExecuteNonQueryAsync();
    }

    // ✅ Eliminar contrato
    public async Task DeleteAsync(int id)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Contratos WHERE Id=@Id";
        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

    public async Task<Contrato?> GetByIdConDetallesAsync(int id)
{
    using var conn = _factory.Create();
    await ((MySqlConnection)conn).OpenAsync();

    using var cmd = conn.CreateCommand();
    cmd.CommandText = @"
        SELECT c.*,  
               CONCAT(i.Nombre, ' ', i.Apellido) AS InquilinoNombre,
               p.Direccion AS PropiedadDireccion
        FROM contratos c
        JOIN inquilinos i ON c.IdInquilino = i.Id
        JOIN propiedades p ON c.IdPropiedad = p.Id
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
            Cuotas = rd.IsDBNull(rd.GetOrdinal("Cuotas")) ? 0 : rd.GetInt32(rd.GetOrdinal("Cuotas")),
            InquilinoNombre = rd.GetString(rd.GetOrdinal("InquilinoNombre")),
            PropiedadDireccion = rd.GetString(rd.GetOrdinal("PropiedadDireccion")),
            RegistradoPor = rd.IsDBNull(rd.GetOrdinal("RegistradoPor")) ? "Desconocido" : rd.GetString(rd.GetOrdinal("RegistradoPor")),

            // 🔑 Campos de rescisión
            DadoDeBaja = rd.GetInt32(rd.GetOrdinal("DadoDeBaja")) == 1,
            FechaBaja = rd.IsDBNull(rd.GetOrdinal("FechaBaja")) ? null : rd.GetDateTime(rd.GetOrdinal("FechaBaja")),
            UsuarioBaja = rd.IsDBNull(rd.GetOrdinal("UsuarioBaja")) ? null : rd.GetString(rd.GetOrdinal("UsuarioBaja"))
        };
    }
    return null;
}

    // ✅ Verificar superposición de fechas
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
        return result > 0;
    }
    public async Task RescindirContratoAsync(int idContrato, string usuario)
    {
        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        UPDATE contratos
        SET DadoDeBaja = 1,
            FechaBaja = @FechaBaja,
            UsuarioBaja = @UsuarioBaja
        WHERE Id = @Id";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", idContrato);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@FechaBaja", DateTime.Now);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@UsuarioBaja", usuario ?? "Desconocido");

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }


    public async Task RescindirYLiberarAsync(int idContrato, string usuario)
    {
        var contrato = await GetByIdAsync(idContrato);
        if (contrato == null) return;

        using var conn = _factory.Create();
        await ((MySqlConnection)conn).OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        UPDATE contratos
        SET DadoDeBaja = 1,
            FechaBaja = @FechaBaja,
            UsuarioBaja = @UsuarioBaja
        WHERE Id = @Id;

        UPDATE propiedades
        SET Disponible = 1
        WHERE Id = @IdPropiedad;";

        ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", idContrato);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@FechaBaja", DateTime.Now);
        ((MySqlCommand)cmd).Parameters.AddWithValue("@UsuarioBaja", usuario ?? "Desconocido");
        ((MySqlCommand)cmd).Parameters.AddWithValue("@IdPropiedad", contrato.IdPropiedad);

        await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
    }

}
