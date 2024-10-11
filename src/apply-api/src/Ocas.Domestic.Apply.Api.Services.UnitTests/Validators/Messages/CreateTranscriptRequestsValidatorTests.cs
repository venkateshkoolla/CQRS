using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class CreateTranscriptRequestsValidatorTests
    {
        private readonly CreateTranscriptRequestsValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Mock<IPrincipal> _user = new Mock<IPrincipal>();

        public CreateTranscriptRequestsValidatorTests()
        {
            _validator = new CreateTranscriptRequestsValidator(XunitInjectionCollection.LookupsCache);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateTranscriptRequests_ShouldPass()
        {
            var applicationId = Guid.NewGuid();
            var transcriptRequests = _modelFakerFixture.GetTranscriptRequestBase().Generate(2);
            transcriptRequests.ForEach(c => c.ApplicationId = applicationId);

            var model = new Faker<CreateTranscriptRequests>()
                .RuleFor(o => o.User, _ => _user.Object)
                .RuleFor(o => o.TranscriptRequests, transcriptRequests)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateTranscriptRequests_ShouldThrow_When_Differed_ApplicationId()
        {
            var transcriptRequests = _modelFakerFixture.GetTranscriptRequestBase().Generate(3);

            var model = new Faker<CreateTranscriptRequests>()
                .RuleFor(o => o.User, _ => _user.Object)
                .RuleFor(o => o.TranscriptRequests, transcriptRequests)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Transcript Requests' must be for same application.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CreateTranscriptRequests_ShouldThrow_When_Transcript_Requests_Empty()
        {
            var model = new Faker<CreateTranscriptRequests>()
                .RuleFor(o => o.User, _ => _user.Object)
                .RuleFor(o => o.TranscriptRequests, new List<TranscriptRequestBase>())
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Transcript Requests' must not be empty.");
        }
    }
}
