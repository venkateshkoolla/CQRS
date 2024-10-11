using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetUniversitiesHandler : IRequestHandler<GetUniversities, IList<University>>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;

        public GetUniversitiesHandler(
            ILogger<GetUniversitiesHandler> logger,
            ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<IList<University>> Handle(GetUniversities request, CancellationToken cancellationToken)
        {
            var universities = await _lookupsCache.GetUniversities();
            return universities.Where(u => u.ShowInEducation).ToList();
        }
    }
}
