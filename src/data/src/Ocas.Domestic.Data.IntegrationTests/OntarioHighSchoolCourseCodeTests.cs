using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OntarioHighSchoolCourseCodeTests : BaseTest
    {
        [Fact]
        public async Task GetCourseCodes_ShouldPass()
        {
            var result = await Context.GetOntarioHighSchoolCourseCodes();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetCourseCodes_ShouldPass_When_CourseCode()
        {
            const string courseCode = "ZPR4T";
            var result = await Context.GetOntarioHighSchoolCourseCode(courseCode);
            result.Name.Should().Be(courseCode);
        }

        [Fact]
        public async Task GetCourseCodes_ShouldPass_When_CourseCode_Not_Exists()
        {
            const string courseCode = "ZP123";

            var result = await Context.GetOntarioHighSchoolCourseCode(courseCode);
            result.Should().BeNull();
        }
    }
}
