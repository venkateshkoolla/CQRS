using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class UpsertAcademicRecordValidator : AbstractValidator<UpsertAcademicRecord>
    {
        public UpsertAcademicRecordValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.AcademicRecord)
                .NotNull()
                .SetValidator(new AcademicRecordBaseValidator(lookupsCache));
        }
    }
}
