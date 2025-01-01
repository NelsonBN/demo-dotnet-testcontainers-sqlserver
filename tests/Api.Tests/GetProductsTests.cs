using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Tests.Config;

namespace Api.Tests;

[Collection(nameof(CollectionIntegrationTests))]
public sealed class GetProductsTests(IntegrationTestsFactory factory)
{
    private readonly IntegrationTestsFactory _factory = factory;


    [Fact]
    public async Task When_get_products_then_return_200_and_products()
    {
        // Arrange && Act
        var act = await _factory.CreateClient()
            .GetAsync("/products");


        // Assert
        act.Should()
           .Be200Ok()
           .And.Satisfy<IEnumerable<ProductResponse>>(model =>
                model.Should().HaveCountGreaterThan(90));
    }
}
