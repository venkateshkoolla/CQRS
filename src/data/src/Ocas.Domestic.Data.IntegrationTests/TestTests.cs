using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TestTests : BaseTest
    {
        [Fact]
        public async Task GetTests_ShouldPass()
        {
            var entities = new List<Test>();
            Contact applicant = null;
            try
            {
                // Arrange
                const int numberOfTests = 3;
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                foreach (var model in DataFakerFixture.Models.TestBase.Generate(numberOfTests))
                {
                    model.ApplicantId = applicant.Id;
                    entities.Add(await Context.CreateTest(model, Locale.English));
                }

                // Act
                var testOptions = new GetTestOptions { ApplicantId = applicant.Id };
                var tests = await Context.GetTests(testOptions, Locale.English);

                // Assert
                tests.Should().HaveCount(numberOfTests);
                tests.Should().OnlyContain(x => x.ApplicantId == applicant.Id);
            }
            finally
            {
                // Cleanup
                if (entities.Any())
                {
                    foreach (var entity in entities)
                    {
                        if (entity?.Id != null)
                            await Context.DeleteTest(entity.Id);
                    }
                }

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        [Fact]
        public async Task GetTests_ShouldPass_With_Official()
        {
            var entities = new List<Test>();
            Contact applicant = null;
            try
            {
                // Arrange
                const int numberOfTests = 3;
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                foreach (var model in DataFakerFixture.Models.TestBase.Generate(numberOfTests))
                {
                    model.ApplicantId = applicant.Id;
                    model.IsOfficial = true;
                    entities.Add(await Context.CreateTest(model, Locale.English));
                }

                // Act
                var testOptions = new GetTestOptions { ApplicantId = applicant.Id, IsOfficial = true };
                var tests = await Context.GetTests(testOptions, Locale.English);

                // Assert
                tests.Should().HaveCount(numberOfTests);
                tests.Should().OnlyContain(x => x.ApplicantId == applicant.Id);
            }
            finally
            {
                // Cleanup
                if (entities.Any())
                {
                    foreach (var entity in entities)
                    {
                        if (entity?.Id != null)
                            await Context.DeleteTest(entity.Id);
                    }
                }

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        [Fact]
        public async Task GetTests_ShouldPass_With_Official_And_UnOfficial()
        {
            var entities = new List<Test>();
            Contact applicant = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));

                var test1 = DataFakerFixture.Models.TestBase.Generate();
                test1.ApplicantId = applicant.Id;
                test1.IsOfficial = true;
                entities.Add(await Context.CreateTest(test1, Locale.English));

                var test2 = DataFakerFixture.Models.TestBase.Generate();
                test2.ApplicantId = applicant.Id;
                test2.IsOfficial = false;
                entities.Add(await Context.CreateTest(test2, Locale.English));

                // Act
                var officialTests = await Context.GetTests(new GetTestOptions { ApplicantId = applicant.Id, IsOfficial = true }, Locale.English);
                var unOfficialTests = await Context.GetTests(new GetTestOptions { ApplicantId = applicant.Id, IsOfficial = false }, Locale.English);

                // Assert
                officialTests.Should().HaveCount(1);
                officialTests.Should().OnlyContain(x => x.ApplicantId == applicant.Id);
                unOfficialTests.Should().HaveCount(1);
                unOfficialTests.Should().OnlyContain(x => x.ApplicantId == applicant.Id);
            }
            finally
            {
                // Cleanup
                if (entities.Any())
                {
                    foreach (var entity in entities)
                    {
                        if (entity?.Id != null)
                            await Context.DeleteTest(entity.Id);
                    }
                }

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        [Fact]
        public async Task GetTest_ShouldPass()
        {
            Test entity = null;
            Contact applicant = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var model = DataFakerFixture.Models.TestBase.Generate();
                model.ApplicantId = applicant.Id;
                entity = await Context.CreateTest(model, Locale.English);

                // Act
                var test = await Context.GetTest(entity.Id, Locale.English);

                // Assert
                test.Should().NotBeNull();
            }
            finally
            {
                if (entity?.Id != null)
                    await Context.DeleteTest(entity);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        [Fact]
        public async Task GetTest_ShouldPass_WithDetails()
        {
            // Act
            var test = await Context.GetTest(TestConstants.Tests.TestWithDetails, Locale.English);

            // Assert
            test.Should().NotBeNull();
            test.Details.Should().NotBeNull();
            test.Details.Should().NotBeEmpty();
            test.Details.Should().HaveCount(8);
            test.Details.Should().OnlyContain(x => x.TestId == TestConstants.Tests.TestWithDetails);
            test.Details.Should().OnlyHaveUniqueItems(x => x.Id);
            test.ApplicationCycleName.Should().NotBeNull();
            test.NormingGroupName.Should().NotBeNull();
        }

        [Fact]
        public async Task GetTests_ShouldPass_WithDetails()
        {
            // Act
            var testOptions = new GetTestOptions { ApplicantId = TestConstants.Tests.ApplicantWithTestDetails };

            var tests = await Context.GetTests(testOptions, Locale.English);

            // Assert
            tests.Should().NotBeNull();
            tests.Should().NotBeEmpty();

            var test = tests.Single(x => x.Id == TestConstants.Tests.TestWithDetails);
            test.Should().NotBeNull();
            test.Details.Should().NotBeNull();
            test.Details.Should().NotBeEmpty();
            test.Details.Should().HaveCount(8);
            test.Details.Should().OnlyContain(x => x.TestId == TestConstants.Tests.TestWithDetails);
            test.Details.Should().OnlyHaveUniqueItems(x => x.Id);
            test.ApplicationCycleName.Should().NotBeNull();
            test.NormingGroupName.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateTest_ShouldPass()
        {
            Test entity = null;
            Contact applicant = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var model = DataFakerFixture.Models.TestBase.Generate();
                model.ApplicantId = applicant.Id;

                // Act
                entity = await Context.CreateTest(model, Locale.English);

                // Assert
                CheckTestBaseFields(entity, model);
            }
            finally
            {
                // Cleanup
                if (entity?.Id != null)
                    await Context.DeleteTest(entity.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        [Fact]
        public async Task UpdateTest_ShouldPass()
        {
            Test entityBefore = null;
            Contact applicant = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));

                var entity = DataFakerFixture.Models.TestBase.Generate();
                entity.ApplicantId = applicant.Id;
                entityBefore = await Context.CreateTest(entity, Locale.English);

                var model = DataFakerFixture.Models.Test.Generate();
                model.Id = entityBefore.Id;
                model.ApplicantId = entityBefore.ApplicantId;
                model.Description = entityBefore.Description;

                // Act
                var entityAfter = await Context.UpdateTest(model, Locale.English);

                // Assert
                CheckTestFields(entityAfter, model);
            }
            finally
            {
                // Cleanup
                if (entityBefore?.Id != null)
                    await Context.DeleteTest(entityBefore.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        [Fact]
        public async Task DeleteTest_ShouldPass_WhenId()
        {
            Contact applicant = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var model = DataFakerFixture.Models.TestBase.Generate();
                model.ApplicantId = applicant.Id;
                var entity = await Context.CreateTest(model, Locale.English);

                // Act
                await Context.DeleteTest(entity.Id);
                var testAfter = await Context.GetTest(entity.Id, Locale.English);

                // Assert
                testAfter.Should().BeNull();
            }
            finally
            {
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        [Fact]
        public async Task DeleteTest_ShouldPass_WhenObject()
        {
            Contact applicant = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var model = DataFakerFixture.Models.TestBase.Generate();
                model.ApplicantId = applicant.Id;

                // Act
                var entity = await Context.CreateTest(model, Locale.English);
                await Context.DeleteTest(entity);
                var testAfter = await Context.GetTest(entity.Id, Locale.English);

                // Assert
                testAfter.Should().BeNull();
            }
            finally
            {
                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);
            }
        }

        private void CheckTestFields(Test entity, Test model)
        {
            CheckTestBaseFields(entity, model);
            entity.Id.Should().Be(model.Id);
        }

        private void CheckTestBaseFields(Test entity, TestBase model)
        {
            entity.Id.Should().NotBeEmpty();
            entity.TestTypeId.Should().Be(model.TestTypeId);
            entity.CountryId.Should().Be(model.CountryId);
            entity.ProvinceStateName.Should().Be(model.ProvinceStateName);
            entity.ProvinceStateId.Should().Be(model.ProvinceStateId);
            entity.CityName.Should().Be(model.CityName);
            entity.CityId.Should().Be(model.CityId);
            entity.DateTestTaken.Should().Be(model.DateTestTaken);
            entity.Description.Should().Be(model.Description);
            entity.IsOfficial.Should().Be(model.IsOfficial);
        }
    }
}
