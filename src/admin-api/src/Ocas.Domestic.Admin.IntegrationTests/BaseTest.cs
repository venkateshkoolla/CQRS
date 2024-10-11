using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Admin.Api.Client;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.TestFramework;
using Serilog;
using Serilog.Extensions.Logging;
using Xunit;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public abstract class BaseTest : IDisposable, IAsyncLifetime
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly TestServerFixture _testServerFixture;
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _modelFakerFixture;

        protected BaseTest(TestServerFixture testServerFixture, DataFakerFixture dataFakerFixture, IdentityUserFixture identityUserFixture, AdminTestFramework.ModelFakerFixture modelFakerFixture)
        {
            _dataFakerFixture = dataFakerFixture;
            _testServerFixture = testServerFixture;
            _identityUserFixture = identityUserFixture;
            _modelFakerFixture = modelFakerFixture;
        }

        protected AdminApiClient Client { get; private set; }
        protected AlgoliaFixture AlgoliaClient { get; private set; }
        protected ApplyApiClient ApplyApiClient { get; private set; }
        protected ApplyApiClientFixture ApplyClientFixture { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            Client = await _testServerFixture.CreateAdminApiClient();
            AlgoliaClient = new AlgoliaFixture(_modelFakerFixture);
            ApplyApiClient = InitializeApplyApiClient();
            ApplyClientFixture = new ApplyApiClientFixture(ApplyApiClient, AlgoliaClient, _modelFakerFixture);
            await _identityUserFixture.InitializeAsync();
        }

        protected ApplyApiClient InitializeApplyApiClient()
        {
            var logger = new SerilogLoggerProvider(Log.Logger).CreateLogger(nameof(ApplyApiClientFixture));
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://applyapi-ci.dev.ontariocolleges.ca/api/v1/")
            };
            return new ApplyApiClient(httpClient, logger);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Client?.Dispose();
            Client = null;
        }

        protected Task<Program> CreateNewProgram()
        {
            var programBase = _modelFakerFixture.GetProgramBase().Generate();
            programBase.Intakes = _modelFakerFixture.GetProgramIntake(programBase).Generate(3);

            return Client.CreateProgram(programBase);
        }
    }
}
