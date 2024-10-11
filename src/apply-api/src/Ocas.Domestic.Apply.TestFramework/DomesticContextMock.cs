using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ocas.Domestic.Data;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.TestFramework
{
    public class DomesticContextMock : Mock<IDomesticContext>
    {
        public static readonly Dto.OntarioHighSchoolCourseCode CourseCode = new Dto.OntarioHighSchoolCourseCode
        {
            Id = Guid.NewGuid(),
            Name = "0801A"
        };

        private readonly SeedDataFixture _seedDataFixture;

        public DomesticContextMock()
        {
            _seedDataFixture = new SeedDataFixture();
            SetupData();
        }

        private void SetupData()
        {
            Setup(m => m.GetAccountStatuses(It.IsAny<Locale>())).ReturnsAsync(_seedDataFixture.AccountStatuses);
            Setup(m => m.GetColleges()).ReturnsAsync(_seedDataFixture.Colleges);
            Setup(m => m.GetSources(It.IsAny<Locale>())).ReturnsAsync(_seedDataFixture.Sources);
            Setup(m => m.GetProgramSpecialCodes(It.IsAny<Guid>())).ReturnsAsync((Guid collegeApplicationId) => _seedDataFixture.ProgramSpecialCodes.Where(x => x.CollegeApplicationId == collegeApplicationId).ToList());
            Setup(m => m.GetOntarioHighSchoolCourseCode(It.IsAny<string>())).ReturnsAsync(CourseCode);
        }
    }
}