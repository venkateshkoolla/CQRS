using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class GetCollegeTemplateValidatorTests
    {
        private readonly IPrincipal _user = new Mock<IPrincipal>().Object;
        private readonly AllLookups _lookups;
        private readonly GetCollegeTemplateValidator _validator;

        public GetCollegeTemplateValidatorTests()
        {
            _lookups = XunitInjectionCollection.ModelFakerFixture.AllApplyLookups;
            _validator = new GetCollegeTemplateValidator(XunitInjectionCollection.LookupsCache);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCollegeTemplateValidator_ShouldPass()
        {
            var message = new Faker<GetCollegeTemplate>()
                .RuleFor(o => o.CollegeId, f => f.PickRandom(_lookups.Colleges).Id)
                .RuleFor(o => o.Key, f => f.PickRandom<CollegeTemplateKey>())
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCollegeTemplateValidator_ShouldFail_When_CollegeIdEmpty()
        {
            var message = new Faker<GetCollegeTemplate>()
                .RuleFor(o => o.CollegeId, _ => Guid.Empty)
                .RuleFor(o => o.Key, f => f.PickRandom<CollegeTemplateKey>())
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'College Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCollegeTemplateValidator_ShouldFail_When_CollegeIdNotFound()
        {
            var collegeId = Guid.NewGuid();
            var message = new Faker<GetCollegeTemplate>()
                .RuleFor(o => o.CollegeId, _ => collegeId)
                .RuleFor(o => o.Key, f => f.PickRandom<CollegeTemplateKey>())
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'College Id' does not exist: {collegeId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetCollegeTemplateValidator_ShouldFail_When_KeyNotInEnum()
        {
            var message = new Faker<GetCollegeTemplate>()
                .RuleFor(o => o.CollegeId, f => f.PickRandom(_lookups.Colleges).Id)
                .RuleFor(o => o.Key, _ => (CollegeTemplateKey)(-1))
                .RuleFor(o => o.User, _ => _user)
                .Generate();

            var result = await _validator.ValidateAsync(message);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Key' has a range of values which does not include '-1'.");
        }
    }
}
