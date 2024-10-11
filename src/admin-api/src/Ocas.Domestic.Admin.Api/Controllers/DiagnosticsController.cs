using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ocas.Domestic.Apply.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public DiagnosticsController(
            ILogger<DiagnosticsController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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
    }
}
