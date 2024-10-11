using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Api.Services;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Services.Handlers
{
    public class GetCollegesHandler : IRequestHandler<GetColleges, IList<College>>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;
        private readonly IUserAuthorization _userAuthorization;

        public GetCollegesHandler(
            ILogger<GetCollegesHandler> logger,
            ILookupsCache lookupsCache,
            RequestCache requestCache,
            IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<IList<College>> Handle(GetColleges request, CancellationToken cancellationToken)
        {
            var colleges = await _lookupsCache.GetColleges(_locale);

            if (_userAuthorization.GetUserType(request.User) == UserType.CollegeUser)
            {
                // college users only get their college
                colleges = colleges.Where(x => x.Code == request.User.GetPartnerId()).ToList();
            }

            return colleges;
        }
    }
}