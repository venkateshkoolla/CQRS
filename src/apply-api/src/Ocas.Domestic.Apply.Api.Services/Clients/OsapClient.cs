using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Settings;

namespace Ocas.Domestic.Apply.Api.Services.Clients
{
    public class OsapClient : IOsapClient
    {
        private readonly ILogger _logger;
        private readonly IAppSettings _appSettings;

        public OsapClient(ILogger<OsapClient> logger, IAppSettings appSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<TokenResponse> GetApplicantToken(string accountNumber, DateTime birthDate)
        {
            var clientId = _appSettings.GetAppSetting(Constants.AppSettings.OsapClientId);
            var clientSecret = _appSettings.GetAppSetting(Constants.AppSettings.OsapClientSecret);
            var grantType = _appSettings.GetAppSetting(Constants.AppSettings.OsapGrantType);
            var apiUrl = new Uri(_appSettings.GetAppSetting(Constants.AppSettings.OsapApi));

            using (var client = new HttpClient())
            {
                client.BaseAddress = apiUrl;

                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { Constants.Osap.FormData.ClientId, clientId },
                    { Constants.Osap.FormData.ClientSecret, clientSecret },
                    { Constants.Osap.FormData.GrantType, grantType },
                    { Constants.Osap.FormData.AccountNumber, accountNumber },
                    { Constants.Osap.FormData.DateOfBirth, birthDate.ToStringOrDefault(Constants.DateFormat.OsapDate) }
                });

                using (var response = await client.PostAsync(Constants.Osap.Token, formContent))
                {
                    if (!response.IsSuccessStatusCode || response.Content is null)
                    {
                        var sb = new StringBuilder();
                        var message = sb.Append("POST to ").Append(apiUrl).Append(Constants.Osap.Token).Append(" responded ").Append(response.StatusCode).Append(".")
                            .AppendLine(response.Content is null ? string.Empty : await response.Content.ReadAsStringAsync())
                            .ToString();

                        _logger.LogWarning(message);
                        throw new Exception(message);
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                }
            }
        }
    }
}
