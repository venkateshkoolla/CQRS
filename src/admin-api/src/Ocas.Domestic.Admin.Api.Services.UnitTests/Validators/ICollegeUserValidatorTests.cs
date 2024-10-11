using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators
{
    public class ICollegeUserValidatorTests
    {
        private readonly ICollegeUserValidator _validator;
        private readonly ILookupsCache _lookupsCache;
        private readonly Faker _faker;

        public ICollegeUserValidatorTests()
        {
            _validator = new ICollegeUserValidator(XunitInjectionCollection.LookupsCache);
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ICollegeUserValidator_ShouldPass()
        {
            var colleges = await _lookupsCache.GetColleges(Domestic.Apply.Constants.Localization.EnglishCanada);

            var message = new MyRequest
            {
                CollegeId = _faker.PickRandom(colleges).Id
            };

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ICollegeUserValidator_ShouldFail_When_CollegeId_NotValid()
        {
            var message = new MyRequest
            {
                CollegeId = Guid.NewGuid()
            };

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
            .And.ContainSingle(x => x.ErrorMessage == "'College Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ICollegeUserValidator_ShouldFail_When_CollegeId_Empty()
        {
            var message = new MyRequest
            {
                CollegeId = Guid.Empty
            };

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
            .And.ContainSingle(x => x.ErrorMessage == "'College Id' must not be empty.");
        }

        private class MyRequest : ICollegeUser
        {
            public IPrincipal User => Mock.Of<IPrincipal>();

            public Guid CollegeId { get; set; }
        }
    }
}
