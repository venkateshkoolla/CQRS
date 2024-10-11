using System;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public abstract class BaseTest<T> : IDisposable, IAsyncLifetime
           where T : ApplyApiClient
    {
        private readonly TestServerFixture _testServerFixture;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly IdentityUserFixture _identityUserFixture;

        protected BaseTest(TestServerFixture testServerFixture, ModelFakerFixture modelFakerFixture, IdentityUserFixture identityUserFixture)
        {
            _testServerFixture = testServerFixture;
            _modelFakerFixture = modelFakerFixture;
            _identityUserFixture = identityUserFixture;
        }

        protected T Client { get; private set; }
        protected AlgoliaFixture AlgoliaClient { get; private set; }
        protected ApplyApiClientFixture ClientFixture { get; private set; }

        public async Task InitializeAsync()
        {
            Client = await _testServerFixture.CreateApplyApiClient() as T;
            AlgoliaClient = new AlgoliaFixture(_modelFakerFixture);
            ClientFixture = new ApplyApiClientFixture(Client, AlgoliaClient, _modelFakerFixture);
            await _identityUserFixture.InitializeAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
    }
}
