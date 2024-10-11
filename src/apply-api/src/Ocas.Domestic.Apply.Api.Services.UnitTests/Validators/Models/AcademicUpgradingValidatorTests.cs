using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class AcademicUpgradingValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly EducationBaseValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly DataFakerFixture _dataFakerFixture;

        public AcademicUpgradingValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new EducationBaseValidator(XunitInjectionCollection.LookupsCache, new DomesticContextMock().Object);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicUpgradingValidator_ShouldPass_WhenAcademicUpgrading()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default, AcademicUpgrading");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicUpgradingValidator_ShouldPass_WhenAcademicUpgrading_Update()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default, AcademicUpgrading");
            model.InstituteId = Guid.NewGuid();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetCollege(It.IsAny<Guid>())).ReturnsAsync(new Dto.College { Id = model.InstituteId.Value });

            var validator = new AcademicUpgradingValidator(_lookupsCache, domesticContextMock.Object, Core.Enums.OperationType.Update);

            // Act
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
            domesticContextMock.Verify(e => e.GetCollege(It.Is<Guid>(p => p == model.InstituteId.Value)), Times.Once);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicUpgradingValidator_ShouldFail_WhenProvinceNotOntario_InAcademicUpgrading()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default, AcademicUpgrading");
            var provinceStates = await _lookupsCache.GetProvinceStates(TestConstants.Locale.English);
            var provinceState = _dataFakerFixture.Faker.PickRandom(provinceStates.Where(x => x.Code != Constants.Provinces.Ontario).ToList());
            model.ProvinceId = provinceState.Id;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Province Id' is not Ontario: {model.ProvinceId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicUpgradingValidator_ShouldFail_WhenDataMissing_InAcademicUpgrading()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default, AcademicUpgrading");
            model.ProvinceId = Guid.Empty;
            model.StudentNumber = string.Empty;
            model.InstituteId = Guid.Empty;
            model.InstituteTypeId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(5);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Province Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Student Number' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Institute Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Institute Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Institute Type Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicUpgradingValidator_ShouldFail_WhenNotCollege_InAcademicUpgrading()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default, AcademicUpgrading");
            var instituteTypes = await _lookupsCache.GetInstituteTypes(TestConstants.Locale.English);
            var instituteType = _dataFakerFixture.Faker.PickRandom(instituteTypes.Where(x => x.Code != Constants.InstituteTypes.College).ToList());
            model.InstituteTypeId = instituteType.Id;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Institute Type Id' is not college: {model.InstituteTypeId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicUpgradingValidator_ShouldFail_WhenNotCollege_InAcademicUpgrading_Update()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default, AcademicUpgrading");
            model.InstituteId = Guid.NewGuid();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetCollege(It.IsAny<Guid>())).ReturnsAsync((Dto.College)null);

            var validator = new AcademicUpgradingValidator(_lookupsCache, domesticContextMock.Object, Core.Enums.OperationType.Update);

            // Act
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == $"'Institute Id' is not an Ontario college: {model.InstituteId}");
            domesticContextMock.Verify(e => e.GetCollege(It.Is<Guid>(p => p == model.InstituteId.Value)), Times.Once);
        }
    }
}
