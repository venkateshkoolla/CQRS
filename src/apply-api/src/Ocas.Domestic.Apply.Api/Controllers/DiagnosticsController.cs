using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TemplateProcessors;

namespace Ocas.Domestic.Apply.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRazorTemplateService _razorTemplateService;

        public DiagnosticsController(
            ILogger<DiagnosticsController> logger,
            IRazorTemplateService razorTemplateService,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _razorTemplateService = razorTemplateService;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult ServerTime()
        {
            return Ok(new
            {
                UTC = DateTime.UtcNow,
                Local = DateTimeOffset.Now
            });
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult Logging()
        {
            _logger.LogDebug("Diagnostic test - Debug");
            _logger.LogInformation("Diagnostic test - Info");
            _logger.LogWarning("Diagnostic test - Warn");
            _logger.LogError("Diagnostic test - Error");
            _logger.LogCritical("Diagnostic test - Fatal");
            return Ok("OK");
        }

        [HttpGet("[action]")]
        public IActionResult Authorization()
        {
            return Ok("Authorized!");
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<Dictionary<string, bool>>> HealthCheck()
        {
            const string ids3WellKnown = "/.well-known/jwks";
            const string ids4WellKnown = "/.well-known/openid-configuration/jwks";
            var items = new Dictionary<string, bool>();
            var anyFailures = false;

            //Applicant IDP
            const string applicantIdp = "ApplicantIdp";
            try
            {
                using (var client = new HttpClient())
                {
                    var applicantIdpResponse = await client.GetAsync(new Uri(_configuration["ocas:idpvr:authority"] + ids3WellKnown));

                    if (applicantIdpResponse.IsSuccessStatusCode && (applicantIdpResponse.Content.Headers.ContentLength != null || applicantIdpResponse.Content.Headers.ContentLength != 0))
                    {
                        items.Add(applicantIdp, true);
                    }
                    else
                    {
                        anyFailures = true;
                        items.Add(applicantIdp, false);
                    }
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"{applicantIdp} health check failure");
                anyFailures = true;
                items.Add(applicantIdp, false);
            }

            //IDS
            const string IDS = "IDS";
            try
            {
                using (var client = new HttpClient())
                {
                    var idsResponse = await client.GetAsync(new Uri(_configuration["ocas:idsvr:authority"] + ids3WellKnown));

                    if (idsResponse.IsSuccessStatusCode && (idsResponse.Content.Headers.ContentLength != null || idsResponse.Content.Headers.ContentLength != 0))
                    {
                        items.Add(IDS, true);
                    }
                    else
                    {
                        anyFailures = true;
                        items.Add(IDS, false);
                    }
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"{IDS} health check failure");
                anyFailures = true;
                items.Add(IDS, false);
            }

            //IDS4
            const string IDS4 = "IDS4";
            try
            {
                using (var client = new HttpClient())
                {
                    var idsResponse = await client.GetAsync(new Uri(_configuration["ocas:ids4:authority"] + ids4WellKnown));

                    if (idsResponse.IsSuccessStatusCode && (idsResponse.Content.Headers.ContentLength != null || idsResponse.Content.Headers.ContentLength != 0))
                    {
                        items.Add(IDS4, true);
                    }
                    else
                    {
                        anyFailures = true;
                        items.Add(IDS4, false);
                    }
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"{IDS4} health check failure");
                anyFailures = true;
                items.Add(IDS4, false);
            }

            if (anyFailures)
                return StatusCode((int)HttpStatusCode.InternalServerError, items);

            return Ok(items);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "BO")]
        public async Task<IActionResult> EvoPdf()
        {
            var pdfBytes = await _razorTemplateService.GenerateTestPdfAsync();
            _logger.LogInformation("EvoPdf Verified, byte count: '{0}'", pdfBytes.Length);
            return Ok("Ok");
        }
    }
}
