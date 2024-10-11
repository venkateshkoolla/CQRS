using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class UpdateOntarioStudentCourseCreditValidator : AbstractValidator<UpdateOntarioStudentCourseCredit>
    {
        public UpdateOntarioStudentCourseCreditValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.OntarioStudentCourseCreditId)
                .NotEmpty()
                .Equal(x => x.OntarioStudentCourseCredit.Id);

            RuleFor(x => x.OntarioStudentCourseCredit)
                .NotNull()
                .SetValidator(new OntarioStudentCourseCreditValidator(lookupsCache, domesticContext));
        }
    }
}
