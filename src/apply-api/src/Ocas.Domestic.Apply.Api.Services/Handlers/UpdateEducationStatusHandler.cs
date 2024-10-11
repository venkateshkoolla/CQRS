using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Data.Extras;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class UpdateEducationStatusHandler : IRequestHandler<UpdateEducationStatus>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IDomesticContextExtras _domesticContextExtras;
        private readonly ILookupsCache _lookupsCache;

        public UpdateEducationStatusHandler(ILogger<UpdateEducationStatusHandler> logger, IDomesticContext domesticContext, ILookupsCache lookupsCache, IDomesticContextExtras domesticContextExtras)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _domesticContextExtras = domesticContextExtras ?? throw new ArgumentNullException(nameof(domesticContextExtras));
        }

        public async Task<Unit> Handle(UpdateEducationStatus request, CancellationToken cancellationToken)
        {
            var applicant = await _domesticContext.GetContact(request.ApplicantId) ??
                throw new NotFoundException($"Applicant {request.ApplicantId} not found");

            var modifiedBy = request.User.GetUpnOrEmail();
            applicant.ModifiedBy = modifiedBy;
            applicant.HighSchoolEnrolled = request.EnrolledInHighSchool;

            if (request.EnrolledInHighSchool.Value)
            {
                // Applicant is enrolled so when is graduation
                applicant.HighSchoolGraduated = null;
                applicant.HighSchoolGraduationDate = request.GraduationHighSchoolDate?.ToDateTime(Constants.DateFormat.YearMonthDashed);
            }
            else if (!request.EnrolledInHighSchool.Value)
            {
                // Applicant is not enrolled so has applicant graduated
                applicant.HighSchoolGraduated = request.GraduatedHighSchool;
                applicant.HighSchoolGraduationDate = null;
            }

            await _domesticContext.BeginTransaction();
            try
            {
                await _domesticContext.UpdateContact(applicant);

                var applications = await _domesticContext.GetApplications(applicant.Id);

                foreach (var application in applications)
                {
                    var hasChanges = await _domesticContextExtras.PatchBasisForAdmission(
                        application,
                        applicant,
                        modifiedBy,
                        await _lookupsCache.GetBasisForAdmissionsDto(Constants.Localization.EnglishCanada),
                        await _lookupsCache.GetCurrentsDto(Constants.Localization.EnglishCanada),
                        await _lookupsCache.GetApplicationCyclesDto());

                    if (hasChanges)
                    {
                        await _domesticContext.UpdateApplication(application);
                    }
                }

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            return Unit.Value;
        }
    }
}
