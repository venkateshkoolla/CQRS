using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProductTests : BaseTest
    {
        [Fact]
        public async Task GetProducts_ShouldPass()
        {
            var enResult = await Context.GetProducts(ProductServiceType.AdministrationFee);
            enResult.Should().HaveCountGreaterOrEqualTo(DataFakerFixture.SeedData.Products.Count(x => x.ProductServiceType == ProductServiceType.AdministrationFee));
        }

        [Fact]
        public async Task GetProduct_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Products);
            var product = await Context.GetProduct(result.Id);

            product.Should().BeEquivalentTo(result);
        }
    }
}
