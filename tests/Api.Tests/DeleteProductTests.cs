using System.Threading.Tasks;
using Api.Tests.Config;

namespace Api.Tests;

[Collection(nameof(CollectionIntegrationTests))]
public sealed class DeleteProductTests(IntegrationTestsFactory factory)
{
    private readonly IntegrationTestsFactory _factory = factory;


    [Fact]
    public async Task When_delete_existing_product_then_return_204()
    {
        // Arrange
        var id = 11;


        // Act
        var act = await _factory.CreateClient()
            .DeleteAsync($"/products/{id}");


        // Assert
        act.Should().Be204NoContent();
    }

    [Fact]
    public async Task When_delete_nonexisting_product_then_return_404()
    {
        // Arrange
        var id = 651;


        // Act
        var act = await _factory.CreateClient()
            .DeleteAsync($"/products/{id}");


        // Assert
        act.Should().Be404NotFound();
    }
}
