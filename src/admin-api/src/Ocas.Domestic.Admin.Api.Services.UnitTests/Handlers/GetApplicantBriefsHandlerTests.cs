using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using ValidationException = Ocas.Common.Exceptions.ValidationException;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetApplicantBriefsHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly IDomesticContext _domesticContext;
        private readonly ILogger<GetApplicantBriefsHandler> _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly TestFramework.ModelFakerFixture _models;
        private readonly IPrincipal _user;
        private readonly IUserAuthorization _userAuthorization;

        public GetApplicantBriefsHandlerTests()
        {
            _dataFakerFixture = new DataFakerFixture();
            _logger = Mock.Of<ILogger<GetApplicantBriefsHandler>>();
            _apiMapper = new AutoMapperFixture().CreateApiMapper();
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _models = XunitInjectionCollection.ModelFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _domesticContext = new DomesticContextMock().Object;
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicantBriefsHandler_ShouldPass()
        {
            // Arrange
            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions(),
                User = _user
            };

            const int expectedCount = 10;
            var dtoApplicantBriefs = new Faker<Dto.ApplicantBrief>()
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.AccountNumber, f => f.Random.ReplaceNumbers("###########"))
                .RuleFor(x => x.ApplicationNumber, f => f.Random.ReplaceNumbers("###########"))
                .RuleFor(x => x.ApplicationStatusId, f => f.PickRandom(_models.AllAdminLookups.ApplicationStatuses).Id)
                .RuleFor(x => x.BirthDate, f => f.Date.Past(16, DateTime.UtcNow))
                .RuleFor(x => x.CountryOfCitizenshipId, f => f.PickRandom(_models.AllAdminLookups.Countries).Id)
                .RuleFor(x => x.Email, f => f.Internet.Email(provider: "test.ocas.ca", uniqueSuffix: "_test"))
                .RuleFor(x => x.HomePhone, f => f.Person.Phone)
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .RuleFor(x => x.MiddleName, f => f.Person.FirstName)
                .RuleFor(x => x.MobilePhone, f => f.Person.Phone)
                .RuleFor(x => x.OntarioEducationNumber, _ => Constants.Education.DefaultOntarioEducationNumber)
                .RuleFor(x => x.PaymentLocked, f => f.Random.Bool())
                .RuleFor(x => x.PreferredName, f => f.Person.FullName)
                .RuleFor(x => x.PreviousLastName, f => f.Person.LastName)
                .Generate(expectedCount) as IList<Dto.ApplicantBrief>;

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetApplicantBriefs(It.IsAny<Dto.GetApplicantBriefOptions>(), It.IsAny<UserType>(), It.IsAny<string>()))
                    .ReturnsAsync(new Dto.PagedResult<Dto.ApplicantBrief> { Items = dtoApplicantBriefs, TotalCount = expectedCount });

            var handler = new GetApplicantBriefsHandler(_logger, _userAuthorization, _lookupsCache, domesticContext.Object, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<PagedResult<ApplicantBrief>>();
            result.Items.Should().NotBeNullOrEmpty().And.HaveCount(expectedCount);
            result.TotalCount.Should().Be(expectedCount);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantBriefsHandler_ShouldPass_When_HighSchoolUser_Mident()
        {
            // Arrange
            var mident = _dataFakerFixture.Faker.Random.String2(6, "0123456789");
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", _dataFakerFixture.Faker.Internet.Email(provider: "test.ocas.ca", uniqueSuffix: "_test")),
                        new Claim("partner_id", mident)
                    }));

            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions
                {
                    Mident = mident
                },
                User = user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolUser);

            var handler = new GetApplicantBriefsHandler(_logger, userAuthorization.Object, _lookupsCache, _domesticContext, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantBriefsHandler_ShouldPass_When_HighSchoolBoardUser_Mident()
        {
            // Arrange
            var boardMident = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.HighSchools).BoardMident;
            var mident = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.HighSchools.Where(h => h.BoardMident == boardMident)).Mident;

            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", _dataFakerFixture.Faker.Internet.Email(provider: "test.ocas.ca", uniqueSuffix: "_test")),
                        new Claim("partner_id", boardMident)
                    }));

            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions
                {
                    Mident = mident
                },
                User = user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolBoardUser);

            var handler = new GetApplicantBriefsHandler(_logger, userAuthorization.Object, _lookupsCache, _domesticContext, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantBriefsHandler_ShouldPass_When_OcasUser_Mident()
        {
            // Arrange
            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions
                {
                    Mident = _dataFakerFixture.Faker.Random.String2(6, "0123456789")
                },
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var handler = new GetApplicantBriefsHandler(_logger, userAuthorization.Object, _lookupsCache, _domesticContext, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantBriefsHandler_ShouldPass_When_CollegeUser_Mident()
        {
            // Arrange
            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions
                {
                    Mident = _dataFakerFixture.Faker.Random.String2(6, "0123456789")
                },
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.CollegeUser);

            var handler = new GetApplicantBriefsHandler(_logger, userAuthorization.Object, _lookupsCache, _domesticContext, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantBriefsHandler_ShouldThrow_When_HighSchoolUser_Different_Mident()
        {
            // Arrange
            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions
                {
                    Mident = _dataFakerFixture.Faker.Random.String2(6, "0123456789")
                },
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolUser);

            var handler = new GetApplicantBriefsHandler(_logger, userAuthorization.Object, _lookupsCache, _domesticContext, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>().WithMessage("User does not have access to mident.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantBriefsHandler_ShouldThrow_When_HighSchoolBoardUser_Different_Mident()
        {
            // Arrange
            var boardMident = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.HighSchools).BoardMident;
            var mident = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.HighSchools.Where(h => h.BoardMident != boardMident)).Mident;

            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", _dataFakerFixture.Faker.Internet.Email(provider: "test.ocas.ca", uniqueSuffix: "_test")),
                        new Claim("partner_id", boardMident)
                    }));

            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions
                {
                    Mident = mident
                },
                User = user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolBoardUser);

            var handler = new GetApplicantBriefsHandler(_logger, userAuthorization.Object, _lookupsCache, _domesticContext, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>().WithMessage("User does not have access to mident.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantBriefsHandler_ShouldPass_When_OcasUserTier2_PaymentLocked()
        {
            // Arrange
            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions
                {
                    PaymentLocked = _dataFakerFixture.Faker.Random.Bool()
                },
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new GetApplicantBriefsHandler(_logger, userAuthorization.Object, _lookupsCache, _domesticContext, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow<ValidationException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantBriefsHandler_ShouldThrow_When_NotOcasUserTier2_PaymentLocked()
        {
            // Arrange
            var request = new GetApplicantBriefs
            {
                Params = new GetApplicantBriefOptions
                {
                    PaymentLocked = _dataFakerFixture.Faker.Random.Bool()
                },
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new GetApplicantBriefsHandler(_logger, userAuthorization.Object, _lookupsCache, _domesticContext, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("User cannot filter by payment locked.");
        }
    }
}
