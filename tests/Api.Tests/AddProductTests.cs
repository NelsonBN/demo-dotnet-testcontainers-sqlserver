using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Tests.Config;

namespace Api.Tests;

[Collection(nameof(CollectionIntegrationTests))]
public sealed class AddProductTests(IntegrationTestsFactory factory)
{
    private readonly IntegrationTestsFactory _factory = factory;


    [Fact]
    public async Task When_add_new_product_then_return_201_and_id()
    {
        // Arrange
        var product = new ProductRequest
        {
            Name = "Redmi Note 10",
            Quantity = 10
        };


        // Act
        var act = await _factory.CreateClient()
            .PostAsync("/products", JsonContent.Create(product));


        // Assert
        act.Should()
           .Be201Created()
           .And.Satisfy<ulong>(model =>
                model.Should().BeGreaterThan(0));
    }
}
