using Microsoft.Data.Sqlite;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Crear la base de datos y la tabla
using (var connection = new SqliteConnection("Data Source=editorial.db"))
{
    connection.Open();

    var sql = @"
        CREATE TABLE IF NOT EXISTS Autores
        (
            IdAutor INTEGER PRIMARY KEY AUTOINCREMENT,
            Nombres TEXT NOT NULL,
            Nacionalidades TEXT NOT NULL,
            Fechas TEXT NOT NULL,
            Sueldos REAL NOT NULL
        );
    ";

    using var command = new SqliteCommand(sql, connection);
    command.ExecuteNonQuery();
}

app.MapOpenApi();

app.MapScalarApiReference();

app.MapControllers();

app.Run();