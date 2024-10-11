using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class CreateEducationHandler : IRequestHandler<CreateEducation, Education>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IDtoMapper _dtoMapper;
        private readonly ILookupsCache _lookupsCache;

        public CreateEducationHandler(ILogger<CreateEducationHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, IDtoMapper dtoMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<Education> Handle(CreateEducation request, CancellationToken cancellationToken)
        {
            var applicant = await _domesticContext.GetContact(request.ApplicantId) ?? throw new NotFoundException("Applicant does not exist.");

            if (request.Education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed) <= applicant.BirthDate)
            {
                throw new ValidationException($"Education.AttendedFrom must be after applicant's birth: {request.Education.AttendedFrom}");
            }

            // Force Ontario HS to have OEN (applicant's or new)
            if (request.Education.GetEducationType(await _lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await _lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization)) == EducationType.CanadianHighSchool)
            {
                await ValidateOntarioHighSchoolEditsAsync(request, applicant);
            }

            var educationBase = new Dto.EducationBase
            {
                ModifiedBy = request.User.GetUpnOrEmail()
            };

            await _dtoMapper.PatchEducation(educationBase, request.Education);

            Dto.Education education;
            await _domesticContext.BeginTransaction();
            try
            {
                education = await _domesticContext.CreateEducation(educationBase);
                await _domesticContext.UpdateCompletedSteps(request.ApplicantId);

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            return await _apiMapper.MapEducation(
                    education,
                    await _lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetUniversities(),
                    _domesticContext);
        }

        private async Task ValidateOntarioHighSchoolEditsAsync(CreateEducation request, Dto.Contact applicant)
        {
            if (request.Education.ProvinceId.HasValue)
            {
                var provinces = await _lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                if (provinces.FirstOrDefault(z => z.Id == request.Education.ProvinceId.Value)?.Code == Constants.Provinces.Ontario)
                {
                    // When applicant's OEN is set force education's to match
                    if (!string.IsNullOrEmpty(applicant.OntarioEducationNumber)
                        && applicant.OntarioEducationNumber != Constants.Education.DefaultOntarioEducationNumber
                        && applicant.OntarioEducationNumber != request.Education.OntarioEducationNumber)
                    {
                        request.Education.OntarioEducationNumber = applicant.OntarioEducationNumber;
                    }

                    // Cannot change OEN to another applicant's OEN
                    if (applicant.OntarioEducationNumber != request.Education.OntarioEducationNumber
                        && await _domesticContext.IsDuplicateOen(request.ApplicantId, request.Education.OntarioEducationNumber))
                    {
                        throw new ValidationException($"Applicant exists with {request.Education.OntarioEducationNumber}");
                    }
                }
            }
        }
    }
}
