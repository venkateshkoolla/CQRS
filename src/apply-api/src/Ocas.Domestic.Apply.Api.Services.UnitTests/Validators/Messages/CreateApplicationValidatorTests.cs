using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class CreateApplicationValidatorTests
    {
        private readonly CreateApplicationValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Mock<IPrincipal> _user = new Mock<IPrincipal>();

        public CreateApplicationValidatorTests()
        {
            _validator = new CreateApplicationValidator(XunitInjectionCollection.LookupsCache);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateApplicationValidator_ShouldPass()
        {
            var applicationActiveCycles = _modelFakerFixture.AllApplyLookups.ApplicationCycles.Where(x => x.Status == Constants.ApplicationCycleStatuses.Active);

            var model = new Faker<CreateApplication>()
                .RuleFor(o => o.User, _ => _user.Object)
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(applicationActiveCycles).Id)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateApplicationValidator_ShouldFail_When_ApplicantIdEmpty()
        {
            var applicationActiveCycles = _modelFakerFixture.AllApplyLookups.ApplicationCycles.Where(x => x.Status == Constants.ApplicationCycleStatuses.Active);
            var model = new Faker<CreateApplication>()
                .RuleFor(o => o.User, _ => _user.Object)
                .RuleFor(o => o.ApplicantId, _ => Guid.Empty)
                .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(applicationActiveCycles).Id)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Applicant Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateApplicationValidator_ShouldFail_When_ApplicationCycleIdEmpty()
        {
            var model = new Faker<CreateApplication>()
                .RuleFor(o => o.User, _ => _user.Object)
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicationCycleId, _ => Guid.Empty)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Application Cycle Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateApplicationValidator_ShouldFail_When_ApplicationCycleIdDoesNotExist()
        {
            var model = new Faker<CreateApplication>()
                .RuleFor(o => o.User, _ => _user.Object)
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicationCycleId, _ => Guid.NewGuid())
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Application Cycle Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateApplicationValidator_ShouldFail_When_ApplicationCycleIdNotActive()
        {
            var applicationPreviousCycles = _modelFakerFixture.AllApplyLookups.ApplicationCycles.Where(x => x.Status == Constants.ApplicationCycleStatuses.Previous);
            var model = new Faker<CreateApplication>()
                .RuleFor(o => o.User, _ => _user.Object)
                .RuleFor(o => o.ApplicantId, _ => Guid.Empty)
                .RuleFor(o => o.ApplicationCycleId, f => f.PickRandom(applicationPreviousCycles).Id)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Application Cycle Id' must be active to create an application.");
        }
    }
}
