using System.Security.Principal;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators
{
    internal class TestObject
    {
        public TestObject()
        {
        }

        public TestObject(string val)
        {
            GenericString = val;
        }

        public string GenericString { get; set; }
        public IPrincipal GenericUser { get; set; }
    }
}
