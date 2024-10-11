using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class CreateOntarioStudentCourseCreditValidator : AbstractValidator<CreateOntarioStudentCourseCredit>
    {
        public CreateOntarioStudentCourseCreditValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.OntarioStudentCourseCredit)
                .SetValidator(new OntarioStudentCourseCreditBaseValidator(lookupsCache, domesticContext));
        }
    }
}