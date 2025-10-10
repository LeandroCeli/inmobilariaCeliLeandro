using inmobilariaCeli.Data;
using inmobilariaCeli.Models;
using MySql.Data.MySqlClient;

namespace inmobilariaCeli.Repositories
{
    public class PagoRepository
    {
        private readonly DbConnectionFactory _factory;
        public PagoRepository(DbConnectionFactory factory) => _factory = factory;

        // 🔹 Obtener todos los pagos por contrato
        public async Task<List<Pago>> GetByContratoIdAsync(int contratoId)
        {
            var list = new List<Pago>();
            using var conn = _factory.Create();
            await ((MySqlConnection)conn).OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT p.*, 
                       i.Nombre, i.Apellido,
                       prop.Direccion
                FROM Pagos p
                JOIN contratos c ON p.IdContrato = c.Id
                JOIN inquilinos i ON c.IdInquilino = i.Id
                JOIN propiedades prop ON c.IdPropiedad = prop.Id
                WHERE p.IdContrato = @id
                ORDER BY p.NumeroPago ASC";
            ((MySqlCommand)cmd).Parameters.AddWithValue("@id", contratoId);

            using var rd = await ((MySqlCommand)cmd).ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Pago
                {
                    Id = rd.GetInt32(rd.GetOrdinal("Id")),

                    NumeroPago = rd.GetInt32(rd.GetOrdinal("NumeroPago")),

                    IdContrato = rd.GetInt32(rd.GetOrdinal("IdContrato")),

                    FechaPago = rd.GetDateTime(rd.GetOrdinal("FechaPago")),

                    Importe = rd.GetDecimal(rd.GetOrdinal("Importe")),

                    Detalle = rd.IsDBNull(rd.GetOrdinal("Detalle")) ? "" : rd.GetString(rd.GetOrdinal("Detalle")),

                    Pagado = rd.GetBoolean(rd.GetOrdinal("Pagado")),


                    InquilinoNombre = $"{rd.GetString(rd.GetOrdinal("Nombre"))} {rd.GetString(rd.GetOrdinal("Apellido"))}",

                    PropiedadDireccion = rd.GetString(rd.GetOrdinal("Direccion"))
                });
            }
            return list;
        }

        // 🔹 Contar cuántos pagos existen para un contrato
        public async Task<int> ContarPagosPorContratoAsync(int contratoId)
        {
            using var conn = _factory.Create();
            await ((MySqlConnection)conn).OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM pagos WHERE IdContrato = @idContrato";
            ((MySqlCommand)cmd).Parameters.AddWithValue("@idContrato", contratoId);

            var result = await ((MySqlCommand)cmd).ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        // 🔹 Crear un nuevo pago
        public async Task CreateAsync(Pago p)
        {
            using var conn = _factory.Create();
            await ((MySqlConnection)conn).OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO pagos (NumeroPago, IdContrato, FechaPago, Importe, Detalle, Pagado)
                VALUES (@NumeroPago, @IdContrato, @FechaPago, @Importe, @Detalle, @Pagado)";
            var sql = (MySqlCommand)cmd;
            sql.Parameters.AddWithValue("@NumeroPago", p.NumeroPago);
            sql.Parameters.AddWithValue("@IdContrato", p.IdContrato);
            sql.Parameters.AddWithValue("@FechaPago", p.FechaPago);
            sql.Parameters.AddWithValue("@Importe", p.Importe);
            sql.Parameters.AddWithValue("@Detalle", p.Detalle ?? "");
            sql.Parameters.AddWithValue("@Pagado", p.Pagado);

            await sql.ExecuteNonQueryAsync();
        }

        // 🔹 Marcar un pago como abonado
        public async Task SetPagadoAsync(int idPago)
        {
            using var conn = _factory.Create();
            await ((MySqlConnection)conn).OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE pagos SET Pagado = 1, FechaPago = @Fecha WHERE Id = @Id";
            ((MySqlCommand)cmd).Parameters.AddWithValue("@Fecha", DateTime.Now);
            ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", idPago);

            await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
        }

        // 🔹 Eliminar un pago
        public async Task DeleteAsync(int id)
        {
            using var conn = _factory.Create();
            await ((MySqlConnection)conn).OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM pagos WHERE Id = @Id";
            ((MySqlCommand)cmd).Parameters.AddWithValue("@Id", id);

            await ((MySqlCommand)cmd).ExecuteNonQueryAsync();
        }
    }
}
