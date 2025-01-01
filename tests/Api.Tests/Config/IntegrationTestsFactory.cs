using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace Api.Tests.Config;

public sealed class IntegrationTestsFactory : WebApplicationFactory<ProductRequest>, IAsyncLifetime
{
    private const string DB_NAME = "demo";
    private readonly MsSqlContainer _container;

    public IntegrationTestsFactory()
        => _container = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder
            .ConfigureTestServices(services =>
            {
                services.RemoveAll<IDbConnection>();
                services.AddScoped<IDbConnection>(sp => new SqlConnection(GetConnectionString()));
            });

    public string GetConnectionString()
        => _container
            .GetConnectionString()
            .Replace("Database=master", $"Database={DB_NAME}");

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var script = await File.ReadAllTextAsync(Path.GetFullPath("./data/init-db.sql"));
        await _container.ExecScriptAsync(script);
    }

    public new Task DisposeAsync() => _container.StopAsync();
}

[CollectionDefinition(nameof(CollectionIntegrationTests))]
public sealed class CollectionIntegrationTests : ICollectionFixture<IntegrationTestsFactory> { }
