namespace Ocas.Domestic.Apply.Models
{
    public class OsapToken
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
