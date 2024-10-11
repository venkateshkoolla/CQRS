using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class MonerisFixture
    {
        private static HttpClient _httpClient;
        private HttpClient HttpClient => _httpClient ?? (_httpClient = new HttpClient());

        public async Task<string> GetPaymentToken(string cardNumber)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var pageUri = $"https://esqa.moneris.com/HPPtoken/index.php?id={TestConstants.Moneris.HostedTokenizationKey}&pmmsg=true";
            var pageResp = await HttpClient.GetAsync(pageUri);
            var pageContent = await pageResp.Content.ReadAsStringAsync();
            var ticketMatch = Regex.Match(pageContent, "var\\sticket\\s\\=\\s\\'([a-zA-Z0-9]+)\\'\\;");
            var ticket = ticketMatch.Groups[1].Value;
            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "input_data", cardNumber },
                    { "hpt_id", TestConstants.Moneris.HostedTokenizationKey },
                    { "hpt_ticket", ticket },
                    { "doTokenize", "true" }
                });

            var result = await HttpClient.PostAsync("https://esqa.moneris.com/HPPtoken/request.php", formContent);
            var resultContent = await result.Content.ReadAsStringAsync();
            var tokenization = JsonConvert.DeserializeObject<TokenizationResponse>(resultContent);

            return tokenization.DataKey;
        }

        private class TokenizationResponse
        {
            [JsonProperty("dataKey")]
            public string DataKey { get; set; }

            [JsonProperty("bin")]
            public string Bin { get; set; }

            [JsonProperty("responseCode")]
            public string ResponseCode { get; set; }
        }
    }
}
