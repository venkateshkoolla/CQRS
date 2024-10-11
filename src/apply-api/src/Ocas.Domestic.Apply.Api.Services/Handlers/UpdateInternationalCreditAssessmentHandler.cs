using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class UpdateInternationalCreditAssessmentHandler : IRequestHandler<UpdateInternationalCreditAssessment>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCache _lookupsCache;

        public UpdateInternationalCreditAssessmentHandler(ILogger<UpdateInternationalCreditAssessmentHandler> logger, IDomesticContext domesticContext, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<Unit> Handle(UpdateInternationalCreditAssessment request, CancellationToken cancellationToken)
        {
            var contact = await _domesticContext.GetContact(request.ApplicantId) ??
                throw new NotFoundException($"Applicant {request.ApplicantId} not found");

            var statuses = await _lookupsCache.GetInternationalCreditAssessmentStatuses(Constants.Localization.EnglishCanada);
            var completingId = statuses.First(x => x.Code == Constants.IntlCreditAssessments.Completing).Id;

            var intlCredits = await _domesticContext.GetInternationalCreditAssessments(contact.Id);
            var intlCredit = intlCredits.FirstOrDefault();

            var modifiedBy = request.User.GetUpnOrEmail();

            if (string.IsNullOrWhiteSpace(request.IntlCredentialAssessment?.IntlReferenceNumber))
            {
                if (intlCredit is null)
                    return Unit.Value;

                intlCredit.HaveHighSchoolCourseEvaluation = false;
                intlCredit.HavePostSecondaryEvaluation = false;
                intlCredit.ReferenceNumber = null;
                intlCredit.CredentialEvaluationAgencyId = null;
                intlCredit.InternationalCreditAssessmentStatusId = completingId;
                intlCredit.ModifiedBy = modifiedBy;

                await _domesticContext.UpdateInternationalCreditAssessment(intlCredit);
            }
            else if (intlCredit is null)
            {
                var intlCreditBase = new Dto.InternationalCreditAssessmentBase
                {
                    ApplicantId = request.ApplicantId,
                    HaveHighSchoolCourseEvaluation = true,
                    HavePostSecondaryEvaluation = true,
                    ReferenceNumber = request.IntlCredentialAssessment.IntlReferenceNumber,
                    CredentialEvaluationAgencyId = request.IntlCredentialAssessment.IntlEvaluatorId,
                    InternationalCreditAssessmentStatusId = completingId,
                    ModifiedBy = modifiedBy
                };

                await _domesticContext.CreateInternationalCreditAssessment(intlCreditBase);
            }
            else
            {
                intlCredit.HaveHighSchoolCourseEvaluation = true;
                intlCredit.HavePostSecondaryEvaluation = true;
                intlCredit.ReferenceNumber = request.IntlCredentialAssessment.IntlReferenceNumber;
                intlCredit.CredentialEvaluationAgencyId = request.IntlCredentialAssessment.IntlEvaluatorId;
                intlCredit.InternationalCreditAssessmentStatusId = completingId;
                intlCredit.ModifiedBy = modifiedBy;

                await _domesticContext.UpdateInternationalCreditAssessment(intlCredit);
            }

            return Unit.Value;
        }
    }
}
