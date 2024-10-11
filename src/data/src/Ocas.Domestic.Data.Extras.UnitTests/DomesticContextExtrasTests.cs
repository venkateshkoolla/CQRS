using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Data.Utils;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Extras.UnitTests
{
    public class DomesticContextExtrasTests
    {
        private readonly DataFakerFixture _dataFakerFixture;

        public DomesticContextExtrasTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
        }

        [Theory]
        [UnitTest]
        [InlineData(BoaTestScenario.GraduatingBeforeProgramStart, Constants.BasisForAdmission.WillHaveOssd, Constants.Current.Yes)]
        [InlineData(BoaTestScenario.GraduatingAfterProgramStart, Constants.BasisForAdmission.WillNotHaveOssd, Constants.Current.Yes)]
        [InlineData(BoaTestScenario.GraduatingBeforeCycleStart, Constants.BasisForAdmission.WillHaveOssd, Constants.Current.Yes)]
        [InlineData(BoaTestScenario.GraduatingAfterCycleStart, Constants.BasisForAdmission.WillNotHaveOssd, Constants.Current.Yes)]
        [InlineData(BoaTestScenario.HasGed, Constants.BasisForAdmission.WillHaveOssd, Constants.Current.No)]
        [InlineData(BoaTestScenario.NoGed, Constants.BasisForAdmission.WillNotHaveOssd, Constants.Current.No)]
        public void PatchBasisForAdmission_ShouldPass(BoaTestScenario scenario, string bfaCode, string currentCode)
        {
            // Arrange
            var basisForAdmissionId = _dataFakerFixture.SeedData.BasisForAdmissions.Single(x => x.Code == bfaCode).Id;
            var currentId = _dataFakerFixture.SeedData.Currents.Single(x => x.Code == currentCode).Id;
            var modifiedBy = _dataFakerFixture.Faker.Name.FirstName();
            BoaTestSetup(scenario, out var domesticContextMock, out var application, out var applicant);
            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            // Act
            Func<Task> func = () => domesticContextExtras.PatchBasisForAdmission(
                application,
                applicant,
                modifiedBy,
                _dataFakerFixture.SeedData.BasisForAdmissions,
                _dataFakerFixture.SeedData.Currents,
                _dataFakerFixture.SeedData.ApplicationCycles);

            // Assert
            func.Should().NotThrow();
            application.BasisForAdmissionId.Should().Be(basisForAdmissionId);
            application.CurrentId.Should().Be(currentId);
            application.ModifiedBy.Should().Be(modifiedBy);
        }

        [Theory]
        [UnitTest]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CreateOrder_ShouldPass_When_NoChoices_Then_Not_CreateAnOrder(bool isOfflinePayment)
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            const string modifiedBy = "test@test.ocas.ca";
            var sourceId = Guid.NewGuid();

            var domesticContextMock = new Mock<IDomesticContext>();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice>());
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = applicationId });
            domesticContextMock.Setup(m => m.GetShoppingCart(It.IsAny<Dto.GetShoppingCartOptions>(), It.IsAny<Locale>()))
                .ReturnsAsync(new Dto.ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    Details = new List<Dto.ShoppingCartDetail>
                    {
                        new Dto.ShoppingCartDetail { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Type = ShoppingCartItemType.ApplicationFee }
                    }
                });

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            // Act
            var result = await domesticContextExtras.CreateOrder(applicationId, applicantId, modifiedBy, sourceId, isOfflinePayment);

            // Assert
            result.Should().BeNull();

            domesticContextMock.Verify(m => m.CreateOrder(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Dto.ShoppingCart>()), Times.Never);
            domesticContextMock.Verify(m => m.CreateOrderDetail(It.IsAny<Dto.Order>(), It.IsAny<Dto.ShoppingCartDetail>()), Times.Never);
            domesticContextMock.Verify(m => m.UpdateOrder(It.IsAny<Dto.Order>()), Times.Never);
        }

        [Theory]
        [UnitTest]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CreateOrder_ShouldPass_When_VoucherInOrder_Then_CreateAnOrder(bool isOfflinePayment)
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            const string modifiedBy = "test@test.ocas.ca";
            var sourceId = Guid.NewGuid();

            var domesticContextMock = new Mock<IDomesticContext>();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { Id = Guid.NewGuid() } });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = applicationId });
            domesticContextMock.Setup(m => m.GetShoppingCart(It.IsAny<Dto.GetShoppingCartOptions>(), It.IsAny<Locale>()))
              .ReturnsAsync(new Dto.ShoppingCart
              {
                  Id = Guid.NewGuid(),
                  Details = new List<Dto.ShoppingCartDetail>
                  {
                                new Dto.ShoppingCartDetail { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Type = ShoppingCartItemType.ApplicationFee },
                                new Dto.ShoppingCartDetail { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Type = ShoppingCartItemType.Voucher }
                  }
              });
            domesticContextMock.Setup(m => m.CreateOrder(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Dto.ShoppingCart>())).ReturnsAsync(new Dto.Order());
            domesticContextMock.SetupSequence(m => m.CreateOrderDetail(It.IsAny<Dto.Order>(), It.IsAny<Dto.ShoppingCartDetail>()))
                .ReturnsAsync(new Dto.OrderDetail { Id = Guid.NewGuid(), Type = ShoppingCartItemType.ApplicationFee })
                .ReturnsAsync(new Dto.OrderDetail { Id = Guid.NewGuid(), Type = ShoppingCartItemType.Voucher, VoucherId = Guid.NewGuid(), PricePerUnit = 25 });
            domesticContextMock.Setup(m => m.GetVoucher(It.IsAny<Dto.GetVoucherOptions>())).ReturnsAsync(new Dto.Voucher { Id = Guid.NewGuid() });
            domesticContextMock.Setup(m => m.UpdateOrder(It.IsAny<Dto.Order>())).ReturnsAsync(new Dto.Order());

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            // Act
            var result = await domesticContextExtras.CreateOrder(applicationId, applicantId, modifiedBy, sourceId, isOfflinePayment);

            // Assert
            result.Should().NotBeNull();

            domesticContextMock.Verify(m => m.CreateOrder(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Dto.ShoppingCart>()), Times.Once);
            domesticContextMock.Verify(m => m.CreateOrderDetail(It.IsAny<Dto.Order>(), It.IsAny<Dto.ShoppingCartDetail>()), Times.AtLeastOnce);
            domesticContextMock.Verify(m => m.UpdateOrder(It.IsAny<Dto.Order>()), Times.AtLeastOnce);
            domesticContextMock.Verify(m => m.LinkOrderToVoucher(It.IsAny<Dto.Voucher>()), Times.Once);
        }

        [Fact]
        [UnitTest]
        public async Task CreateOrder_ShouldPass_Offline_When_NoShoppingCart_Then_CreateAnOrder()
        {
            // Arrange
            const bool isOfflinePayment = true;
            var applicationId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            const string modifiedBy = "test@test.ocas.ca";
            var sourceId = Guid.NewGuid();

            var domesticContextMock = new Mock<IDomesticContext>();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { Id = Guid.NewGuid() } });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = applicationId });
            domesticContextMock.Setup(m => m.GetShoppingCart(It.IsAny<Dto.GetShoppingCartOptions>(), It.IsAny<Locale>())).ReturnsAsync((Dto.ShoppingCart)null);
            domesticContextMock.Setup(m => m.CreateOrder(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Dto.ShoppingCart>())).ReturnsAsync(new Dto.Order());
            domesticContextMock.Setup(m => m.UpdateOrder(It.IsAny<Dto.Order>())).ReturnsAsync(new Dto.Order());

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            // Act
            var result = await domesticContextExtras.CreateOrder(applicationId, applicantId, modifiedBy, sourceId, isOfflinePayment);

            // Assert
            result.Should().NotBeNull();

            domesticContextMock.Verify(m => m.CreateOrder(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Dto.ShoppingCart>()), Times.Once);
            domesticContextMock.Verify(m => m.CreateOrderDetail(It.IsAny<Dto.Order>(), It.IsAny<Dto.ShoppingCartDetail>()), Times.Never);
            domesticContextMock.Verify(m => m.UpdateOrder(It.IsAny<Dto.Order>()), Times.AtLeastOnce);
        }

        [Fact]
        [UnitTest]
        public async Task CreateOrder_ShouldPass_Online_When_NoShoppingCart_Then_Not_CreateAnOrder()
        {
            // Arrange
            const bool isOfflinePayment = false;
            var applicationId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            const string modifiedBy = "test@test.ocas.ca";
            var sourceId = Guid.NewGuid();

            var domesticContextMock = new Mock<IDomesticContext>();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { Id = Guid.NewGuid() } });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = applicationId });
            domesticContextMock.Setup(m => m.GetShoppingCart(It.IsAny<Dto.GetShoppingCartOptions>(), It.IsAny<Locale>())).ReturnsAsync((Dto.ShoppingCart)null);

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            // Act
            var result = await domesticContextExtras.CreateOrder(applicationId, applicantId, modifiedBy, sourceId, isOfflinePayment);

            // Assert
            result.Should().BeNull();

            domesticContextMock.Verify(m => m.CreateOrder(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Dto.ShoppingCart>()), Times.Never);
            domesticContextMock.Verify(m => m.CreateOrderDetail(It.IsAny<Dto.Order>(), It.IsAny<Dto.ShoppingCartDetail>()), Times.Never);
            domesticContextMock.Verify(m => m.UpdateOrder(It.IsAny<Dto.Order>()), Times.Never);
        }

        [Fact]
        [UnitTest]
        public async Task CreateOrder_ShouldPass_Online_When_ShoppingCart_Then_CreateAnOrder()
        {
            // Arrange
            const bool isOfflinePayment = false;
            var applicationId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            const string modifiedBy = "test@test.ocas.ca";
            var sourceId = Guid.NewGuid();

            var domesticContextMock = new Mock<IDomesticContext>();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { Id = Guid.NewGuid() } });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = applicationId });
            domesticContextMock.Setup(m => m.GetShoppingCart(It.IsAny<Dto.GetShoppingCartOptions>(), It.IsAny<Locale>()))
              .ReturnsAsync(new Dto.ShoppingCart
              {
                  Id = Guid.NewGuid(),
                  Details = new List<Dto.ShoppingCartDetail>
                  {
                                new Dto.ShoppingCartDetail { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Type = ShoppingCartItemType.ApplicationFee }
                  }
              });
            domesticContextMock.Setup(m => m.CreateOrder(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Dto.ShoppingCart>())).ReturnsAsync(new Dto.Order());
            domesticContextMock.Setup(m => m.CreateOrderDetail(It.IsAny<Dto.Order>(), It.IsAny<Dto.ShoppingCartDetail>())).ReturnsAsync(new Dto.OrderDetail { Id = Guid.NewGuid() });
            domesticContextMock.Setup(m => m.UpdateOrder(It.IsAny<Dto.Order>())).ReturnsAsync(new Dto.Order());

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            // Act
            var result = await domesticContextExtras.CreateOrder(applicationId, applicantId, modifiedBy, sourceId, isOfflinePayment);

            // Assert
            result.Should().NotBeNull();

            domesticContextMock.Verify(m => m.CreateOrder(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Dto.ShoppingCart>()), Times.Once);
            domesticContextMock.Verify(m => m.CreateOrderDetail(It.IsAny<Dto.Order>(), It.IsAny<Dto.ShoppingCartDetail>()), Times.AtLeastOnce);
            domesticContextMock.Verify(m => m.UpdateOrder(It.IsAny<Dto.Order>()), Times.AtLeastOnce);
        }

        [Theory]
        [UnitTest]
        [InlineData(true, null, null)]
        [InlineData(null, true, null)]
        [InlineData(null, null, true)]
        public async Task PatchEducationStatus_ShouldPass_When_AlreadyAnswered(bool? highSchoolEnrolled, bool? highSchoolGraduated, bool? hasHighSchoolGraduationDate)
        {
            // Arrange
            var applicant = new Dto.Contact
            {
                Id = Guid.NewGuid(),
                HighSchoolEnrolled = highSchoolEnrolled,
                HighSchoolGraduated = highSchoolGraduated,
                HighSchoolGraduationDate = hasHighSchoolGraduationDate == true ? _dataFakerFixture.Faker.Date.Recent() : (DateTime?)null
            };
            const string modifiedBy = "test@test.ocas.ca";

            var domesticContextExtras = new DomesticContextExtras(Mock.Of<IDomesticContext>());

            // Act
            var result = await domesticContextExtras.PatchEducationStatus(
                applicant,
                modifiedBy,
                _dataFakerFixture.SeedData.BasisForAdmissions,
                _dataFakerFixture.SeedData.Currents,
                _dataFakerFixture.SeedData.ApplicationCycles);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [UnitTest]
        public async Task PatchEducationStatus_ShouldPass_When_Application_Without_Boa()
        {
            // Arrange
            var applicant = new Dto.Contact
            {
                Id = Guid.NewGuid()
            };
            const string modifiedBy = "test@test.ocas.ca";
            var application = new Dto.Application
            {
                Id = Guid.NewGuid()
            };

            var domesticContextMock = new Mock<IDomesticContext>();
            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Application> { application });

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            // Act
            var result = await domesticContextExtras.PatchEducationStatus(
                applicant,
                modifiedBy,
                _dataFakerFixture.SeedData.BasisForAdmissions,
                _dataFakerFixture.SeedData.Currents,
                _dataFakerFixture.SeedData.ApplicationCycles);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [UnitTest]
        public async Task PatchEducationStatus_ShouldPass_When_MulitpleApplications_CurrentYesBasisGraduated()
        {
            // Arrange
            const string modifiedBy = "test@test.ocas.ca";
            EduStatusSetup(EduStatusTestScenario.CurrentYesBasisGraduated, out var domesticContextMock, out var application, out var applicant);

            var activeAppCycleStatus = _dataFakerFixture.SeedData.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active);
            var activeAppCycles = _dataFakerFixture.SeedData.ApplicationCycles.Where(a => a.StatusId == activeAppCycleStatus.Id);
            activeAppCycles.Should().HaveCountGreaterThan(1, "because must have mulitple cycles to create applications");

            var newestAppCycle = activeAppCycles.OrderByDescending(c => c.StartDate).First();
            application.ApplicationCycleId = newestAppCycle.Id;

            var earliestAppCycle = activeAppCycles.OrderBy(c => c.StartDate).First();
            var earliestApplication = new Dto.Application
            {
                Id = Guid.NewGuid(),
                ApplicantId = applicant.Id,
                ApplicationCycleId = earliestAppCycle.Id,
                CurrentId = _dataFakerFixture.SeedData.Currents.First(c => c.Code == Constants.Current.No).Id,
                BasisForAdmissionId = _dataFakerFixture.SeedData.BasisForAdmissions.First(b => b.Code == Constants.BasisForAdmission.WillHaveOssd).Id
            };

            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Application> { earliestApplication, application });

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            var result = await domesticContextExtras.PatchEducationStatus(
                applicant,
                modifiedBy,
                _dataFakerFixture.SeedData.BasisForAdmissions,
                _dataFakerFixture.SeedData.Currents,
                _dataFakerFixture.SeedData.ApplicationCycles);

            // Assert
            result.Should().BeTrue();
            applicant.HighSchoolEnrolled.Should().BeTrue();
            applicant.HighSchoolGraduated.Should().BeNull();
            applicant.HighSchoolGraduationDate.Should().Be(earliestAppCycle.StartDate);
            applicant.ModifiedBy.Should().Be(modifiedBy);
        }

        [Theory]
        [UnitTest]
        [InlineData(EduStatusTestScenario.CurrentYesBasisNull, true, null, null)]
        [InlineData(EduStatusTestScenario.CurrentYesBasisNotGraduated, true, null, null)]
        [InlineData(EduStatusTestScenario.CurrentYesBasisGraduated, true, null, true)]
        [InlineData(EduStatusTestScenario.CurrentNoBasisNull, false, null, null)]
        [InlineData(EduStatusTestScenario.CurrentNoBasisNotGraduated, false, false, null)]
        [InlineData(EduStatusTestScenario.CurrentNoBasisGraduated, false, true, null)]
        [InlineData(EduStatusTestScenario.CurrentNullBasisNotGraduated, false, false, null)]
        public async Task PatchEducationStatus_ShouldPass_When_Application(EduStatusTestScenario scenario, bool? highSchoolEnrolled, bool? highSchoolGraduated, bool? hasHighSchoolGraduationDate)
        {
            // Arrange
            const string modifiedBy = "test@test.ocas.ca";
            EduStatusSetup(scenario, out var domesticContextMock, out var application, out var applicant);

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            // Act
            var result = await domesticContextExtras.PatchEducationStatus(
                applicant,
                modifiedBy,
                _dataFakerFixture.SeedData.BasisForAdmissions,
                _dataFakerFixture.SeedData.Currents,
                _dataFakerFixture.SeedData.ApplicationCycles);

            // Assert
            result.Should().BeTrue();
            applicant.HighSchoolEnrolled.Should().Be(highSchoolEnrolled);
            applicant.HighSchoolGraduated.Should().Be(highSchoolGraduated);
            applicant.ModifiedBy.Should().Be(modifiedBy);

            if (hasHighSchoolGraduationDate == true)
            {
                var highSchoolGraduationDate = _dataFakerFixture.SeedData.ApplicationCycles.First(c => c.Id == application.ApplicationCycleId).StartDate;
                applicant.HighSchoolGraduationDate.Should().NotBeNull().And.Be(highSchoolGraduationDate);
            }
            else
            {
                applicant.HighSchoolGraduationDate.Should().BeNull();
            }
        }

        [Fact]
        [UnitTest]
        public void PatchEducationStatus_ShouldThrow_When_Applicant_Null()
        {
            // Arrange
            Dto.Contact applicant = null;
            const string modifiedBy = "test@test.ocas.ca";

            var domesticContextExtras = new DomesticContextExtras(Mock.Of<IDomesticContext>());

            // Act
            Func<Task> func = () => domesticContextExtras.PatchEducationStatus(
                applicant,
                modifiedBy,
                _dataFakerFixture.SeedData.BasisForAdmissions,
                _dataFakerFixture.SeedData.Currents,
                _dataFakerFixture.SeedData.ApplicationCycles);

            // Assert
            func.Should().Throw<ArgumentNullException>();
        }

        private void BoaTestSetup(BoaTestScenario scenario, out Mock<IDomesticContext> domesticContextMock, out Dto.Application application, out Dto.Contact applicant)
        {
            var applicationCycle = _dataFakerFixture.SeedData.ApplicationCycles.First();
            var education = _dataFakerFixture.Models.EducationBase.Generate();
            applicant = new Dto.Contact
            {
                Id = Guid.NewGuid()
            };
            application = new Dto.Application
            {
                Id = Guid.NewGuid(),
                ApplicantId = applicant.Id,
                ApplicationCycleId = applicationCycle.Id
            };
            var intakeStartDate = DateTime.UtcNow.ToDateInEstAsUtc();
            var programChoices = new List<Dto.ProgramChoice>
            {
                new Dto.ProgramChoice
                {
                    IntakeStartDate = intakeStartDate.ToString("yyMM")
                }
            };

            domesticContextMock = new Mock<IDomesticContext>();
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).Returns(Task.FromResult((IList<Dto.ProgramChoice>)programChoices));

            switch (scenario)
            {
                case BoaTestScenario.GraduatingBeforeProgramStart:
                    applicant.HighSchoolGraduationDate = intakeStartDate.AddMonths(-1);
                    applicant.HighSchoolEnrolled = true;
                    applicant.HighSchoolGraduated = false;
                    break;
                case BoaTestScenario.GraduatingAfterProgramStart:
                    applicant.HighSchoolGraduationDate = intakeStartDate.AddMonths(1);
                    applicant.HighSchoolEnrolled = true;
                    applicant.HighSchoolGraduated = false;
                    break;
                case BoaTestScenario.GraduatingBeforeCycleStart:
                    programChoices.Clear();
                    applicant.HighSchoolGraduationDate = applicationCycle.StartDate.AddMonths(-1);
                    applicant.HighSchoolEnrolled = true;
                    applicant.HighSchoolGraduated = false;
                    break;
                case BoaTestScenario.GraduatingAfterCycleStart:
                    programChoices.Clear();
                    applicant.HighSchoolGraduationDate = applicationCycle.StartDate.AddMonths(1);
                    applicant.HighSchoolEnrolled = true;
                    applicant.HighSchoolGraduated = false;
                    break;
                case BoaTestScenario.HasGed:
                    applicant.HighSchoolEnrolled = false;
                    applicant.HighSchoolGraduated = true;
                    break;
                case BoaTestScenario.NoGed:
                    applicant.HighSchoolEnrolled = false;
                    applicant.HighSchoolGraduated = false;
                    break;
            }
        }

        private void EduStatusSetup(EduStatusTestScenario scenario, out Mock<IDomesticContext> domesticContextMock, out Dto.Application application, out Dto.Contact applicant)
        {
            var applicationCycle = _dataFakerFixture.Faker.PickRandom(_dataFakerFixture.SeedData.ApplicationCycles);
            applicant = new Dto.Contact
            {
                Id = Guid.NewGuid()
            };
            application = new Dto.Application
            {
                Id = Guid.NewGuid(),
                ApplicantId = applicant.Id,
                ApplicationCycleId = applicationCycle.Id
            };

            domesticContextMock = new Mock<IDomesticContext>();
            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Application> { application });

            switch (scenario)
            {
                case EduStatusTestScenario.CurrentYesBasisNull:
                    application.CurrentId = _dataFakerFixture.SeedData.Currents.First(c => c.Code == Constants.Current.Yes).Id;
                    application.BasisForAdmissionId = null;
                    break;
                case EduStatusTestScenario.CurrentYesBasisNotGraduated:
                    application.CurrentId = _dataFakerFixture.SeedData.Currents.First(c => c.Code == Constants.Current.Yes).Id;
                    application.BasisForAdmissionId = _dataFakerFixture.SeedData.BasisForAdmissions.First(b => b.Code == Constants.BasisForAdmission.WillNotHaveOssd).Id;
                    break;
                case EduStatusTestScenario.CurrentYesBasisGraduated:
                    application.CurrentId = _dataFakerFixture.SeedData.Currents.First(c => c.Code == Constants.Current.Yes).Id;
                    application.BasisForAdmissionId = _dataFakerFixture.SeedData.BasisForAdmissions.First(b => b.Code == Constants.BasisForAdmission.WillHaveOssd).Id;
                    break;
                case EduStatusTestScenario.CurrentNoBasisNull:
                    application.CurrentId = _dataFakerFixture.SeedData.Currents.First(c => c.Code == Constants.Current.No).Id;
                    application.BasisForAdmissionId = null;
                    break;
                case EduStatusTestScenario.CurrentNoBasisNotGraduated:
                    application.CurrentId = _dataFakerFixture.SeedData.Currents.First(c => c.Code == Constants.Current.No).Id;
                    application.BasisForAdmissionId = _dataFakerFixture.SeedData.BasisForAdmissions.First(b => b.Code == Constants.BasisForAdmission.WillNotHaveOssd).Id;
                    break;
                case EduStatusTestScenario.CurrentNoBasisGraduated:
                    application.CurrentId = _dataFakerFixture.SeedData.Currents.First(c => c.Code == Constants.Current.No).Id;
                    application.BasisForAdmissionId = _dataFakerFixture.SeedData.BasisForAdmissions.First(b => b.Code == Constants.BasisForAdmission.WillHaveOssd).Id;
                    break;
                case EduStatusTestScenario.CurrentNullBasisNotGraduated:
                    application.CurrentId = null;
                    application.BasisForAdmissionId = _dataFakerFixture.SeedData.BasisForAdmissions.First(b => b.Code == Constants.BasisForAdmission.WillNotHaveOssd).Id;
                    break;
                case EduStatusTestScenario.CurrentNullBasisGraduated:
                    application.CurrentId = null;
                    application.BasisForAdmissionId = _dataFakerFixture.SeedData.BasisForAdmissions.First(b => b.Code == Constants.BasisForAdmission.WillHaveOssd).Id;
                    break;
            }
        }

        public enum BoaTestScenario
        {
            GraduatingBeforeProgramStart = 0,
            GraduatingAfterProgramStart = 1,
            GraduatingBeforeCycleStart = 2,
            GraduatingAfterCycleStart = 3,
            HasGed = 4,
            NoGed = 5
        }

        public enum EduStatusTestScenario
        {
            CurrentYesBasisNull = 0,
            CurrentYesBasisNotGraduated = 1,
            CurrentYesBasisGraduated = 2,
            CurrentNoBasisNull = 3,
            CurrentNoBasisNotGraduated = 4,
            CurrentNoBasisGraduated = 5,
            CurrentNullBasisNotGraduated = 6,
            CurrentNullBasisGraduated = 7
        }
    }
}
