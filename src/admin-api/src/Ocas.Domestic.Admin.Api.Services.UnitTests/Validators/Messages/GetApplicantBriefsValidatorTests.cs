using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class GetApplicantBriefsValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly ILookupsCache _lookupsCache;
        private readonly TestFramework.ModelFakerFixture _modelFakerFixture;
        private readonly GetApplicantBriefsValidator _validator;

        public GetApplicantBriefsValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _validator = new GetApplicantBriefsValidator(_lookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefsValidator_ShouldPass()
        {
            // Arrange
            var model = new GetApplicantBriefs
            {
                Params = new Faker<GetApplicantBriefOptions>()
                        .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(_modelFakerFixture.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id)
                        .RuleFor(o => o.BirthDate, f => f.Date.Past(2, System.DateTime.UtcNow).AsUtc().ToStringOrDefault())
                        .Generate(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefsValidator_ShouldFail_When_Null()
        {
            // Arrange
            var model = new GetApplicantBriefs
            {
                Params = null,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Params' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetApplicantBriefsValidator_ShouldFail_When_Empty()
        {
            // Arrange
            var model = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Params' must not be empty.");
        }
    }
}
