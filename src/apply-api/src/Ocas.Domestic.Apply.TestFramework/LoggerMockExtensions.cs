using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ocas.Domestic.Apply.TestFramework
{
    public static class LoggerMockExtensions
    {
        public static void VerifyLogInformation<T>(this Mock<ILogger<T>> loggerMock, string message, string failMessage = null)
        {
            loggerMock.VerifyLog(LogLevel.Information, message, Times.Once(), failMessage);
        }

        public static void VerifyLogWarning<T>(this Mock<ILogger<T>> loggerMock, string message, string failMessage = null)
        {
            loggerMock.VerifyLog(LogLevel.Warning, message, Times.Once(), failMessage);
        }

        public static void VerifyLogError<T>(this Mock<ILogger<T>> loggerMock, string message, string failMessage = null)
        {
            loggerMock.VerifyLog(LogLevel.Error, message, Times.Once(), failMessage);
        }

        public static void VerifyLogCritical<T>(this Mock<ILogger<T>> loggerMock, string message, string failMessage = null)
        {
            loggerMock.VerifyLog(LogLevel.Critical, message, Times.Once(), failMessage);
        }

        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, string failMessage = null)
        {
            loggerMock.VerifyLog(level, message, Times.Once(), failMessage);
        }

        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times times, string failMessage = null)
        {
            loggerMock.Verify(
                l => l.Log(
                    level,
                It.IsAny<EventId>(),
                It.Is<object>(o => o.ToString() == message),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
                times,
                failMessage);
        }
    }
}
