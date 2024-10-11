using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Ocas.Common;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Client
{
    public partial class ApplyApiClient : IDisposable
    {
        private readonly HttpClient _http;
        private readonly ILogger _logger;
        private readonly JsonMediaTypeFormatter _formatter;
        private string _accessToken;
        private CultureInfo _locale = ApplyApiConstants.Localization.EnglishCanada;
        private string _partner;

        private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                }
        };

        public ApplyApiClient(HttpClient http, ILogger logger)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _http.Timeout = TimeSpan.FromMinutes(10);
            _http.DefaultRequestHeaders.ConnectionClose = true;
            _formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = JsonSerializerSettings
            };
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ApplyApiClient WithAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            return this;
        }

        public ApplyApiClient WithLocale(CultureInfo locale)
        {
            if (!ApplyApiConstants.Localization.SupportedLocalizations.Contains(locale))
                throw new Exception("Locale not supported by client.");

            _locale = locale;
            return this;
        }

        public ApplyApiClient WithPartner(string partner)
        {
            _partner = partner;
            return this;
        }

        #region IDisposable
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

            _http?.Dispose();
        }

        ~ApplyApiClient()
        {
            Dispose(false);
        }
        #endregion IDisposable

        protected async Task SendRequest(HttpMethod method, string url, object content = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (content != null)
            {
                request.Content = new ObjectContent(content.GetType(), content, _formatter);
            }
            else if (method != HttpMethod.Get)
            {
                var jsonContent = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            ConfigureHeaders(request);

            using (var response = await _http.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    await ThrowStatusCodeExceptionAsync(method, url, request, response);
                }
            }
        }

        protected async Task<T> SendRequest<T>(HttpMethod method, string url, object content = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (content != null)
            {
                if (content is Stream stream)
                {
                    request.Content = new StreamContent(stream);
                }
                else
                {
                    request.Content = new ObjectContent(content.GetType(), content, _formatter);
                }
            }
            else if (method != HttpMethod.Get)
            {
                var jsonContent = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            ConfigureHeaders(request);

            if (typeof(T) == typeof(Stream))
            {
                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    await ThrowStatusCodeExceptionAsync(method, url, request, response);
                }

                return (T)(await response.Content.ReadAsStreamAsync() as object);
            }

            if (typeof(T) == typeof(BinaryDocument))
            {
                var response = await _http.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    await ThrowStatusCodeExceptionAsync(method, url, request, response);
                }

                var data = await response.Content.ReadAsByteArrayAsync();

                var result = new BinaryDocument
                {
                    Data = data,
                    Name = response.Content.Headers.ContentDisposition.FileName,
                    MimeType = response.Content.Headers.ContentType.ToString()
                } as object;

                return (T)result;
            }

            using (var response = await _http.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    await ThrowStatusCodeExceptionAsync(method, url, request, response);
                }

                if (typeof(T) == typeof(string))
                {
                    return (T)((await response.Content.ReadAsStringAsync()) as object);
                }

                return await response.Content.ReadAsAsync<T>();
            }
        }

        protected Task<T> Get<T>(string url, object content = null)
        {
            return SendRequest<T>(HttpMethod.Get, url, content);
        }

        protected Task Post(string url, object content = null)
        {
            return SendRequest(HttpMethod.Post, url, content);
        }

        protected Task<T> Post<T>(string url, object content = null)
        {
            return SendRequest<T>(HttpMethod.Post, url, content);
        }

        protected Task Put(string url, object content = null)
        {
            return SendRequest(HttpMethod.Put, url, content);
        }

        protected Task<T> Put<T>(string url, object content = null)
        {
            return SendRequest<T>(HttpMethod.Put, url, content);
        }

        protected Task Delete(string url, object content = null)
        {
            return SendRequest(HttpMethod.Delete, url, content);
        }

        protected Task<T> Delete<T>(string url, object content = null)
        {
            return SendRequest<T>(HttpMethod.Delete, url, content);
        }

        protected void ConfigureHeaders(HttpRequestMessage request)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                return;
            }

            // Remove any old headers if they exist
            request.Headers.Remove("Authorization");
            request.Headers.AcceptLanguage.Clear();
            request.Headers.Remove("X-Partner");

            // Now add the new headers
            request.Headers.Add("Authorization", "Bearer " + _accessToken);
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(_locale.Name));
            if (!string.IsNullOrWhiteSpace(_partner)) request.Headers.Add("X-Partner", _partner);
        }

        private async Task ThrowStatusCodeExceptionAsync(HttpMethod method, string url, HttpRequestMessage request, HttpResponseMessage response)
        {
            try
            {
                await response.ThrowStatusCodeExceptionAsync();
            }
            catch (StatusCodeException e)
            {
                _logger.LogCritical(e, $"HTTP REQUEST {method} to {url} responded {response.StatusCode}.", request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync());
                _logger.LogCritical(e, $"HTTP RESPONSE {method} to {url} responded {response.StatusCode}.", response.Content is null ? string.Empty : await response.Content.ReadAsStringAsync());
                throw;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Unexpected exception.");
                throw;
            }
        }
    }

    internal static class HttpResponseMessageExtensions
    {
        public static async Task ThrowStatusCodeExceptionAsync(this HttpResponseMessage response)
        {
            var ocasHttpResponseMessage = await response.Content.ReadAsAsync<OcasHttpResponseMessage>();
            if (ocasHttpResponseMessage == null)
                throw new StatusCodeException(response.StatusCode);

            throw new StatusCodeException(response.StatusCode, ocasHttpResponseMessage);
        }
    }
}
