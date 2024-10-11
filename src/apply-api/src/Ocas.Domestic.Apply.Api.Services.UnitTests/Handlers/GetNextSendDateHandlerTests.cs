using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Core.Settings;
using Ocas.Domestic.AppSettings.Extras;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetNextSendDateHandlerTests
    {
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly ILogger<GetNextSendDateHandler> _logger;

        public GetNextSendDateHandlerTests()
        {
            _logger = Mock.Of<ILogger<GetNextSendDateHandler>>();

            var appSettings = Mock.Of<IAppSettings>();
            Mock.Get(appSettings).Setup(a => a.GetAppSetting(It.Is<string>(arg => arg == Constants.AppSettings.LockStartTime))).Returns(TestConstants.AppSettings.LockStartTime);
            Mock.Get(appSettings).Setup(a => a.GetAppSetting(It.Is<string>(arg => arg == Constants.AppSettings.LockEndTime))).Returns(TestConstants.AppSettings.LockEndTime);
            _appSettingsExtras = new AppSettingsExtras(appSettings);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetNextSendDate_ShouldPass()
        {
            // Arrange
            var estToday = DateTime.UtcNow.ToDateInEstAsUtc().Date.AddDays(-1);
            var request = new GetNextSendDate
            {
                UtcNow = estToday.ToUniversalTime()
            };

            var handler = new GetNextSendDateHandler(_logger, _appSettingsExtras);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}
