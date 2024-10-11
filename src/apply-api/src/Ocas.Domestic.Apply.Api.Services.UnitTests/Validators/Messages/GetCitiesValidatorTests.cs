using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class GetCitiesValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly GetCitiesValidator _validator;

        public GetCitiesValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new GetCitiesValidator(_lookupsCache);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCitiesValidator_ShouldPass_WhenNullProvince()
        {
            var user = new Mock<IPrincipal>();

            var model = new Faker<GetCities>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ProvinceId, _ => null)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCitiesValidator_ShouldPass_WhenValidProvince()
        {
            var user = new Mock<IPrincipal>();
            var provinces = await _lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);

            var model = new Faker<GetCities>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ProvinceId, f => f.PickRandom(provinces).Id)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCitiesValidator_ShouldFail_WhenInvalidProvince()
        {
            var user = new Mock<IPrincipal>();
            var provinceId = Guid.NewGuid();

            var model = new Faker<GetCities>()
                .RuleFor(o => o.User, _ => user.Object)
                .RuleFor(o => o.ProvinceId, _ => provinceId)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Province Id' does not exist: {provinceId}");
        }
    }
}
