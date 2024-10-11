namespace Ocas.Domestic.Apply.TestFramework.Models
{
    public class TestUser
    {
        public string AccessToken { get; }
        public string Email { get; }
        public string IdToken { get; }
        public string Name { get; }
        public string Sub { get; }
        public string UserId => string.IsNullOrWhiteSpace(Sub) ? null : Sub.Split(':')[0];

        public TestUser(string sub, string idToken, string accessToken, string name, string email)
        {
            Sub = sub;
            IdToken = idToken;
            AccessToken = accessToken;
            Name = name;
            Email = email;
        }
    }
}
