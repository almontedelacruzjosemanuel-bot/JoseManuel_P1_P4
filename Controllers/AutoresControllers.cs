using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Parcial1_P4_JoseManuel.Models;

namespace Parcial1_P4_JoseManuel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly string connectionString = "Data Source=editorial.db";

    // GET: api/autores
    [HttpGet]
    public async Task<IActionResult> GetAutores()
    {
        using var connection = new SqliteConnection(connectionString);

        string sql = "SELECT * FROM Autores";

        var autores = await connection.QueryAsync<Autor>(sql);

        return Ok(autores);
    }

    // GET: api/autores/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAutor(long id)
    {
        using var connection = new SqliteConnection(connectionString);

        string sql = @"
            SELECT
                IdAutor,
                Nombres,
                Nacionalidades,
                Fechas,
                Sueldos
            FROM Autores
            WHERE IdAutor = @Id";

        var autor = await connection.QueryFirstOrDefaultAsync<Autor>(
            sql,
            new { Id = id }
        );

        if (autor == null)
        {
            return NotFound();
        }

        return Ok(autor);
    }

    // POST: api/autores
    [HttpPost]
    public async Task<IActionResult> CrearAutor(Autor autor)
    {
        using var connection = new SqliteConnection(connectionString);

        string sql = @"
            INSERT INTO Autores
            (Nombres, Nacionalidades, Fechas, Sueldos)
            VALUES
            (@Nombres, @Nacionalidades, @Fechas, @Sueldos);

            SELECT last_insert_rowid();
        ";

        // ExecuteScalar obtiene el ID generado
        var id = await connection.ExecuteScalarAsync<long>(
            sql,
            autor
        );

        return Ok(new
        {
            IdAutor = id,
            autor.Nombres,
            autor.Nacionalidades,
            autor.Fechas,
            autor.Sueldos
        });
    }

    // PUT: api/autores/1
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarAutor(
        long id,
        Autor autor)
    {
        using var connection = new SqliteConnection(connectionString);

        string sql = @"
            UPDATE Autores
            SET
                Nombres = @Nombres,
                Nacionalidades = @Nacionalidades,
                Fechas = @Fechas,
                Sueldos = @Sueldos
            WHERE IdAutor = @Id";

        var filas = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                autor.Nombres,
                autor.Nacionalidades,
                autor.Fechas,
                autor.Sueldos
            }
        );

        if (filas == 0)
        {
            return NotFound();
        }

        return Ok();
    }

    // DELETE: api/autores/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarAutor(long id)
    {
        using var connection = new SqliteConnection(connectionString);

        string sql = "DELETE FROM Autores WHERE IdAutor = @Id";

        var filas = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        if (filas == 0)
        {
            return NotFound();
        }

        return Ok();
    }
}