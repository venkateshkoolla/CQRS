using Microsoft.Extensions.Logging;
using Polly;

namespace Ocas.Domestic.Crm.Provider
{
    public static class PollyContextExtensions
    {
        public static readonly string LoggerKey = "Logger";

        public static Context WithLogger(this Context context, ILogger logger)
        {
            context[LoggerKey] = logger;
            return context;
        }

        public static bool TryGetLogger(this Context context, out ILogger logger)
        {
            if (context.TryGetValue(LoggerKey, out var loggerObject) && loggerObject is ILogger theLogger)
            {
                logger = theLogger;
                return true;
            }

            logger = null;
            return false;
        }
    }
}
