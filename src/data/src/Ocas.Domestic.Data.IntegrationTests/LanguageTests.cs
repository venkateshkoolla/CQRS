using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class LanguageTests : BaseTest
    {
        //No languages exits within CRM CI/QA instance
        [Fact]
        public async Task GetLanguages_ShouldPass()
        {
            var enResult = await Context.GetLanguages(Locale.English);
            var frResult = await Context.GetLanguages(Locale.French);

            frResult.Should().HaveCount(enResult.Count);
        }
    }
}
