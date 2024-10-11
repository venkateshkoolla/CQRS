﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class PurgeLookupsHandler : IRequestHandler<PurgeLookups>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly IUserAuthorization _userAuthorization;

        public PurgeLookupsHandler(ILogger<PurgeLookupsHandler> logger, ILookupsCache lookupsCache, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public Task<Unit> Handle(PurgeLookups request, CancellationToken cancellationToken)
        {
            if (_userAuthorization.GetUserType(request.User) != UserType.OcasUser)
                throw new ForbiddenException("Only Ocas users can purge lookups.");

            var keys = request.Filter?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            _lookupsCache.PurgeAllLookups(keys);

            return Unit.Task;
        }
    }
}
