using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class CreateEducationHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly IDtoMapper _dtoMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;

        public CreateEducationHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateEducation_ShouldThrow_WhenInvalidDate()
        {
            // Arrange
            var educationBase = _models.GetEducationBase().Generate();
            var request = new CreateEducation
            {
                User = new ClaimsPrincipal(),
                ApplicantId = Guid.NewGuid(),
                Education = educationBase
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).Returns(Task.FromResult(new Dto.Contact
            {
                BirthDate = educationBase.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddDays(1)
            }));
            var handler = new CreateEducationHandler(Mock.Of<ILogger<CreateEducationHandler>>(), domesticContextMock.Object, _apiMapper, _dtoMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be($"Education.AttendedFrom must be after applicant's birth: {request.Education.AttendedFrom}");
        }
    }
}
