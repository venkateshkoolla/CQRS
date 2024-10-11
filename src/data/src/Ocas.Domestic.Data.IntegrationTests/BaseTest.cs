using System.Threading.Tasks;
using Ocas.Domestic.Data.TestFramework;
using Serilog;
using Serilog.Extensions.Logging;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public abstract class BaseTest : IAsyncLifetime
    {
        protected IDomesticContext Context { get; set; }
        protected DataFakerFixture DataFakerFixture { get; }

        protected BaseTest()
        {
            DataFakerFixture = XunitInjectionCollection.DataFakerFixture;
        }

        public async Task InitializeAsync()
        {
            if (Context != null) return;

            var log = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.Trace()
                .CreateLogger();

            var config = new DomesticContextConfig();
            await config.InitializeAsync();
            Context = new DomesticContext(config, new SerilogLoggerProvider(log).CreateLogger(nameof(BaseTest)));
        }

        public Task DisposeAsync()
        {
            try
            {
                Context.Dispose();
            }
            catch
            {
                // transactions try to rollback when disposed,
                // but since we don't actually support transactions,
                // they just throw exceptions
            }

            return Task.CompletedTask;
        }
    }
}
