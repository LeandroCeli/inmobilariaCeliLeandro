using MySql.Data.MySqlClient;
using System.Data;

namespace inmobilariaCeli.Data;

public class DbConnectionFactory
{
    private readonly string _connectionString;
    public DbConnectionFactory(string connectionString) => _connectionString = connectionString;

    public IDbConnection Create() => new MySqlConnection(_connectionString);
}
