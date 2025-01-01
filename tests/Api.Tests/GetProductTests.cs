using System.Threading.Tasks;
using Api.Tests.Config;

namespace Api.Tests;

[Collection(nameof(CollectionIntegrationTests))]
public sealed class GetProductTests(IntegrationTestsFactory factory)
{
    private readonly IntegrationTestsFactory _factory = factory;


    [Fact]
    public async Task When_get_existing_product_then_return_200_and_product()
    {
        // Arrange
        var id = 41;


        // Act
        var act = await _factory.CreateClient()
            .GetAsync($"/products/{id}");


        // Assert
        act.Should()
           .Be200Ok()
           .And.Satisfy<ProductResponse>(model =>
                model.Should().Match<ProductResponse>(m =>
                    m.Id == 41 &&
                    m.Name == "Drone" &&
                    m.Quantity == 5));
    }

    [Fact]
    public async Task When_get_nonexisting_product_then_return_404()
    {
        // Arrange
        var id = 247;


        // Act
        var act = await _factory.CreateClient()
            .GetAsync($"/products/{id}");


        // Assert
        act.Should().Be404NotFound();
    }
}
