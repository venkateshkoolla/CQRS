using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class UpdateApplicantHandler : IRequestHandler<UpdateApplicant, Applicant>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IDtoMapper _dtoMapper;
        private readonly ILookupsCache _lookupsCache;

        public UpdateApplicantHandler(ILogger<UpdateApplicantHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, IDtoMapper dtoMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<Applicant> Handle(UpdateApplicant request, CancellationToken cancellationToken)
        {
            var current = await _domesticContext.GetContact(request.ApplicantId) ??
                throw new NotFoundException($"Applicant {request.ApplicantId} not found");

            if (current.Email != request.Applicant.Email)
            {
                var isDuplicate = await _domesticContext.IsDuplicateContact(request.ApplicantId, request.Applicant.Email);
                if (isDuplicate) throw new ConflictException($"Applicant exists with {request.Applicant.Email}");
            }

            if (current.FirstName != request.Applicant.FirstName
                || current.LastName != request.Applicant.LastName
                || current.BirthDate.ToStringOrDefault() != request.Applicant.BirthDate
                || current.Username != request.Applicant.UserName)
            {
                throw new ValidationException("Cannot change applicant locked fields");
            }

            if (!string.IsNullOrEmpty(request.Applicant.DateOfArrival))
            {
                // validate that DateOfArrival is after birthdate, when applicant was born outside Canada
                var countries = await _lookupsCache.GetCountries(Constants.Localization.EnglishCanada);
                var countryOfBirth = countries.First(x => x.Id == request.Applicant.CountryOfBirthId);

                if (countryOfBirth.Code != Constants.Countries.Canada)
                {
                    var dateOfArrival = request.Applicant.DateOfArrival.ToDateTime();
                    var birthDate = current.BirthDate;
                    var today = DateTime.UtcNow.ToDateInEstAsUtc();

                    if (!(dateOfArrival >= birthDate && dateOfArrival <= today))
                    {
                        throw new ValidationException($"Applicant.DateOfArrival is outside valid range: {request.Applicant.DateOfArrival}");
                    }
                }
            }

            await _dtoMapper.PatchContact(current, request.Applicant);
            current.ModifiedBy = request.User.GetUpnOrEmail();

            Contact result;
            await _domesticContext.BeginTransaction();
            try
            {
                result = await _domesticContext.UpdateContact(current);
                result.CompletedSteps = await _domesticContext.UpdateCompletedSteps(current.Id);

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            return _apiMapper.MapApplicant(
                result,
                await _lookupsCache.GetAboriginalStatuses(Constants.Localization.EnglishCanada),
                await _lookupsCache.GetTitles(Constants.Localization.EnglishCanada));
        }
    }
}
