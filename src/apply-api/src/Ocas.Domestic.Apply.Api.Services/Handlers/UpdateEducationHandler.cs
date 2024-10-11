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
    public class UpdateEducationHandler : IRequestHandler<UpdateEducation, Education>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly IDtoMapper _dtoMapper;
        private readonly ILookupsCache _lookupsCache;

        public UpdateEducationHandler(ILogger<UpdateEducationHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, IDtoMapper dtoMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<Education> Handle(UpdateEducation request, CancellationToken cancellationToken)
        {
            var applicant = await _domesticContext.GetContact(request.ApplicantId)
                ?? throw new NotFoundException("Applicant does not exist.");

            if (request.Education.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed) <= applicant.BirthDate)
            {
                throw new ValidationException($"Education.AttendedFrom must be after applicant's birth: {request.Education.AttendedFrom}");
            }

            var education = await _domesticContext.GetEducation(request.EducationId)
                ?? throw new NotFoundException("Education does not exist");

            // Client is trying to change an education record that doesn't belong to them
            if (education.ApplicantId != request.ApplicantId) throw new NotAuthorizedException("Education does not belong to this applicant");

            var dbEducationType = (await _apiMapper.MapEducation(
                    education,
                    await _lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetUniversities(),
                    _domesticContext)).GetEducationType(await _lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await _lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization));
            var requestEducationType = request.Education.GetEducationType(await _lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await _lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization));

            // Client is trying to change education type
            if (dbEducationType != requestEducationType)
            {
                throw new ValidationException("Cannot change EducationType");
            }

            if ((request.Education.InstituteId != education.InstituteId || request.Education.InstituteName != education.InstituteName || request.Education.ProvinceId != education.ProvinceId)
                && education.HasTranscripts)
            {
                throw new ValidationException("Cannot change province or institute on transcript requested.");
            }

            if (requestEducationType == EducationType.CanadianHighSchool)
            {
                await ValidateOntarioHighSchoolEditsAsync(request, applicant, education);
            }

            if (requestEducationType == EducationType.CanadianUniversity)
            {
                await ValidateCanadianUniversityEditsAsync(request, education);
            }

            //Set education modified by to current user
            education.ModifiedBy = request.User.GetUpnOrEmail();

            await _dtoMapper.PatchEducation(education, request.Education);

            Dto.Education result;
            await _domesticContext.BeginTransaction();
            try
            {
                result = await _domesticContext.UpdateEducation(education);
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

        private async Task ValidateOntarioHighSchoolEditsAsync(UpdateEducation request, Dto.Contact applicant, Dto.Education education)
        {
            // Force Ontario HS to have OEN (applicant's or new)
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

            // Validate if institute has changed that it can be chosen
            if (request.Education.InstituteId.HasValue && education.InstituteId.HasValue && request.Education.InstituteId.Value != education.InstituteId.Value)
            {
                var highSchools = await _lookupsCache.GetHighSchools(Constants.Localization.FallbackLocalization);
                var requestedHighSchool = highSchools.FirstOrDefault(h => h.Id == request.Education.InstituteId.Value);
                if (!requestedHighSchool.ShowInEducation) throw new ValidationException($"'Institute Id' is not an Ontario high school: {request.Education.InstituteId.Value}");
            }
        }

        private async Task ValidateCanadianUniversityEditsAsync(UpdateEducation request, Dto.Education education)
        {
            // Validate if institute has changed that it can be chosen
            if (request.Education.InstituteId.HasValue && education.InstituteId.HasValue && request.Education.InstituteId.Value != education.InstituteId.Value)
            {
                var universities = await _lookupsCache.GetUniversities();
                var requestedUni = universities.FirstOrDefault(u => u.Id == request.Education.InstituteId.Value);
                if (!requestedUni.ShowInEducation) throw new ValidationException($"'Institute Id' is not an Ontario university: {request.Education.InstituteId.Value}");
            }
        }
    }
}
