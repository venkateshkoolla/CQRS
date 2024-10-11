namespace Ocas.Domestic.Apply.Api.Services.UnitTests
{
    public class RequestCacheMock : RequestCache
    {
        public RequestCacheMock()
        {
            AddOrUpdate(TestConstants.Locale.EnglishCanada);
            AddOrUpdate(Constants.RequestCacheKeys.Partner, string.Empty);
        }
    }
}
