using System;
using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class AcademicRecordBaseValidator : AbstractValidator<AcademicRecordBase>
    {
        public AcademicRecordBaseValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ApplicantId)
                .NotEmpty();

            RuleFor(x => x.DateCredentialAchieved)
                .Must(y => y.IsDate())
                .WithMessage("'{PropertyName}' must be a valid date.")
                .Must(y => y.ToDateTime() <= DateTime.UtcNow)
                .WithMessage("'{PropertyName}' must be in the past.")
                .Unless(y => string.IsNullOrEmpty(y.DateCredentialAchieved));

            RuleFor(x => x.SchoolId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var highSchools = await lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada);

                    return highSchools.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.SchoolId}")
                .When(y => y.SchoolId.HasValue);

            RuleFor(x => x.CommunityInvolvementId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var communityInvolvements = await lookupsCache.GetCommunityInvolvements(Constants.Localization.EnglishCanada);

                    return communityInvolvements.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.CommunityInvolvementId}");

            RuleFor(x => x.LiteracyTestId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var literacyTestId = await lookupsCache.GetLiteracyTests(Constants.Localization.EnglishCanada);

                    return literacyTestId.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.LiteracyTestId}");

            RuleFor(x => x.HighestEducationId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var highestEducationId = await lookupsCache.GetHighestEducations(Constants.Localization.EnglishCanada);

                    return highestEducationId.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.HighestEducationId}");

            RuleFor(x => x.HighSkillsMajorId)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var highSkillsMajorId = await lookupsCache.GetHighSkillsMajors(Constants.Localization.EnglishCanada);

                    return highSkillsMajorId.Any(z => z.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.HighSkillsMajorId}")
                .When(y => y.HighSkillsMajorId.HasValue);
        }
    }
}
