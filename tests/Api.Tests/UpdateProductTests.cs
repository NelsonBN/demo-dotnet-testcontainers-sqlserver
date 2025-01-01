using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Tests.Config;

namespace Api.Tests;

[Collection(nameof(CollectionIntegrationTests))]
public sealed class UpdateProductTests(IntegrationTestsFactory factory)
{
    private readonly IntegrationTestsFactory _factory = factory;


    [Fact]
    public async Task When_update_existing_product_then_return_204()
    {
        // Arrange
        var id = 57;

        var product = new ProductRequest
        {
            Name = "New Drone",
            Quantity = 5
        };


        // Act
        var act = await _factory.CreateClient()
            .PutAsync(
                $"/products/{id}",
                JsonContent.Create(product));


        // Assert
        act.Should().Be204NoContent();
    }

    [Fact]
    public async Task When_update_nonexisting_product_then_return_404()
    {
        // Arrange
        var id = 204;

        var product = new ProductRequest
        {
            Name = "Another Drone",
            Quantity = 5
        };


        // Act
        var act = await _factory.CreateClient()
            .PutAsync(
                $"/products/{id}",
                JsonContent.Create(product));


        // Assert
        act.Should().Be404NotFound();
    }
}
