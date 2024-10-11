using System;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Settings;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.AppSettings.Extras.UnitTests
{
    public class AppSettingsExtrasTests
    {
        private const string LockStartTime = "22:00:00";
        private const string LockEndTime = "23:00:00";
        private const string AppCycleEqualConsiderDay = "01";
        private const string AppCycleEqualConsiderMonth = "02";
        private const string AppCycleEqualConsiderTime = "23:59:59";

        private readonly Faker _faker;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly TimeSpan _lockStartTime;
        private readonly TimeSpan _lockEndTime;
        private readonly int _appCycleEqualConsiderDay;
        private readonly int _appCycleEqualConsiderMonth;
        private readonly TimeSpan _appCycleEqualConsiderTime;

        public AppSettingsExtrasTests()
        {
            _faker = new Faker();

            var appSettings = Mock.Of<IAppSettingsBase>();
            Mock.Get(appSettings).Setup(a => a.GetAppSetting(It.Is<string>(arg => arg == Apply.Constants.AppSettings.LockStartTime))).Returns(LockStartTime);
            Mock.Get(appSettings).Setup(a => a.GetAppSetting(It.Is<string>(arg => arg == Apply.Constants.AppSettings.LockEndTime))).Returns(LockEndTime);
            Mock.Get(appSettings).Setup(a => a.GetAppSetting(It.Is<string>(arg => arg == Apply.Constants.AppSettings.AppCycleEqualConsiderDay))).Returns(AppCycleEqualConsiderDay);
            Mock.Get(appSettings).Setup(a => a.GetAppSetting(It.Is<string>(arg => arg == Apply.Constants.AppSettings.AppCycleEqualConsiderMonth))).Returns(AppCycleEqualConsiderMonth);
            Mock.Get(appSettings).Setup(a => a.GetAppSetting(It.Is<string>(arg => arg == Apply.Constants.AppSettings.AppCycleEqualConsiderTime))).Returns(AppCycleEqualConsiderTime);

            _appSettingsExtras = new AppSettingsExtras(appSettings);

            var lockStartTimeParts = LockStartTime.Split(':');
            var lockEndTimeParts = LockEndTime.Split(':');
            var appCycleEqualConsiderTimeParts = AppCycleEqualConsiderTime.Split(':');
            _lockStartTime = new TimeSpan(int.Parse(lockStartTimeParts[0]), int.Parse(lockStartTimeParts[1]), int.Parse(lockStartTimeParts[2]));
            _lockEndTime = new TimeSpan(int.Parse(lockEndTimeParts[0]), int.Parse(lockEndTimeParts[1]), int.Parse(lockEndTimeParts[2]));
            _appCycleEqualConsiderDay = int.Parse(AppCycleEqualConsiderDay);
            _appCycleEqualConsiderMonth = int.Parse(AppCycleEqualConsiderMonth);
            _appCycleEqualConsiderTime = new TimeSpan(int.Parse(appCycleEqualConsiderTimeParts[0]), int.Parse(appCycleEqualConsiderTimeParts[1]), int.Parse(appCycleEqualConsiderTimeParts[2]));
        }

        [Theory]
        [UnitTest]
        [InlineData(DayOfWeek.Saturday, OfferLockScenario.BeforeLockStart, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Sunday, OfferLockScenario.BeforeLockStart, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Monday, OfferLockScenario.BeforeLockStart, DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Tuesday, OfferLockScenario.BeforeLockStart, DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Wednesday, OfferLockScenario.BeforeLockStart, DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Thursday, OfferLockScenario.BeforeLockStart, DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Friday, OfferLockScenario.BeforeLockStart, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Saturday, OfferLockScenario.DuringLockWindow, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Sunday, OfferLockScenario.DuringLockWindow, DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Monday, OfferLockScenario.DuringLockWindow, DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Tuesday, OfferLockScenario.DuringLockWindow, DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Wednesday, OfferLockScenario.DuringLockWindow, DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Thursday, OfferLockScenario.DuringLockWindow, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Friday, OfferLockScenario.DuringLockWindow, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Saturday, OfferLockScenario.AfterLockEnd, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Sunday, OfferLockScenario.AfterLockEnd, DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Monday, OfferLockScenario.AfterLockEnd, DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Tuesday, OfferLockScenario.AfterLockEnd, DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Wednesday, OfferLockScenario.AfterLockEnd, DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Thursday, OfferLockScenario.AfterLockEnd, DayOfWeek.Sunday)]
        [InlineData(DayOfWeek.Friday, OfferLockScenario.AfterLockEnd, DayOfWeek.Sunday)]
        public void GetNextSendDate_ShouldPass(DayOfWeek dayOfWeek, OfferLockScenario scenario, DayOfWeek expectedDayOfWeek)
        {
            // Arrange
            var estToday = GetRandomDateEst(dayOfWeek);
            switch (scenario)
            {
                case OfferLockScenario.BeforeLockStart:
                    estToday = estToday.AddHours(-1);
                    break;
                case OfferLockScenario.DuringLockWindow:
                    estToday = estToday.AddHours(0.5);
                    break;
                case OfferLockScenario.AfterLockEnd:
                    estToday = estToday.AddHours(1.5);
                    break;
            }

            // Act
            var result = _appSettingsExtras.GetNextSendDate(TimeZoneInfo.ConvertTimeToUtc(estToday, Apply.Constants.TimeZone.Est));
            var estResult = TimeZoneInfo.ConvertTimeFromUtc(result.Value, Apply.Constants.TimeZone.Est);

            // Assert
            result.Value.Kind.Should().Be(DateTimeKind.Utc);
            estResult.TimeOfDay.Hours.Should().Be(_lockEndTime.Hours);
            estResult.DayOfWeek.Should().Be(expectedDayOfWeek);
        }

        [Fact]
        [UnitTest]
        public void GetApplicationCycleEqualConsiderationDate_ShouldPass()
        {
            // Arrange
            var year = _faker.Date.Past(2, DateTime.UtcNow).Year;

            // Act
            var result = _appSettingsExtras.GetApplicationCycleEqualConsiderationDate(year);
            var estResult = TimeZoneInfo.ConvertTimeFromUtc(result, Apply.Constants.TimeZone.Est);

            // Assert
            result.Kind.Should().Be(DateTimeKind.Utc);
            estResult.Year.Should().Be(year);
            estResult.Month.Should().Be(_appCycleEqualConsiderMonth);
            estResult.Day.Should().Be(_appCycleEqualConsiderDay);
            estResult.TimeOfDay.Should().Be(_appCycleEqualConsiderTime);
        }

        private DateTime GetRandomDateEst(DayOfWeek dayOfWeek)
        {
            var localNow = _faker.Date.Future();

            while (localNow.DayOfWeek != dayOfWeek)
                localNow = localNow.AddDays(1);

            return new DateTime(localNow.Year, localNow.Month, localNow.Day, _lockStartTime.Hours, _lockStartTime.Minutes, _lockStartTime.Seconds);
        }

        public enum OfferLockScenario
        {
            BeforeLockStart = 0,
            DuringLockWindow = 1,
            AfterLockEnd = 2
        }
    }
}
