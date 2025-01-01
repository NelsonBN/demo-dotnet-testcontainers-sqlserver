using System.Data;
using Api;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddScoped<IDbConnection>(sp =>
        new SqlConnection(sp.GetRequiredService<IConfiguration>().GetConnectionString("Default")));

var app = builder.Build();



app.MapGet("/products", async (IDbConnection connection) =>
{
    var result = await connection.QueryAsync<ProductResponse>(
        """
        SELECT
            Id,
            Name,
            Quantity
        FROM Product ;
        """);

    return Results.Ok(result);
});


app.MapGet("/products/{id:int}", async (IDbConnection connection, int id) =>
{
    var result = await connection.QuerySingleOrDefaultAsync<ProductResponse>(
        """
        SELECT
            Id,
            Name,
            Quantity
        FROM Product
        WHERE Id = @id ;
        """,
        new { id });

    if(result is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result);
}).WithName("GetProduct");


app.MapPost("/products", async (IDbConnection connection, ProductRequest product) =>
{
    var id = await connection.ExecuteScalarAsync<int>(
        """
            INSERT INTO Product (Name , Quantity )
                         VALUES (@Name, @Quantity)
            SELECT CAST(SCOPE_IDENTITY() as int);
            """,
        product);

    return TypedResults.CreatedAtRoute(
        id,
        "GetProduct",
        new { id });
});


app.MapPut("/products/{id:int}", async (IDbConnection connection, int id, ProductRequest product) =>
{
    var rows = await connection.ExecuteAsync(
        """
        UPDATE Product
           SET Name = @Name,
               Quantity = @Quantity
        WHERE Id = @id ;
        """,
        new
        {
            id,
            product.Name,
            product.Quantity
        });

    if(rows == 0)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});


app.MapDelete("/products/{id:int}", async (IDbConnection connection, int id) =>
{
    var rows = await connection.ExecuteAsync(
        """
        DELETE FROM Product
        WHERE Id = @id ;
        """,
        new { id });

    if(rows == 0)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

await app.RunAsync();
