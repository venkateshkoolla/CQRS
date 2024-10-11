using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class DeleteOntarioStudentCourseCreditValidator : AbstractValidator<DeleteOntarioStudentCourseCredit>
    {
        public DeleteOntarioStudentCourseCreditValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.OntarioStudentCourseCreditId)
                .NotEmpty();
        }
    }
}
