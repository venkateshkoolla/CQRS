using System;
using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetOntarioHighSchoolCourseCodeValidator : AbstractValidator<GetOntarioHighSchoolCourseCode>
    {
        public GetOntarioHighSchoolCourseCodeValidator(IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(5, 6)
                .MustAsync(async (y, _) =>
                {
                    var courseCode = await domesticContext.GetOntarioHighSchoolCourseCode(y);
                    return courseCode?.Name.Equals(y, StringComparison.OrdinalIgnoreCase) == true;
                })
                .WithMessage("'{PropertyName}' must exist.");
        }
    }
}
