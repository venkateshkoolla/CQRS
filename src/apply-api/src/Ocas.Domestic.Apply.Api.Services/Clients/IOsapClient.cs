using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Api.Services.Clients
{
    public interface IOsapClient
    {
        Task<TokenResponse> GetApplicantToken(string accountNumber, DateTime birthDate);
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
