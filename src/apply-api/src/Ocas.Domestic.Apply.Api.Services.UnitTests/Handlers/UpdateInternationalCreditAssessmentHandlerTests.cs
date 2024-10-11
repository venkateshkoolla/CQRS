using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class UpdateInternationalCreditAssessmentHandlerTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;

        public UpdateInternationalCreditAssessmentHandlerTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateInternationalCreditAssessmentHandler_ShouldPass_When_IntlCredentialAssessment_Created()
        {
            // Arrange
            var request = new UpdateInternationalCreditAssessment
            {
                ApplicantId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>(),
                IntlCredentialAssessment = _models.GetIntlCredentialAssessment().Generate()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                Id = Guid.NewGuid()
            });
            domesticContextMock.Setup(m => m.GetInternationalCreditAssessments(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.InternationalCreditAssessment>() as IList<Dto.InternationalCreditAssessment>);

            var handler = new UpdateInternationalCreditAssessmentHandler(Mock.Of<ILogger<UpdateInternationalCreditAssessmentHandler>>(), domesticContextMock.Object, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.CreateInternationalCreditAssessment(It.IsAny<Dto.InternationalCreditAssessmentBase>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateInternationalCreditAssessmentHandler_ShouldPass_When_IntlCredentialAssessment_Removed()
        {
            // Arrange
            var request = new UpdateInternationalCreditAssessment
            {
                ApplicantId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>(),
                IntlCredentialAssessment = null
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                Id = Guid.NewGuid()
            });
            var existingInltCred = _models.GetIntlCredentialAssessment().Generate();
            domesticContextMock.Setup(m => m.GetInternationalCreditAssessments(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.InternationalCreditAssessment>
            {
                new Dto.InternationalCreditAssessment
                {
                    CredentialEvaluationAgencyId = existingInltCred.IntlEvaluatorId,
                    ReferenceNumber = existingInltCred.IntlReferenceNumber
                }
            } as IList<Dto.InternationalCreditAssessment>);
            var handler = new UpdateInternationalCreditAssessmentHandler(Mock.Of<ILogger<UpdateInternationalCreditAssessmentHandler>>(), domesticContextMock.Object, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.UpdateInternationalCreditAssessment(It.Is<Dto.InternationalCreditAssessment>(o => o.ReferenceNumber == null)), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateInternationalCreditAssessmentHandler_ShouldPass_When_IntlCredentialAssessment_Removed_Already()
        {
            // Arrange
            var request = new UpdateInternationalCreditAssessment
            {
                ApplicantId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>(),
                IntlCredentialAssessment = null
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                Id = Guid.NewGuid()
            });
            var existingInltCred = _models.GetIntlCredentialAssessment().Generate();
            domesticContextMock.Setup(m => m.GetInternationalCreditAssessments(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.InternationalCreditAssessment>() as IList<Dto.InternationalCreditAssessment>);

            var handler = new UpdateInternationalCreditAssessmentHandler(Mock.Of<ILogger<UpdateInternationalCreditAssessmentHandler>>(), domesticContextMock.Object, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.UpdateInternationalCreditAssessment(It.Is<Dto.InternationalCreditAssessment>(o => o.ReferenceNumber == null)), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateInternationalCreditAssessmentHandler_ShouldPass_When_IntlCredentialAssessment_Updated()
        {
            // Arrange
            var request = new UpdateInternationalCreditAssessment
            {
                ApplicantId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>(),
                IntlCredentialAssessment = _models.GetIntlCredentialAssessment().Generate()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact
            {
                Id = Guid.NewGuid()
            });
            var existingInltCred = _models.GetIntlCredentialAssessment().Generate();
            domesticContextMock.Setup(m => m.GetInternationalCreditAssessments(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.InternationalCreditAssessment>
            {
                new Dto.InternationalCreditAssessment
                {
                    CredentialEvaluationAgencyId = existingInltCred.IntlEvaluatorId,
                    ReferenceNumber = existingInltCred.IntlReferenceNumber
                }
            } as IList<Dto.InternationalCreditAssessment>);

            var handler = new UpdateInternationalCreditAssessmentHandler(Mock.Of<ILogger<UpdateInternationalCreditAssessmentHandler>>(), domesticContextMock.Object, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(
                e => e.UpdateInternationalCreditAssessment(
                It.Is<Dto.InternationalCreditAssessment>(o => o.ReferenceNumber == request.IntlCredentialAssessment.IntlReferenceNumber)), Times.Once);
        }
    }
}
