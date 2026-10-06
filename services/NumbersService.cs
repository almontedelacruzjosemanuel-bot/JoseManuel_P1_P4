using Dapper;
using Microsoft.Data.Sqlite;
using Parcial1_P4_JoseManuel.Models;

namespace Parcial1_P4_JoseManuel.Services;

public class NumbersService(IConfiguration configuration)
{
    private readonly string connectionString =
        configuration.GetConnectionString("DefaultConnection")!;

    public async Task InitializeDatabase()
    {
        using var connection = new SqliteConnection(connectionString);

        const string sql = """
            CREATE TABLE IF NOT EXISTS NumberRecords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha TEXT NOT NULL,
                Numero REAL NOT NULL,
                Resultado REAL NOT NULL
            );
            """;

        await connection.ExecuteAsync(sql);
    }

    public async Task SaveNumber(double numero, double resultado)
    {
        using var connection = new SqliteConnection(connectionString);

        const string sql = """
            INSERT INTO NumberRecords (Fecha, Numero, Resultado)
            VALUES (@Fecha, @Numero, @Resultado);
            """;

        await connection.ExecuteAsync(sql, new
        {
            Fecha = DateTime.Now,
            Numero = numero,
            Resultado = resultado
        });
    }

    public async Task<IEnumerable<NumberRecord>> GetNumbers()
    {
        using var connection = new SqliteConnection(connectionString);

        const string sql = """
            SELECT Id, Fecha, Numero, Resultado
            FROM NumberRecords
            ORDER BY Id DESC;
            """;

        return await connection.QueryAsync<NumberRecord>(sql);
    }
}