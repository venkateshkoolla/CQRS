namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests
{
    public class RequestCacheMock : RequestCache
    {
        public RequestCacheMock()
        {
            AddOrUpdate(TestConstants.Locale.EnglishCanada);
        }
    }
}
