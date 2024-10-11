using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ocas.Common;
using Ocas.Domestic.Apply.Api.Client;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class EtmsFixture
    {
        private const string BaseAddress = "https://etms.dev.ocas.ca/api/";

        public async Task FulfillTranscriptRequest(string referenceNumber, string applicationNumber, string birthDate)
        {
            var xml = Read("Ocas.Domestic.Apply.IntegrationTests.Resources.PESC.xml");
            xml = xml.Replace("{{APPLICATION_NUMBER}}", applicationNumber)
                     .Replace("{{REFERENCE_NUMBER}}", referenceNumber)
                     .Replace("{{BIRTH_DATE}}", birthDate);

            string accessToken;
            using (var client = new HttpClient())
            {
                // Get access token
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();

                var response = await client.PostAsync("auth/login", new StringContent($"UserName={TestConstants.Etms.GeorgianUser}&Password={TestConstants.Etms.GeorgianPassword}&grant_type=password"));

                if (!response.IsSuccessStatusCode)
                {
                    await ThrowStatusCodeExceptionAsync(response);
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                accessToken = responseData["access_token"];
            }

            using (var client = new HttpClient())
            {
                // Complete transcript request
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var transcriptRequestResponse = new TranscriptRequestResponse();
                transcriptRequestResponse.PescXml = xml;

                var json = JsonConvert.SerializeObject(transcriptRequestResponse);

                var response = await client.PostAsync($"transcriptrequests/{referenceNumber}/transcript", new StringContent(json));

                if (!response.IsSuccessStatusCode)
                {
                    await ThrowStatusCodeExceptionAsync(response);
                }
            }
        }

        private static string Read(string resourceName, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private class TranscriptRequestResponse
        {
            [JsonProperty(PropertyName = "PESCXML")]
            public string PescXml { get; set; }
        }

        private async Task ThrowStatusCodeExceptionAsync(HttpResponseMessage response)
        {
            var ocasHttpResponseMessage = await response.Content.ReadAsAsync<OcasHttpResponseMessage>();
            if (ocasHttpResponseMessage == null)
                throw new StatusCodeException(response.StatusCode);

            throw new StatusCodeException(response.StatusCode, ocasHttpResponseMessage);
        }
    }
}
